using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> deckData;
    [SerializeField] private CardSystem cardSystem;
    private void Start()
    {
        cardSystem.Setup(deckData);
        DrawCardGA drawCardGA = new DrawCardGA(5);
        ActionSystem.Instance.Perform(drawCardGA);
    }
}
