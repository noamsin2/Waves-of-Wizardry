using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    public const string CURRENT_UPGRADES = "CurrentUpgrades";
    public const string TOTAL_UPGRADES = "TotalUpgrades";
    public static GameSaver Instance { get; private set; }
    private IUnit unit;
    [SerializeField] private GameObject unitGameObject;

    private void Awake()
    {
       
        Instance = this;
        if (unitGameObject != null)
            unit = unitGameObject.GetComponent<IUnit>();
    }
    public void SaveGame()
    {
        int gold = unit.GetGoldAmount();
        PlayerPrefs.SetInt("gold", gold);
        if (PlayerPrefs.GetInt("wave", 0) <= WaveManager.Instance.wave)
            PlayerPrefs.SetInt("wave", WaveManager.Instance.wave);
        PlayerPrefs.Save();
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("gold"))
        {
            int gold = PlayerPrefs.GetInt("gold", 0);
            unit.SetGoldAmount(gold);
        }
        for (int i = 0; i < ShopMenuUI.UPGRADES_IN_GAME; i++)
        {
            if (PlayerPrefs.HasKey(CURRENT_UPGRADES + i))
            {
                //if the upgrade equal to 1 (bool) then it exists and it gets sent to the player
                if(PlayerPrefs.GetInt(CURRENT_UPGRADES + i.ToString(), 0) == 1)
                    unit.GetCurrentUpgrades(i);
            }
        }
    }
    public int LoadGold()
    {
        int gold = PlayerPrefs.GetInt("gold", 0);
        return gold;
    }
}
