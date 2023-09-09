using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    const float BASE_DROP_RATE = 0.01f;
    public EquipmentItemsListSO normalEquipmentList;
    public EquipmentItemsListSO rareEquipmentList;
    public EquipmentItemsListSO epicEquipmentList;
    public EquipmentItemsListSO legendaryEquipmentList;
    [SerializeField] private Transform pendantPrefab;
    public enum RarityDropRates
    {
        normal = 40,
        rare = 30,
        epic = 20,
        legendary = 10,
    }

    public static ItemDropManager Instance { get; private set; }
    public delegate void ItemDropHandler(EquipmentItemSO equip);
    public event ItemDropHandler OnItemPickup;
    [field: SerializeField] private float itemDropRate;

    private void Awake()
    {
        Instance = this;
        itemDropRate = BASE_DROP_RATE;
    }
    public void PickupItem()
    {
        EquipmentItemSO newEquip;
        int rarityDrop = Random.Range(1, 101);
        if (rarityDrop <= (int)RarityDropRates.normal)
            newEquip = normalEquipmentList.equipmentItems[Random.Range(0, normalEquipmentList.equipmentItems.Count)];
        else if (rarityDrop > 100 - ((int)RarityDropRates.epic + (int)RarityDropRates.legendary) && rarityDrop  <= 100 - (int)RarityDropRates.legendary)
            newEquip = epicEquipmentList.equipmentItems[Random.Range(0, epicEquipmentList.equipmentItems.Count)];//drop epic
        else if (rarityDrop > (int)RarityDropRates.normal && rarityDrop <= (int)RarityDropRates.normal + (int)RarityDropRates.rare)
            newEquip = rareEquipmentList.equipmentItems[Random.Range(0, rareEquipmentList.equipmentItems.Count)];//drop rare
        else
        {
            Debug.Log(rarityDrop);
            newEquip = legendaryEquipmentList.equipmentItems[Random.Range(0, legendaryEquipmentList.equipmentItems.Count)];//drop legendary
        }

        OnItemPickup?.Invoke(newEquip);
    }
    public void DropItem(Vector2 dropPosition)
    {
        if (Random.Range(1, 1001) < itemDropRate * 1000)    
            Instantiate(pendantPrefab, dropPosition, Quaternion.identity);
    }
    public void IncreaseDropRate(float rate)
    {
        itemDropRate += BASE_DROP_RATE * (rate / 100);
        //Debug.Log(itemDropRate);
    }
    public EquipmentItemSO GetEquipmentSO(RarityDropRates rarity, int id)
    {
        switch (rarity)
        {
            case RarityDropRates.rare:
                return rareEquipmentList.equipmentItems[id];
            case RarityDropRates.epic:
                return epicEquipmentList.equipmentItems[id];
            case RarityDropRates.legendary:
                return legendaryEquipmentList.equipmentItems[id];
            default:
                return normalEquipmentList.equipmentItems[id];
        }
    }
}
