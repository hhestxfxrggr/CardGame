using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private readonly Dictionary<ItemData, int> items = new();
    private readonly List<SoulData> souls = new();
    private int maxAmount = 99;

    public IReadOnlyDictionary<ItemData, int> Items { get; private set; }
    public IReadOnlyList<SoulData> Souls { get; private set; }

    public event Action OnInventoryChanged;

    private void Awake()
    {
        Items = new ReadOnlyDictionary<ItemData, int>(items);
        Souls = souls.AsReadOnly();
    }

    public bool TryAddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        int currentCount = GetItemCount(item);

        if (amount > maxAmount - currentCount)
            return false;

        items[item] = currentCount + amount;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool TryRemoveItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        int currentCount = GetItemCount(item);

        if (currentCount < amount)
            return false;

        int remainingCount = currentCount - amount;

        if (remainingCount == 0)
            items.Remove(item);
        else
            items[item] = remainingCount;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool TryAddSoul(SoulData soul)
    {
        if (soul == null)
            return false;

        souls.Add(soul);

        OnInventoryChanged?.Invoke();

        return true;
    }

    public int GetItemCount(ItemData item)
    {
        if (item == null)
            return 0;

        return items.TryGetValue(item, out int count) ? count : 0;
    }
}
