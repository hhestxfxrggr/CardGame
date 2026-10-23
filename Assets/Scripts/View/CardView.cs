using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text playMana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    public Card Card { get; private set; }

    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        playMana.text = card.PlayMana.ToString();
        imageSR.sprite = card.Image;

        // 이전 호버·드래그 상태 초기화
        wrapper.SetActive(true);
        dragStartPosition = Vector3.zero;
        dragStartRotation = Quaternion.identity;
    }

    public void ResetForPool()
    {
        // 이전 카드 데이터 참조 해제
        Card = null;

        title.text = string.Empty;
        description.text = string.Empty;
        playMana.text = string.Empty;
        imageSR.sprite = null;

        wrapper.SetActive(true);
        dragStartPosition = Vector3.zero;
        dragStartRotation = Quaternion.identity;
    }

    private void OnMouseEnter()
    {
        if (!Interaction.Instance.PlayerCanHover())
            return;
        wrapper.SetActive(false);
        Vector3 pos = new Vector3(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);

    }

    private void OnMouseExit()
    {
        if (!Interaction.Instance.PlayerCanHover())
            return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interaction.Instance.PlayerCanInteract())
            return;

        if (Card.RequiresManualTarget)
        {
            ManualTargetingSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interaction.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();

            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;

            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);

        }

    }

    private void OnMouseDrag()
    {
        if (!Interaction.Instance.PlayerCanInteract())
            return;

        if (Card.RequiresManualTarget)
            return;

        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);

    }

    private void OnMouseUp()
    {
        if (!Interaction.Instance.PlayerCanInteract())
            return;

        if (Card.RequiresManualTarget)
        {
            EnemyView target = ManualTargetingSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if (target != null && ManaSystem.Instance.HasEnuoghMana(Card.PlayMana))
            {
                PlayCardGA playCardGA = new(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnuoghMana(Card.PlayMana)
            && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGA playCardGA = new(Card);
                ActionSystem.Instance.Perform(playCardGA);
            }
            else
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }

            Interaction.Instance.PlayerIsDragging = false;
        }


    }
}
