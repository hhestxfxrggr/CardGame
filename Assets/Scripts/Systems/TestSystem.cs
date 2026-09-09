using UnityEngine;
using System.Collections.Generic;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> deckData;
    [SerializeField] private CardSystem cardSystem;
    private void Start()
    {
        cardSystem.Setup(deckData);
    }

}
