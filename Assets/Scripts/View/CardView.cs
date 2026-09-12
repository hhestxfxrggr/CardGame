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
        Interaction.Instance.PlayerIsDragging = true;
        wrapper.SetActive(true);
        CardViewHoverSystem.Instance.Hide();

        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;

        transform.rotation = Quaternion.Euler(0,0,0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
    }

    private void OnMouseDrag()
    {
        if (!Interaction.Instance.PlayerCanInteract())
            return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);

    }

    private void OnMouseUp()
    {
        if (!Interaction.Instance.PlayerCanInteract())
            return;
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

        Interaction.Instance.PlayerIsDragging= false;
    }
}
