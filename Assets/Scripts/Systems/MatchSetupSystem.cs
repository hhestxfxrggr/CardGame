using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private CardSystem cardSystem;
    [SerializeField] private DeckSystem deckSystem;
    [SerializeField] private HeroData heroData;
    [SerializeField] private PerkData perkData;
    [SerializeField] private List<EnemyData> enemyDatas;
    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        EnemySystem.Instance.Setup(enemyDatas);
        deckSystem.Setup(heroData.Deck);
        cardSystem.Setup(deckSystem.OriginalDeck);
        PerkSystem.Instance.AddPerk(new Perk(perkData));


        RefillManaGA refillManaGA = new();

        ActionSystem.Instance.Perform(refillManaGA, () =>
        {
            DrawCardGA drawCardGA = new(5);
            ActionSystem.Instance.Perform(drawCardGA);
        });
    }
}
