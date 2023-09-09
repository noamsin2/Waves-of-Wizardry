using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishAttack : _SkillsUpgrade
{
    private string description = "Jellyfish souls float above you";
    private string iconName = "JellyfishSouls";
    private int upgradeLevel = 0;
    public override string GetDescription()
    {
        if (upgradeLevel == 5)
            description = "Jellyfish and octopus are best friends!";
        return description;
    }

    public override string GetIconName()
    {
        return iconName;
    }

    public override void Upgrade()
    {
        upgradeLevel++;
        GameObject.Find("PlayerLogic").GetComponent<PlayerSkills>().JellyfishAttackActivate();
    }

    public override int GetLevel()
    {
        return upgradeLevel;
    }
}
