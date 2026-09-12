using UnityEngine;

public class DrawCardEffect : Effect
{
    [SerializeField] private int drawAmount;
   public override GameAction GetGameAction()
    {
        DrawCardGA drawCardGA = new DrawCardGA(drawAmount);
        return drawCardGA;
    }
}
