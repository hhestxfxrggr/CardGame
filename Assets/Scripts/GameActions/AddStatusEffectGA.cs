using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffectGA : GameAction
{
    public StatusEffectType statusEffectType { get; private set; }
    public int StackCount { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public AddStatusEffectGA(StatusEffectType type, int stackCount, List<CombatantView> targets)
    {
        this.statusEffectType = type;
        StackCount = stackCount;
        Targets = targets;
    }
}
