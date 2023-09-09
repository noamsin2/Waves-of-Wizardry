using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusAttack : _SkillsUpgrade
{
    private string description = "Octopus souls float below you";
    private string iconName = "OctopusSouls";
    private int upgradeLevel = 0;
    public override string GetDescription()
    {
        if (upgradeLevel == 5)
            description = "Octopus and jellyfish are best friends!";
        return description;
    }

    public override string GetIconName()
    {
        return iconName;
    }

    public override void Upgrade()
    {
        upgradeLevel++;
        GameObject.Find("PlayerLogic").GetComponent<PlayerSkills>().OctopusAttackActivate();
    }

    public override int GetLevel()
    {
        return upgradeLevel;
    }
}
