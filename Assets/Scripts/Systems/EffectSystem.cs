using System.Collections;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }


    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        if (performEffectGA.Targets != null && performEffectGA.Targets.Count > 0)
        {
            performEffectGA.Targets.RemoveAll(target => target == null || target.CurrentHealth <= 0);
            
            if(performEffectGA.Targets.Count == 0)
                yield break;
        }

        GameAction effectAcrion = performEffectGA.Effect.GetGameAction(performEffectGA.Targets, HeroSystem.Instance.HeroView);
        ActionSystem.Instance.AddReaction(effectAcrion);
        yield return null;
    }
}
