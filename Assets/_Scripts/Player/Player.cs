using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IUnit
{
    public static Player Instance { get; private set; }
    public delegate void GoldEventHandler(int gold);
    public event GoldEventHandler OnGoldPickup;
    public event EventHandler OnLevelUp;
    private Inventory inventory;
    private Equipment equipment;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private EquipmentUI equipmentUI;
    private void Awake()
    {
        Instance = this;
        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);
        equipment = new Equipment(inventory);
        equipmentUI.SetEquipment(equipment);
    }

    void Start()
    {
        ItemDropManager.Instance.OnItemPickup += ItemDropManager_OnItemPickup;
        inventoryUI.OnItemEquip += InventoryUI_OnItemEquip;
        equipment.OnEquip += Equipment_OnEquip;
        equipment.OnUnequip += Equipment_OnUnequip;
    }
    private void Equipment_OnUnequip(EquipmentItemSO equip)
    {
        equip.OnUnequip(GetComponent<PlayerStats>());
        
        
        
    }

    private void Equipment_OnEquip(EquipmentItemSO equip)
    {
        equip.OnEquip(GetComponent<PlayerStats>());
        
    }

    private void InventoryUI_OnItemEquip(EquipmentItemSO equip)
    {
        equipment.EquipItem(equip);
    }

    private void ItemDropManager_OnItemPickup(EquipmentItemSO equip)
    {
        if (inventory.AddItem(equip))
        {
            equip.InitDescription();
            inventoryUI.RefreshInventoryItems();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Pendant")
        {
            ItemDropManager.Instance.PickupItem();
            Destroy(col.gameObject);
        }
        else if(col.gameObject.tag == "Gold")
        {
            int gold = col.gameObject.GetComponent<GoldDrop>().GetAmount();
            inventory.AddGold(gold);
            OnGoldPickup?.Invoke(gold);
            Destroy(col.gameObject);
        }
    }
    public void UpgradeEquipment()
    {
        equipmentUI.IncreaseActiveRows();
    }
    public void DowngradeEquipment()
    {
        equipmentUI.DecreaseActiveRows();
    }

    public int GetGoldAmount()
    {
        return inventory.GetGold();
    }

    public void SetGoldAmount(int goldAmount)
    {
        inventory.SetGold(goldAmount);
    }
    public void LevelUp()
    {
        OnLevelUp?.Invoke(this, EventArgs.Empty);
    }
    public void GetCurrentUpgrades(int upgradeIndex)
    {
        if(upgradeIndex == 0 || upgradeIndex == 1 || upgradeIndex == 2 || upgradeIndex == 3)
        {
            equipmentUI.IncreaseActiveRows();
        }
        else if(upgradeIndex == 4)
        {//double the rate of gold drop
            GoldManager.Instance.IncreaseGoldDropRate(100);
        }
        else if(upgradeIndex == 5)
        {
            ItemDropManager.Instance.PickupItem();
        }
    }
}
