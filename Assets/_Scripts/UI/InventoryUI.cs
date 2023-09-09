using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI itemAmount;
    public event Equipment.EquipListHandler OnItemEquip;
    private void Awake()
    {
        itemSlotContainer = transform.Find("InventoryWindow/ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
    }
    private void Start()
    {
        
        GameManager.Instance.OnOpenInventory += GameManager_OnOpenInventory;
        GameManager.Instance.OnCloseInventory += GameManager_OnCloseInventory;
        


        Hide();
    }

   

    private void GameManager_OnCloseInventory(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnOpenInventory(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        goldText.text = inventory.GetGold().ToString();
        gameObject.SetActive(true);
        
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }
    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }
    public void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSizeX = 140f;
        float itemSlotCellSizeY = 160f;
        foreach(Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            ItemSlotBehaviour itemSlot = itemSlotRectTransform.GetComponent<ItemSlotBehaviour>();
            itemSlot.ClickFunc = () =>
            {
                //equip item
                if (!item.IsStackable)
                {
                    inventory.RemoveItem(item);
                    OnItemEquip?.Invoke((EquipmentItemSO)item);
                }
            };
            itemSlot.RightClickFunc = () =>
            {
                if (Input.GetMouseButtonDown(1))
                {
                    inventory.RemoveItem(item);
                    Debug.Log("REMOVE");
                }
            };
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSizeX, -y * itemSlotCellSizeY);
            Image image = itemSlotRectTransform.Find("ItemImage").GetComponent<Image>();
            image.sprite = item.itemInventorySprite;
            
            if (item.amount > 1)
            {
                itemAmount.text = item.amount.ToString();
            }
            else
                itemAmount.text = "";

            Transform itemDetails = itemSlotRectTransform.Find("ItemDetails");

            itemDetails.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.name;

            itemDetails.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.description;

            itemDetails.Find("ItemRarity").GetComponent<TextMeshProUGUI>().text = item.GetRarity();
            x++;
            if(x > 5)
            {
                x = 0;
                y++;
            }
        }
    }
}
