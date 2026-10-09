using UnityEngine;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public Perk Perk {  get; private set; }

    public void Setup(Perk perkdata)
    {
        Perk = perkdata;
        image.sprite = Perk.Image;
    }
}
