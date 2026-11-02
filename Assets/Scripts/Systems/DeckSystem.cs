using System.Collections.Generic;
using UnityEngine;

public class DeckSystem : MonoBehaviour
{
    private readonly List<CardData> originalDeck = new();
    public IReadOnlyList<CardData> OriginalDeck => originalDeck.AsReadOnly();
    public event System.Action OnDeckChanged;
    public void Setup(IReadOnlyList<CardData> startingDeck)
    {
        originalDeck.Clear();

        foreach (CardData cardData in startingDeck)
        {
            originalDeck.Add(cardData);
        }

        OnDeckChanged?.Invoke();
    }

    public void AddCard(CardData cardData)
    {
        originalDeck.Add(cardData);
        OnDeckChanged?.Invoke();
    }

    public void RemoveCardAt(int index)
    {
        originalDeck.RemoveAt(index);
        OnDeckChanged?.Invoke();
    }
}
