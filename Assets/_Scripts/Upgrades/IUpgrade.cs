using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgrade
{
    public int GetLevel();
    public string GetDescription();
    public Sprite GetSprite();
    public void Upgrade();
}
