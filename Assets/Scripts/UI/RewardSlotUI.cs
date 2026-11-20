using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class RewardSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private Action onClick;

    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
    }

    public void Setup(string displayName, Sprite icon, Action onClick)
    {
        nameText.text = displayName;
        iconImage.sprite = icon;
        iconImage.enabled = icon != null;

        this.onClick = onClick;
        button.interactable = onClick != null;
    }

    private void OnClicked()
    {
        onClick?.Invoke();
    }
}
