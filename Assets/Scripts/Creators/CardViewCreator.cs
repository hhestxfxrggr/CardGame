using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class CardViewCreator : MonoBehaviour
{
    [SerializeField] private CardView cardViewPrefab;

    // 반환되어 대기 중인 카드 뷰
    private readonly Queue<CardView> pool = new();
    // 사용 중인 카드 뷰. 중복 반환 방지용
    private readonly HashSet<CardView> activeViews = new();
    // 이 Creator가 생성한 모든 뷰. 종료 시 정리용
    private readonly List<CardView> allViews = new();
    public CardView CreatCardView(Card card, Vector3 pos, Quaternion rot)
    {
        CardView cardView;

        if (pool.Count > 0)
        {
            cardView = pool.Dequeue();
        }
        else
        {
            cardView = Instantiate(cardViewPrefab);
            allViews.Add(cardView);
        }

        activeViews.Add(cardView);

        Transform cardTransform = cardView.transform;

        cardTransform.DOKill();

        // 풀 보관용 부모에서 분리
        cardTransform.SetParent(null, true);
        cardTransform.SetPositionAndRotation(pos, rot);
        cardTransform.localScale = Vector3.zero;

        // 활성화 전에 새 카드 데이터 설정
        cardView.Setup(card);
        cardView.gameObject.SetActive(true);

        cardTransform.DOScale(Vector3.one, 0.15f);

        return cardView;
    }

    public void ReturnCardView(CardView cardView)
    {
        if (cardView == null || !activeViews.Remove(cardView))
            return;

        cardView.transform.DOKill();

        cardView.gameObject.SetActive(false);
        cardView.ResetForPool();

        // 반환된 뷰는 Creator 아래 보관
        cardView.transform.SetParent(transform, false);

        pool.Enqueue(cardView);
    }

    private void OnDestroy()
    {
        // 사용 중인 뷰는 부모에서 분리되어 있으므로 직접 정리
        foreach (CardView cardView in allViews)
        {
            if (cardView == null)
                continue;

            cardView.transform.DOKill();
            Destroy(cardView.gameObject);
        }

        pool.Clear();
        activeViews.Clear();
        allViews.Clear();
    }
}
