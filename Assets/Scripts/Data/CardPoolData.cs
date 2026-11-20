using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CardPool")]
public class CardPoolData : ScriptableObject
{
    [field: SerializeField] public List<CardData> Cards { get; private set; } = new();
}
