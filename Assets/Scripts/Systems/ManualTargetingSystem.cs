using UnityEngine;

public class ManualTargetingSystem : Singleton<ManualTargetingSystem>
{
    [SerializeField] private ArrowView arrowView;
    [SerializeField] private LayerMask targetLayerMask;


    public void StartTargeting(Vector3 startPos)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetupArrow(startPos);
    }

    public EnemyView EndTargeting(Vector3 endPos)
    {
        arrowView.gameObject.SetActive(false);
        if (Physics.Raycast(endPos, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }

        return null;


    }
}
