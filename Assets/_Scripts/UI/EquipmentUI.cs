using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private Transform[] ItemSlotsRows;
    private Equipment equipment;
    private int nofActiveRows;
    [SerializeField] private Sprite defaultPendant;
    void Start()
    {
        nofActiveRows = 1;
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
        gameObject.SetActive(true);
    }
    public void SetEquipment(Equipment equipment)
    {
        this.equipment = equipment;
        equipment.OnEquip += equipment_OnEquip;
    }
    private void SetSlotOnEquip(EquipmentItemSO equip, Transform itemSlotTemplate)
    {
        //hide image holder
        itemSlotTemplate.transform.Find("ItemImage").gameObject.SetActive(false);
        Transform image = itemSlotTemplate.transform.Find("ItemImageActive");
        //show new image
        image.gameObject.SetActive(true);
        image.GetComponent<Image>().sprite = equip.itemInventorySprite;
        Transform itemDetails = itemSlotTemplate.transform.Find("ItemDetails");
        //change name and description
        itemDetails.Find("ItemName").GetComponent<TextMeshProUGUI>().text = equip.name;
        itemDetails.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = equip.description;
        itemDetails.Find("ItemRarity").GetComponent<TextMeshProUGUI>().text = equip.GetRarity();
        //play equip sound
        AudioManager.Instance.PlayEquip();
        //set the slot's functions
        SetActionFuncs(itemSlotTemplate, equip);

    }
    private void equipment_OnEquip(EquipmentItemSO equip)
    {
        
        for (int i = 0; i < nofActiveRows; i++)
        {
            foreach (Transform child in ItemSlotsRows[i])
            {
                Transform itemSlotTemplate = child.transform.Find("ItemSlotTemplate");
                //Transform itemDetails = itemSlotTemplate.transform.Find("ItemDetails");
                if (itemSlotTemplate.transform.Find("ItemDetails/ItemName").GetComponent<TextMeshProUGUI>().text == "")
                {
                    SetSlotOnEquip(equip, itemSlotTemplate);
                    return;
                }
            }
        }
        //if equip is full, then switch the first one for the equip
        Transform slotTemplate = ItemSlotsRows[0].transform.Find("ItemSlot1/ItemSlotTemplate");
        
        ItemSlotBehaviour itemSlot = slotTemplate.GetComponent<ItemSlotBehaviour>();
        itemSlot.ReplaceFunc();
        SetSlotOnEquip(equip, slotTemplate);
        
    }

    private void SetActionFuncs(Transform itemSlotTemplate, EquipmentItemSO equip)
    {
        ItemSlotBehaviour itemSlot = itemSlotTemplate.GetComponent<ItemSlotBehaviour>();
        itemSlot.ClickFunc = () =>
        {
            //unequips only if there's room in inventory
            if (equipment.UnequipItem(equip))
            {
                //reset actions
                itemSlot.ClickFunc = null;
                //remove item from equip and place it in inventory
                itemSlotTemplate.transform.Find("ItemImageActive").gameObject.SetActive(false);
                itemSlotTemplate.transform.Find("ItemImage").gameObject.SetActive(true);
                Transform itemDetails = itemSlotTemplate.transform.Find("ItemDetails");
                itemDetails.Find("ItemName").GetComponent<TextMeshProUGUI>().text = "";
                itemDetails.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = "";
                itemDetails.Find("ItemRarity").GetComponent<TextMeshProUGUI>().text = "";
                //play sound
                AudioManager.Instance.PlayUnequip();
            }
            else
            {
                Debug.Log("inventory full cant unequip");
            }

        };
        itemSlotTemplate.GetComponent<ItemSlotBehaviour>().ReplaceFunc = () =>
        {
            //itemSlot.ReplaceFunc = null;
            equipment.UnequipItem(equip);
        };
    }
    public void IncreaseActiveRows()
    {
        if (nofActiveRows < 5)
        {
            nofActiveRows++;
            ItemSlotsRows[nofActiveRows-1].gameObject.SetActive(true);
        }
    }
    public void DecreaseActiveRows()
    {
        if (nofActiveRows > 1)
        {
            nofActiveRows--;
            ItemSlotsRows[nofActiveRows].gameObject.SetActive(false);
        }
    }
}
