using UnityEngine;

public class Interaction : Singleton<Interaction>
{
    [SerializeField] private GameObject cardsPanel;
    public bool PlayerIsDragging {  get; set; } = false;
    public bool PlayerCanInteract()
    {
        return !cardsPanel.activeInHierarchy && !ActionSystem.Instance.IsPerforming;
    }

    public bool PlayerCanHover()
    {
        return !cardsPanel.activeInHierarchy && !PlayerIsDragging;
    }
}
