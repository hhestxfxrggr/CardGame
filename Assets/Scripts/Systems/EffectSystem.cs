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
        GameAction effectAcrion = performEffectGA.Effect.GetGameAction();
        ActionSystem.Instance.AddReaction(effectAcrion);
        yield return null;
    }
}
