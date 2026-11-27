using System;
using System.Linq;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private MatchResultSystem matchResultSystem;
    [SerializeField] private MatchSetupSystem matchSetupSystem;
    [SerializeField] private RewardGenerator rewardGenerator;
    [SerializeField] private DeckSystem deckSystem;
    [SerializeField] private GoldSystem goldSystem;
    [SerializeField] private InventorySystem inventorySystem;

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

    public bool TryClaimGold()
    {
        if (CurrentReward == null || CurrentReward.Gold <= 0)
            return false;

        int amount = CurrentReward.Gold;

        if (!goldSystem.TryAddGold(amount))
            return false;

        CurrentReward.ClearGold();

        OnRewardChanged?.Invoke();

        Debug.Log($"È¹µæÇÑ °ñµå: {amount}");
        Debug.Log($"ÇöÀç º¸À¯ °ñµå: {goldSystem.CurrentGold}");

        return true;
    }

    public bool TryClaimItem(ItemData item)
    {
        if (CurrentReward == null || item == null)
            return false;

        if (!CurrentReward.Items.TryGetValue(item, out int count))
            return false;

        if (!inventorySystem.TryAddItem(item, count))
            return false;

        CurrentReward.RemoveItem(item);

        OnRewardChanged?.Invoke();

        Debug.Log($"È¹µæÇÑ ¾ÆÀÌÅÛ: {item.Name}, {count}");

        return true;
    }

    public bool TryClaimSoul(SoulData soul)
    {
        if (CurrentReward == null || soul == null)
            return false;

        if (!CurrentReward.Souls.Contains(soul))
            return false;

        if (!inventorySystem.TryAddSoul(soul))
            return false;

        CurrentReward.RemoveSoul(soul);

        OnRewardChanged?.Invoke();

        Debug.Log($"È¹µæÇÑ ¾ÆÀÌÅÛ: {soul.Name}");

        return true;
    }
}

