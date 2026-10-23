using System.Collections.Generic;
using UnityEngine;

public class RemoveCombatCardEffect : Effect
{
    [SerializeField] private CardPileType source;
    [SerializeField, Min(1)] private int amount = 1;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new RemoveCombatCardGA(source, amount);
    }
}
