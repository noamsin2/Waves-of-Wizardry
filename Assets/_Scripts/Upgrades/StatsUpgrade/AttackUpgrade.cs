using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpgrade : _StatsUpgrade
{
    private int upgradeLevel = 0;
    const string ICON_NAME = "AttackUp";
    const string DESCRIPTION = "Increases Player Attack By 1";

    public override string GetDescription()
    {
        return DESCRIPTION;
    }
    public override string GetIconName()
    {
        return ICON_NAME;
    }
    public override void Upgrade()
    {
        upgradeLevel++;
        GameObject.Find("PlayerLogic").GetComponent<PlayerStats>().IncreaseAttack(1);
    }

    public override int GetLevel()
    {
        return upgradeLevel;
    }
}
