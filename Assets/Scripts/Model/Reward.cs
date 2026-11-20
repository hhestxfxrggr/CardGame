using System.Collections.Generic;

public class Reward
{
    public int Gold { get; private set; }

    private readonly List<CardData> cards;
    private readonly List<ItemData> items;
    private readonly List<SoulData> souls;

    public IReadOnlyList<CardData> Cards { get; }
    public IReadOnlyList<ItemData> Items { get; }
    public IReadOnlyList<SoulData> Souls { get; }

    public bool IsEmpty => Gold == 0 && cards.Count == 0 && items.Count == 0 && souls.Count == 0;

    public Reward(int gold, List<CardData> cards, List<ItemData> items, List<SoulData> souls)
    {
        Gold = gold;

        this.cards = new List<CardData>(cards);
        this.items = new List<ItemData>(items);
        this.souls = new List<SoulData>(souls);

        Cards = this.cards.AsReadOnly();
        Items = this.items.AsReadOnly();
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
