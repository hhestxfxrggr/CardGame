using System;
using UnityEngine;

public class GoldSystem : MonoBehaviour
{
    [field: SerializeField, Min(0)] public int CurrentGold { get; private set; }

    public event Action<int> OnGoldChanged;

    public bool TryAddGold(int amount)
    {
        if (amount <= 0)
            return false;

        CurrentGold += amount;

        OnGoldChanged?.Invoke(CurrentGold);

        return true;
    }

    public bool TrySpendGold(int amount)
    {
        if (amount < 0 || CurrentGold < amount)
            return false;

        CurrentGold -= amount;

        OnGoldChanged?.Invoke(CurrentGold);

        return true;
    }
}
