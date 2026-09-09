using UnityEngine;
using System.Collections.Generic;

public abstract class GameAction
{
    //행동 전
    public List<GameAction> PreReactions { get; private set; } = new();
    //실제 행동
    public List<GameAction> PerformReactions { get; private set; } = new();
    //행동 후
    public List<GameAction> PostReactions { get; private set; } = new();

}
