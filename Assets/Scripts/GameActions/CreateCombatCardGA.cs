using UnityEngine;

public class CreateCombatCardGA : GameAction
{
    public CardData CardData { get; private set; }
    public CardPileType Destination { get; private set; }
    public int Amount { get; private set; }

    public CreateCombatCardGA(CardData cardData, CardPileType destination, int amount)
    {
        CardData = cardData;
        Destination = destination;
        Amount = amount;
    }
}
