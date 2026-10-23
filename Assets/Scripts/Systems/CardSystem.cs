using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원본은 싱글톤으로 선언
public class CardSystem : MonoBehaviour
{
    [SerializeField] private HandView handView;
    [SerializeField] private CardViewCreator cardViewCreator;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    private readonly List<Card> drawPile = new();
    private readonly List<Card> disCardPile = new();
    private readonly List<Card> hand = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.AttachPerformer<CreateCombatCardGA>(CreateCombatCardPerformer);
        ActionSystem.AttachPerformer<RemoveCombatCardGA>(RemoveCombatCardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.DetachPerformer<CreateCombatCardGA>();
        ActionSystem.DetachPerformer<RemoveCombatCardGA>();

    }

    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
    }


    private IEnumerator DrawCardsPerformer(DrawCardGA drawCardGA)
    {
        int actualAmount = Mathf.Min(drawCardGA.Amount, drawPile.Count);
        int notDrawAmount = drawCardGA.Amount - actualAmount;

        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        if (notDrawAmount > 0)
        {
            RefillDeck();

            for (int i = 0; i < notDrawAmount; i++)
            {
                if (drawPile.Count > 0)
                    yield return DrawCard();
            }
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach (var card in hand)
        {
            disCardPile.Add(card);
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }

        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);
        disCardPile.Add(playCardGA.Card);

        yield return DiscardCard(cardView);

        SpendManaGA spendManaGA = new SpendManaGA(playCardGA.Card.PlayMana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        if (playCardGA.Card.RequiresManualTarget)
        {
            foreach (var effect in playCardGA.Card.ManualTargetEffects)
            {
                if (effect == null)
                    continue;

                PerformEffectGA performEffectGA = new PerformEffectGA(effect, new() { playCardGA.ManualTarget });
                ActionSystem.Instance.AddReaction(performEffectGA);
            }
        }

        foreach (var effectWrapper in playCardGA.Card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGA performEffectGA = new PerformEffectGA(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    private IEnumerator CreateCombatCardPerformer(CreateCombatCardGA createCombatCardGA)
    {
        for (int i = 0; i < createCombatCardGA.Amount; i++)
        {
            yield return CreateCombatCard(createCombatCardGA.CardData, createCombatCardGA.Destination);
        }
    }

    private IEnumerator RemoveCombatCardPerformer(RemoveCombatCardGA action)
    {
        List<Card> pile = GetPile(action.Source);

        if (pile == null)
            yield break;

        int removeAmount = Mathf.Min(action.Amount, pile.Count);

        for (int i = 0; i < removeAmount; i++)
        {
            int index = Random.Range(0, pile.Count);
            Card card = pile[index];

            RemoveCombatCard(card);
        }

        yield break;
    }

    public IEnumerator CreateCombatCard(CardData cardData, CardPileType destination)
    {
        if (cardData == null)
            yield break;

        // 기존 카드와 별개인 전투용 카드 한 장 생성
        Card card = new(cardData);

        switch (destination)
        {
            case CardPileType.Draw:
                drawPile.Add(card);
                break;

            case CardPileType.Hand:
                hand.Add(card);
                CardView cardView = cardViewCreator.CreatCardView(card, drawPilePoint.position, drawPilePoint.rotation);
                yield return handView.AddCard(cardView);
                break;

            case CardPileType.Discard:
                disCardPile.Add(card);
                break;
        }
    }

    public bool RemoveCombatCard(Card card)
    {
        if (card == null)
            return false;

        // 손패에서 제거한 경우 화면도 정리
        if (hand.Remove(card))
        {
            CardView cardView = handView.RemoveCard(card);

            if (cardView != null)
            {
                cardViewCreator.ReturnCardView(cardView);
            }

            return true;
        }

        if (drawPile.Remove(card))
            return true;

        if (disCardPile.Remove(card))
            return true;

        return false;
    }

    private List<Card> GetPile(CardPileType pileType)
    {
        return pileType switch
        {
            CardPileType.Draw => drawPile,
            CardPileType.Hand => hand,
            CardPileType.Discard => disCardPile,
            _ => null
        };
    }

    private IEnumerator DrawCard()
    {
        Card card = drawPile.Draw();
        hand.Add(card);
        CardView cardView = cardViewCreator.CreatCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        drawPile.AddRange(disCardPile);
        disCardPile.Clear();
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        if (cardView == null)
            yield break;

        // 손패 재배치 등 기존 트윈 종료
        cardView.transform.DOKill();

        // 호버용 확대 카드 숨기기
        CardViewHoverSystem.Instance.Hide();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(cardView.transform.DOMove(discardPilePoint.position, 0.15f));
        sequence.Join(cardView.transform.DOScale(Vector3.zero, 0.15f));

        yield return sequence.WaitForCompletion();

        // Destroy 대신 풀에 반환
        cardViewCreator.ReturnCardView(cardView);
    }
}
