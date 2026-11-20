using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSystem : MonoBehaviour
{
    [SerializeField] private GameObject burnVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnGAPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyBurnGA>();
    }

    private IEnumerator ApplyBurnGAPerformer(ApplyBurnGA applyBurnGA)
    {
        CombatantView target = applyBurnGA.Target;

        if (target == null || target.CurrentHealth <= 0)
            yield break;

        Instantiate(burnVFX, target.transform.position, Quaternion.identity);
        
        DealDamageGA dealDamageGA = new(applyBurnGA.BurnDamage, new List<CombatantView> { target }, null);
        ActionSystem.Instance.AddReaction(dealDamageGA);

        target.RemoveStatusEffect(StatusEffectType.BURN, 1);

        yield return null;
    }
}
