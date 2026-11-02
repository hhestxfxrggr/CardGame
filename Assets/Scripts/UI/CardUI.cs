using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text playMana;
    [SerializeField] private Image image;

    // 원본 덱의 카드 표시
    public void Setup(CardData cardData)
    {
        title.text = cardData.name;
        description.text = cardData.Description;
        playMana.text = cardData.PlayMana.ToString();
        image.sprite = cardData.Image;
    }

    // 전투 중인 카드 표시
    public void Setup(Card card)
    {
        title.text = card.Title;
        description.text = card.Description;
        playMana.text = card.PlayMana.ToString();
        image.sprite = card.Image;
    }
}
