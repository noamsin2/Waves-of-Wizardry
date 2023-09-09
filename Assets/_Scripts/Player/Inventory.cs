using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    public const int INVENTORY_SIZE = 24;
    private int size;
    private List<Item> items;
    private int gold;
    public Inventory()
    {
        gold = 0;
        size = 0;
        items = new List<Item>();
        
        
    }
    public bool RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            size--;
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;
    }
    public void AddGold(int gold)
    {
        this.gold += gold;
    }
    public bool AddItem(Item item)
    {
        if (item.IsStackable)
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in items)
            {
                if (inventoryItem.id == item.id)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                if (size >= INVENTORY_SIZE)
                {
                    return false;
                }
                items.Add(item);
                size++;
            }
        }
        else
        {
            if(size >= INVENTORY_SIZE)
            {
                return false;
            }
            items.Add(item);
            size++;
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }
        
    public void SetGold(int amount)
    {
        gold = amount;
    }

    public List<Item> GetItemList()
    {
        return items;
    }
    public int GetGold()
    {
        return gold;
    }
    public int GetSize()
    {
        return size;
    }
}
