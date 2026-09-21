using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        CombatantView target = EnemySystem.Instance.enemies[Random.Range(0, EnemySystem.Instance.enemies.Count)];
        return new() { target};
    }

}
