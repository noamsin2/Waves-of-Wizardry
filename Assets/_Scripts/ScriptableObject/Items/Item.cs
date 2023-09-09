using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item: ScriptableObject
{
    public new string name;
    public string description;
    public int id;
    public int amount;
    public bool IsStackable;
    public Sprite itemInventorySprite;
    public abstract void InitDescription();
    public abstract string GetRarity();
}
