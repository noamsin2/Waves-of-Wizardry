using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    private Inventory inventory;
    public delegate void EquipListHandler(EquipmentItemSO equip);
    public event EquipListHandler OnEquip;
    public event EquipListHandler OnUnequip;
    private List<EquipmentItemSO> equips;
    public Equipment(Inventory inventory)
    {
        equips = new List<EquipmentItemSO>();
        this.inventory = inventory;

    }
    public bool UnequipItem(EquipmentItemSO equip)
    {
        if (inventory.GetSize() < Inventory.INVENTORY_SIZE)
        {
            equips.Remove(equip);
            inventory.AddItem(equip);
            OnUnequip?.Invoke(equip);
            return true;
        }
        return false;
    }
    public void EquipItem(EquipmentItemSO equip)
    {
        equips.Add(equip);
        
        OnEquip?.Invoke(equip);
    }
    public List<EquipmentItemSO> GetItemList()
    {
        return equips;
    }
    
}
