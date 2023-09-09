using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : _SkillsUpgrade
{
    private string description = "Shoot a water ball at enemies!, (+1 damage per skill level)";
    private string iconName = "WaterBall";
    //const int id = 1;
    private int upgradeLevel = 1;
    public override string GetDescription()
    {
        if (upgradeLevel == 4)
            description = "Shoot a water cannon at enemies!, (+2 damage per skill level)";
        return description;
    }

    public override string GetIconName()
    {
        if (upgradeLevel == 4)
            iconName = "WaterCannon";
        return iconName;

            
    }
    
    public override void Upgrade()
    {
        upgradeLevel++;
        GameObject.Find("PlayerLogic").GetComponent<PlayerSkills>().WaterAttackActivate();
    }

    public override int GetLevel()
    {
        return upgradeLevel;
    }
}
