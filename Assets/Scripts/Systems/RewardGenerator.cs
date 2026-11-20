
using System.Collections.Generic;
using UnityEngine;

public class RewardGenerator : MonoBehaviour
{
    [Header("Gold")]
    [SerializeField, Min(0)] private int minGold = 10;
    [SerializeField, Min(0)] private int maxGold = 20;

    [Header("Card")]
    [SerializeField] private CardPoolData commonCards;
    [SerializeField] private CardPoolData rareCards;
    [SerializeField] private CardPoolData legendaryCards;

    [SerializeField, Min(0)] private int commonWeight = 70;
    [SerializeField, Min(0)] private int rareWeight = 25;
    [SerializeField, Min(0)] private int legendaryWeight = 5;

    public Reward Generate(IReadOnlyList<EnemyData> enemies)
    {
        int min = Mathf.Max(0, minGold);
        int max = Mathf.Max(min, maxGold);
        int gold = Random.Range(min, max + 1);

        List<CardData> cards = GenerateCards();
        List<ItemData> items = new();
        List<SoulData> souls = new();

        if (enemies != null)
        {
            foreach (EnemyData enemy in enemies)
            {
                if (enemy == null)
                    continue;

                // 아이템 독립 확률 판정
                if (enemy.ItemDrops != null)
                {
                    foreach (ItemDrop drop in enemy.ItemDrops)
                    {
                        if (drop.Item == null)
                            continue;

                        if (RollChance(drop.Chance))
                            items.Add(drop.Item);
                    }
                }

                // 소울 드롭 여부 판정
                if (!RollChance(enemy.SoulDropChance))
                    continue;

                if (enemy.SoulDrops == null)
                    continue;

                // 몬스터당 최대 1개 선택
                SoulData soul = PickSoul(enemy.SoulDrops);

                if (soul != null)
                    souls.Add(soul);
            }
        }

        return new Reward(gold, cards, items, souls);
    }

    private List<CardData> GenerateCards()
    {
        List<CardData> common = GetAvailableCards(commonCards);
        List<CardData> rare = GetAvailableCards(rareCards);
        List<CardData> legendary = GetAvailableCards(legendaryCards);

        List<CardData> selected = new();

        for (int i = 0; i < 3; i++)
        {
            CardData card = PickCard(common, rare, legendary);

            if (card == null)
                break;

            selected.Add(card);

            common.Remove(card);
            rare.Remove(card);
            legendary.Remove(card);
        }

        return selected;
    }

    private CardData PickCard(List<CardData> common, List<CardData> rare, List<CardData> legendary)
    {
        WeightedRandom<List<CardData>> rarityRandom = new();

        if (common.Count > 0)
            rarityRandom.AddElement(commonWeight, common);

        if (rare.Count > 0)
            rarityRandom.AddElement(rareWeight, rare);

        if (legendary.Count > 0)
            rarityRandom.AddElement(legendaryWeight, legendary);

        List<CardData> selectedPool = rarityRandom.Random();

        if (selectedPool == null)
            return null;

        WeightedRandom<CardData> cardRandom = new();

        foreach (CardData card in selectedPool)
        {
            cardRandom.AddElement(card.RewardWeight, card);
        }

        return cardRandom.Random();
    }

    private List<CardData> GetAvailableCards(CardPoolData pool)
    {
        List<CardData> available = new();

        if (pool == null || pool.Cards == null)
            return available;

        foreach (CardData card in pool.Cards)
        {
            if (card == null || card.RewardWeight <= 0)
                continue;

            if (!available.Contains(card))
                available.Add(card);
        }

        return available;
    }

    private SoulData PickSoul(IReadOnlyList<SoulDrop> drops)
    {
        WeightedRandom<SoulData> soulRandom = new();

        foreach (SoulDrop drop in drops)
        {
            if (drop.Soul == null)
                continue;

            soulRandom.AddElement(drop.Weight, drop.Soul);
        }

        return soulRandom.Random();
    }

    private bool RollChance(float chance)
    {
        if (chance <= 0f)
            return false;

        if (chance >= 100f)
            return true;

        return Random.value < chance / 100f;
    }
}

