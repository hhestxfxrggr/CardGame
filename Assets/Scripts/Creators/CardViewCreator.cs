using DG.Tweening;
using UnityEngine;

public class CardViewCreator : MonoBehaviour
{
    [SerializeField] private CardView cardViewPrefab;

    public CardView CreatCardView(Card card, Vector3 pos, Quaternion rot)
    {
        CardView cardView = Instantiate(cardViewPrefab, pos, rot);

        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, 0.15f);

        cardView.Setup(card);

        return cardView;
    }
}
