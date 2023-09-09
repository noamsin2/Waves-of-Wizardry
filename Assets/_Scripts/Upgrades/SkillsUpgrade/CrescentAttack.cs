using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrescentAttack : _SkillsUpgrade
{
    private string description = "Swing water blades around you, (+1 damage per skill level)";
    private string iconName = "WaterCrescent";
    private int upgradeLevel = 0;
    public override string GetDescription()
    {
        if (upgradeLevel == 4)
            description = "Swing BLOOD blades around you, (+1 damage per skill level)";
        return description;
    }

    public override string GetIconName()
    {
        if (upgradeLevel == 4)
            iconName = "BloodCrescent";
        return iconName;


    }

    public override void Upgrade()
    {
        upgradeLevel++;
        GameObject.Find("PlayerLogic").GetComponent<PlayerSkills>().WaterCrescentAttackActivate();
    }

    public override int GetLevel()
    {
        return upgradeLevel;
    }
}
