using UnityEngine;

public class RemoveCombatCardGA : GameAction
{
    public CardPileType Source { get; private set; }
    public int Amount { get; private set; }

    public RemoveCombatCardGA(CardPileType source, int amount)
    {
        Source = source;
        Amount = amount;
    }
}
