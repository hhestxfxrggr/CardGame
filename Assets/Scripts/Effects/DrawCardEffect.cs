using System.Collections.Generic;
using UnityEngine;

public class DrawCardEffect : Effect
{
    [SerializeField] private int drawAmount;
   public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DrawCardGA drawCardGA = new DrawCardGA(drawAmount);
        return drawCardGA;
    }
}
