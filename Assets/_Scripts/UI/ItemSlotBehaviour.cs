using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlotBehaviour : MonoBehaviour
{
    private Transform itemDetails;
    public Action ClickFunc = null;
    public Action ReplaceFunc = null;
    public Action RightClickFunc = null;
    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 0.5f;
    private bool isSelected = false;
    private void Awake()
    {
        itemDetails = transform.Find("ItemDetails");
    }
    void Start()
    {
        itemDetails.gameObject.SetActive(false);
    }

    /*public void OnSelect()
    {
        isSelected = !isSelected;
    }*/
    
    public void OnPointerDown()
    {
        if (ClickFunc != null)
        { 
            clicked++;
            if (clicked == 1) clicktime = Time.time;

            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                ClickFunc();
            }
            else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;

            
        }
        if (RightClickFunc != null)
            RightClickFunc();

    }
    public void OnCursorEnter()
    {
        if(itemDetails.gameObject.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text != "")
            itemDetails.gameObject.SetActive(true);
        
    }
    public void OnCursorExit()
    {
        itemDetails.gameObject.SetActive(false);
    }
}
