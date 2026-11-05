using UnityEngine;
using System;

public class MatchResultSystem : MonoBehaviour
{
    [SerializeField] private EnemySystem enemySystem;

    public bool IsVictory { get; private set; }
    public event Action OnVictory;

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<KillEnemyGA>(OnEnemyKilled, ReactionTiming.Post);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<KillEnemyGA>(OnEnemyKilled, ReactionTiming.Post);
    }

    private void OnEnemyKilled(KillEnemyGA action)
    {
        if (IsVictory || enemySystem.enemies.Count > 0)
            return;

        IsVictory = true;
        OnVictory?.Invoke();
    }
}
