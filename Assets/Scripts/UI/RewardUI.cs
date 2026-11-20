using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [Header("System")]
    [SerializeField] private RewardSystem rewardSystem;

    [Header("Panels")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject rewardListPanel;
    [SerializeField] private GameObject cardSelectionPanel;

    [Header("Reward List")]
    [SerializeField] private Transform rewardContent;
    [SerializeField] private RewardSlotUI rewardSlotPrefab;
    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite cardRewardIcon;

    [Header("Card Selection")]
    [SerializeField] private CardUI[] cardUIs = new CardUI[3];

    private readonly List<RewardSlotUI> activeSlots = new();
    private readonly Queue<RewardSlotUI> slotPool = new();

    private void Awake()
    {
        rewardPanel.SetActive(false);

        for (int i = 0; i < cardUIs.Length; i++)
        {
            int index = i;

            cardUIs[i].Button.onClick.AddListener(() => OnCardSelected(index));
        }
    }

    private void OnEnable()
    {
        rewardSystem.OnRewardGenerated += ShowReward;
        rewardSystem.OnRewardChanged += OnRewardChanged;

        if (rewardSystem.CurrentReward != null)
            ShowReward(rewardSystem.CurrentReward);
    }

    private void OnDisable()
    {
        rewardSystem.OnRewardGenerated -= ShowReward;
        rewardSystem.OnRewardChanged -= OnRewardChanged;
    }

    private void ShowReward(Reward reward)
    {
        rewardPanel.SetActive(true);
        rewardListPanel.SetActive(true);
        cardSelectionPanel.SetActive(false);

        UpdateRewardList(reward);
    }

    private void UpdateRewardList(Reward reward)
    {
        ClearRewardSlots();

        if (reward.Gold > 0)
            CreateRewardSlot($"{reward.Gold} 골드", goldIcon, null);

        // 아이템
        foreach (ItemData item in reward.Items)
        {
            if (item == null)
                continue;

            CreateRewardSlot(item.Name, item.Image, null);
        }

        // 소울
        foreach (SoulData soul in reward.Souls)
        {
            if (soul == null)
                continue;

            CreateRewardSlot(soul.Name, soul.Image, null);
        }

        // 카드 보상
        if (reward.Cards.Count > 0)
        {
            CreateRewardSlot("덱에 카드를 추가", cardRewardIcon, OpenCardSelection);
        }
    }

    private void CreateRewardSlot(string displayName, Sprite icon, System.Action onClick)
    {
        RewardSlotUI slot;

        if (slotPool.Count > 0)
        {
            slot = slotPool.Dequeue();
        }
        else
        {
            slot = Instantiate(rewardSlotPrefab, rewardContent);
        }

        slot.transform.SetAsLastSibling();
        slot.Setup(displayName, icon, onClick);
        slot.gameObject.SetActive(true);

        activeSlots.Add(slot);
    }

    private void ClearRewardSlots()
    {
        foreach (RewardSlotUI slot in activeSlots)
        {
            slot.gameObject.SetActive(false);
            slotPool.Enqueue(slot);
        }

        activeSlots.Clear();
    }

    private void OpenCardSelection()
    {
        Reward reward = rewardSystem.CurrentReward;

        if (reward == null || reward.Cards.Count == 0)
            return;

        for (int i = 0; i < cardUIs.Length; i++)
        {
            bool hasCard = i < reward.Cards.Count && i < cardUIs.Length;

            cardUIs[i].Button.gameObject.SetActive(hasCard);

            if (!hasCard)
                continue;

            cardUIs[i].Setup(reward.Cards[i]);
        }

        rewardListPanel.SetActive(false);
        cardSelectionPanel.SetActive(true);
    }

    private void OnCardSelected(int index)
    {
        Reward reward = rewardSystem.CurrentReward;

        if (reward == null || index < 0 || index >= reward.Cards.Count)
            return;

        CardData card = reward.Cards[index];

        rewardSystem.TryClaimCardToDeck(card);
    }

    private void OnRewardChanged()
    {
        Reward reward = rewardSystem.CurrentReward;

        if (reward == null)
            return;

        UpdateRewardList(reward);

        if (reward.Cards.Count == 0 && cardSelectionPanel.activeSelf)
        {
            BackToRewardList();
        }
    }

    public void BackToRewardList()
    {
        cardSelectionPanel.SetActive(false);
        rewardListPanel.SetActive(true);
    }
}

