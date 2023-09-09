using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeList
{
    public static List<IUpgrade> upgrades;

    public static void Initialize()
    {
        upgrades = new List<IUpgrade>();
        upgrades.Add(new AttackUpgrade());
        upgrades.Add(new WaterBall());
        upgrades.Add(new CrescentAttack());
        upgrades.Add(new JellyfishAttack());
        upgrades.Add(new OctopusAttack());
    }
    
}
