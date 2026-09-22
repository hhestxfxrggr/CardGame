using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject arrowHead;
    [SerializeField] private LineRenderer lineRenderer;

    private Vector3 startPos;

    private void Update()
    {
        Vector3 endPos = MouseUtil.GetMousePositionInWorldSpace();
        Vector3 dir = -(startPos - arrowHead.transform.position).normalized;
        lineRenderer.SetPosition(1, endPos - dir * 0.5f);
        arrowHead.transform.position = endPos;
        arrowHead.transform.right = dir;
    }

    public void SetupArrow(Vector3 pos)
    {
        startPos = pos;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());
    }
}
