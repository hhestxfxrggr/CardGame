using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = null;

    public bool IsPerforming { get; private set; } = false;

    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();
    private static Dictionary<(Type type, ReactionTiming timing, Delegate reaction), Action<GameAction>> wrappedDelegates = new();

    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        if (IsPerforming) return;

        IsPerforming = true;

        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }


    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }


    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        // PRE
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        // PERFORM
        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        // POST
        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();

        OnFlowFinished?.Invoke();
    }


    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();

        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }


    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();

        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action);
            }
        }
    }


    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }

    //action 자체가 실제로 하는 행동
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);

        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);

        if (performers.ContainsKey(type))
            performers[type] = wrappedPerformer;
        else
            performers.Add(type, wrappedPerformer);
    }


    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);

        if (performers.ContainsKey(type))
            performers.Remove(type);
    }

    //action이 발생했을 때 추가로 반응하는 효과를 저장
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        var key = (typeof(T), timing, (Delegate)reaction);

        if (wrappedDelegates.ContainsKey(key))
            return;

        Action<GameAction> wrappedReaction = action => reaction((T)action);

        if (!subs.TryGetValue(typeof(T), out var list))
        {
            list = new List<Action<GameAction>>();
            subs.Add(typeof(T), list);
        }

        list.Add(wrappedReaction);
        wrappedDelegates.Add(key, wrappedReaction);
    }


    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        var key = (typeof(T), timing, (Delegate)reaction);

        if (!wrappedDelegates.TryGetValue(key, out var wrappedReaction))
            return;

        if (subs.TryGetValue(typeof(T), out var list))
        {
            list.Remove(wrappedReaction);
        }

        wrappedDelegates.Remove(key);
    }
}
