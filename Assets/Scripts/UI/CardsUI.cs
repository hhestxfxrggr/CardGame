using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardsUI : MonoBehaviour
{
    [SerializeField] private DeckSystem deckSystem;
    [SerializeField] private CardSystem cardSystem;

    [SerializeField] private GameObject panel;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private CardUI cardUIPrefab;
    
    private readonly List<CardUI> cardUIs = new();
    private readonly Queue<CardUI> pool = new();
  
    public void OpenOriginalDeck()
    {
        panel.SetActive(true);
        UpdateOriginalDeckUI();
    }

    public void Open(CardPileType pileType)
    {
        panel.SetActive(true);
        UpdatePileCardsUI(pileType);
    }

    public void Close()
    {
        ClearCardsUI();
        panel.SetActive(false);
    }

    public void UpdateOriginalDeckUI()
    {
        ClearCardsUI();

        IReadOnlyList<CardData> cards = deckSystem.OriginalDeck;
       
        foreach (CardData cardData in cards)
        {
            CardUI cardUI = GetCardUI();

            cardUI.Setup(cardData);
            cardUI.gameObject.SetActive(true);

            cardUIs.Add(cardUI);
        }
    }

    public void UpdatePileCardsUI(CardPileType pileType)
    {
        ClearCardsUI();

        IReadOnlyList<Card> cards = cardSystem.GetCards(pileType);
     
        foreach (Card card in cards)
        {
            CardUI cardUI = GetCardUI();

            cardUI.Setup(card);
            cardUI.gameObject.SetActive(true);

            cardUIs.Add(cardUI);
        }
    }

    private CardUI GetCardUI()
    {
        CardUI cardUI;

        if (pool.Count > 0)
        {
            cardUI = pool.Dequeue();
        }
        else
        {
            cardUI = Instantiate(cardUIPrefab, contentRoot);
            cardUI.gameObject.SetActive(false);
        }
        
        cardUI.transform.SetAsLastSibling();

        return cardUI;
    }

    private void ClearCardsUI()
    {
        foreach (CardUI cardUI in cardUIs)
        {
            cardUI.gameObject.SetActive(false);
            pool.Enqueue(cardUI);
        }

        cardUIs.Clear();
    }

}
