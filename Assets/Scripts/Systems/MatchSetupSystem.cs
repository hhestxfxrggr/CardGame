using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private CardSystem cardSystem;
    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        cardSystem.Setup(heroData.Deck);

        RefillManaGA refillManaGA = new();

        ActionSystem.Instance.Perform(refillManaGA, () =>
        {
            DrawCardGA drawCardGA = new(5);
            ActionSystem.Instance.Perform(drawCardGA);
        });
    }
}
