using TMPro;
using UnityEngine;

public class CardPileUI : MonoBehaviour
{
    [SerializeField] private CardSystem cardSystem;
    [SerializeField] private CardsUI cardsUI;
    [SerializeField] private DeckSystem deckSystem;

    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private TMP_Text drawCountText;
    [SerializeField] private TMP_Text discardCountText;
    [SerializeField] private TMP_Text removedCountText;

    private void OnEnable()
    {
        cardSystem.OnCardsChanged += UpdateCount;
        deckSystem.OnDeckChanged += UpdateDeckCount;
        UpdateCount();
    }

    private void OnDisable()
    {
        cardSystem.OnCardsChanged -= UpdateCount;
        deckSystem.OnDeckChanged -= UpdateDeckCount;
    }

    public void UpdateCount()
    {
        drawCountText.text = cardSystem.GetCards(CardPileType.Draw).Count.ToString();

        discardCountText.text = cardSystem.GetCards(CardPileType.Discard).Count.ToString();

        removedCountText.text = cardSystem.GetCards(CardPileType.Removed).Count.ToString();
    }

    public void UpdateDeckCount()
    {
        deckCountText.text = deckSystem.OriginalDeck.Count.ToString();
    }

    public void OpenDrawPile()
    {
        cardsUI.Open(CardPileType.Draw);
    }

    public void OpenDiscardPile()
    {
        cardsUI.Open(CardPileType.Discard);
    }

    public void OpenRemovedPile()
    {
        cardsUI.Open(CardPileType.Removed);
    }

    public void OpenOriginalDeck()
    {
        cardsUI.OpenOriginalDeck();
    }
}
