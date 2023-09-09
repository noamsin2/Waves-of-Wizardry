using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _SkillsUpgrade : IUpgrade
{
    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Skills/" + GetIconName());
    }
    public abstract string GetDescription();
    public abstract string GetIconName();
    public abstract void Upgrade();

    public abstract int GetLevel();
}
