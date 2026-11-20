using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemDrop
{
    public ItemData Item;

    [Range(0f, 100f)]
    public float Chance;
}

[System.Serializable]
public struct SoulDrop
{
    public SoulData Soul;

    [Min(1)]
    public int Weight;
}

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int AttackPower { get; private set; }

    [field: SerializeField] public List<ItemDrop> ItemDrops { get; private set; } = new();

    [field: SerializeField, Range(0f, 100f)] public float SoulDropChance { get; private set; }

    [field: SerializeField] public List<SoulDrop> SoulDrops { get; private set; } = new();
}


