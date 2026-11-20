using System;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private MatchResultSystem matchResultSystem;
    [SerializeField] private MatchSetupSystem matchSetupSystem;
    [SerializeField] private RewardGenerator rewardGenerator;
    [SerializeField] private DeckSystem deckSystem;

    public Reward CurrentReward { get; private set; }

    public event Action<Reward> OnRewardGenerated;
    public event Action OnRewardChanged;

    private void OnEnable()
    {
        matchResultSystem.OnVictory += OnVictory;
    }

    private void OnDisable()
    {
        matchResultSystem.OnVictory -= OnVictory;
    }

    private void OnVictory()
    {
        if (CurrentReward != null)
            return;

        CurrentReward = rewardGenerator.Generate(matchSetupSystem.EnemyDatas);

        OnRewardGenerated?.Invoke(CurrentReward);
    }

    public bool TryClaimCardToDeck(CardData cardData)
    {
        if (CurrentReward == null || cardData == null)
            return false;

        foreach (CardData card in CurrentReward.Cards)
        {
            if (card != cardData)
                continue;

            deckSystem.AddCard(cardData);
            
            CurrentReward.ClearCards();

            OnRewardChanged?.Invoke();

            return true;
        }

        return false;
    }
}

