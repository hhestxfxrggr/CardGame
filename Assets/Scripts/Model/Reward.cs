using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Reward
{
    public int Gold { get; private set; }

    private readonly List<CardData> cards;
    private readonly Dictionary<ItemData, int> items;
    private readonly List<SoulData> souls;

    public IReadOnlyList<CardData> Cards { get; }
    public IReadOnlyDictionary<ItemData, int> Items { get; }
    public IReadOnlyList<SoulData> Souls { get; }

    public bool IsEmpty => Gold == 0 && cards.Count == 0 && items.Count == 0 && souls.Count == 0;

    public Reward(int gold, List<CardData> cards, List<ItemData> items, List<SoulData> souls)
    {
        Gold = gold;

        this.cards = new List<CardData>(cards);
        this.items = new Dictionary<ItemData, int>();
        this.souls = new List<SoulData>(souls);

        foreach (ItemData item in items)
        {
            if (item == null)
                continue;

            if (this.items.ContainsKey(item))
                this.items[item]++;
            else
                this.items.Add(item, 1);
        }

        Cards = this.cards.AsReadOnly();
        Items = new ReadOnlyDictionary<ItemData, int>(this.items);
        Souls = this.souls.AsReadOnly();
    }

    public void ClearGold()
    {
        Gold = 0;
    }

    public void ClearCards()
    {
        cards.Clear();
    }

    public bool RemoveItem(ItemData item)
    {
        return items.Remove(item);
    }

    public bool RemoveSoul(SoulData soul)
    {
        return souls.Remove(soul);
    }
}
