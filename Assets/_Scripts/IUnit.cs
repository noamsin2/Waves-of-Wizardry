using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    int GetGoldAmount();
    void SetGoldAmount(int goldAmount);
    void GetCurrentUpgrades(int upgradeIndex);
}
