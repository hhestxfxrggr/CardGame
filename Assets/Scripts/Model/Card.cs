
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    public string Title => data.name;
    public string Description => data.Description;
    public Sprite Image =>data.Image;
    public List<Effect> Effects => data.Effects;
    public int PlayMana { get; private set; }

    private readonly CardData data;



    public Card(CardData cardData)
    {
        data = cardData;
        PlayMana = data.PlayMana;
    }
}
