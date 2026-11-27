using System;
using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : Singleton<PerkSystem>
{
    [SerializeField] private PerksUI perksUI;
    [SerializeField] private InventorySystem inventorySystem;

    [Header("Soul")]
    [SerializeField, Min(1)] private int maxSoulSlots = 3;

    private readonly List<Perk> perks = new();
    private readonly Dictionary<SoulData, Perk> soulPerks = new();

    public int MaxSoulSlots => maxSoulSlots;
    public int EquippedSoulCount => soulPerks.Count;

    public event Action OnSoulEquipChanged;



    public void AddPerk(Perk perk)
    {
        perks.Add(perk);
        perksUI.AddPerkUI(perk);
        perk.OnAdd();
    }

    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        perksUI.RemovePerkUI(perk);
        perk.OnRemove();
    }

    public bool IsSoulEquipped(SoulData soul)
    {
        return soul != null && soulPerks.ContainsKey(soul);
    }

    public bool TryEquipSoul(SoulData soul)
    {
        if (soul == null || inventorySystem == null)
            return false;

        if (IsSoulEquipped(soul))
            return false;

        if (soulPerks.Count >= maxSoulSlots)
            return false;

        PerkData perkData = soul.PerkData;

        if (perkData == null || perkData.PerkCondition == null || perkData.AutoTargetEffect?.Effect == null)
            return false;

        if (perkData.UseAutoTarget && perkData.AutoTargetEffect.TargetMode == null)
            return false;

        Perk perk = new(perkData);

        AddPerk(perk);
        soulPerks.Add(soul, perk);

        OnSoulEquipChanged?.Invoke();

        return true;
    }

    public bool TryUnequipSoul(SoulData soul)
    {
        if (soul == null)
            return false;

        if (!soulPerks.TryGetValue(soul, out Perk perk))
            return false;

        RemovePerk(perk);
        soulPerks.Remove(soul);

        OnSoulEquipChanged?.Invoke();

        return true;
    }
}
