using System.Collections.Generic;
using UnityEngine;

public class CreateCombatCardEffect : Effect
{
    [SerializeField] private CardData cardData;
    [SerializeField] private CardPileType destination;
    [SerializeField, Min(1)] private int amount = 1;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new CreateCombatCardGA(cardData, destination, amount);
    }
}
