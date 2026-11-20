using UnityEngine;

[CreateAssetMenu(menuName = "Data/Soul")]
public class SoulData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
}
