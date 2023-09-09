using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ShopMenuUI : MonoBehaviour
{
    public const int UPGRADES_IN_GAME = 6;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private List<Button> upgradeButtons;
    [SerializeField] private List<Button> currentUpgradeButtons;
    [SerializeField] private AudioClip buySound;
    private int gold;
    private Dictionary<int, int> upgradesCost;
    private int currentUpgradesCount;
    private int[] upgradesArr;

    private void Awake()
    {
        upgradesArr = new int[6] { 0, 0, 0, 0, 0, 0 };
        //PlayerPrefs.DeleteAll();
    }
    private void OnDestroy()
    {
        Save();
        Debug.Log("saved");
    }
    void Start()
    {
        
        upgradesCost = new Dictionary<int, int>() { { 0, 2000 }, { 1, 4000 },{ 2, 6000}, { 3, 8000}, { 4, 10000} , { 5, 500} };
        
        gameObject.SetActive(false);
    }

    private void Save()
    {
        for(int i = 0; i < upgradesArr.Length; i++)
        {
            PlayerPrefs.SetInt(GameSaver.CURRENT_UPGRADES + i.ToString(), upgradesArr[i]);
        }
        PlayerPrefs.Save();
    }
    public void Load()
    {
        gameObject.SetActive(true);
        gold = GameSaver.Instance.LoadGold();
       // gold += 2000;
        goldText.text = gold.ToString();
        
        LoadUpgrades();
    }
    public void ShopUpgrade(int upgradeIndex)
    {
        //if we bought it already
        if(PlayerPrefs.GetInt(GameSaver.TOTAL_UPGRADES + upgradeIndex.ToString(),0) == 1)
        {
            //if its not added to current upgrades
            if (currentUpgradesCount < 4 && upgradesArr[upgradeIndex] == 0)
            {
                //add it to current upgrades
                //upgradesArr[upgradeIndex] = 1;
                LoadCurrentUpgrades(upgradeIndex);
                
                Debug.Log("add upgrade to current upgrades: " + upgradeIndex);
                Debug.Log("current upgrades count: " + currentUpgradesCount);

            }
            
            return;
        }
        int cost = upgradesCost[upgradeIndex];
        if(gold < cost)
        {
            //show message
            Debug.Log("NO GOLD");
        }
        else
        {
            gold -= cost;
            upgradeButtons[upgradeIndex].transform.Find("Cost").gameObject.SetActive(false);
            goldText.text = gold.ToString();
            PlayerPrefs.SetInt("gold", gold);
            PlayerPrefs.SetInt(GameSaver.TOTAL_UPGRADES + upgradeIndex.ToString(), 1);
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = buySound;
            audio.Play();
            //bought upgrade
        }
    }
    private void LoadUpgrades()
    {
        currentUpgradesCount = 0;
        for (int i = 0; i < UPGRADES_IN_GAME; i++)
        {
            //if it exists in upgrades that we bought then disable it
            if (PlayerPrefs.GetInt(GameSaver.TOTAL_UPGRADES + i, 0) == 1)
            {
                upgradeButtons[i].transform.Find("Cost").gameObject.SetActive(false);
                if (PlayerPrefs.GetInt(GameSaver.CURRENT_UPGRADES + i, 0) == 1)
                {
                    LoadCurrentUpgrades(i);
                }
            }
        }
        Debug.Log("current upgrades on load: " + currentUpgradesCount);
    }
    private void LoadCurrentUpgrades(int index)
    {
        //set the image for the upgrade that exists in the save
        Image image = currentUpgradeButtons[currentUpgradesCount].transform.Find("UpgradeImage").GetComponent<Image>();
        image.sprite = upgradeButtons[index].transform.Find("UpgradeImage").GetComponent<Image>().sprite;
        image.gameObject.SetActive(true);
        //add it to the upgrades array
        upgradesArr[index] = 1;
        //currentUpgradeButtons[currentUpgradesCount].onClick.RemoveAllListeners();
        int tempUpgradesCount = currentUpgradesCount;
        
        currentUpgradeButtons[currentUpgradesCount].onClick.AddListener(() =>
        {
            //add a listener to remove it from arr and decrease the current upgrades count
            upgradesArr[index] = 0;
            Debug.Log("remove upgrade from current upgrades: " + index);
            currentUpgradeButtons[tempUpgradesCount].onClick.RemoveAllListeners();
            image.gameObject.SetActive(false);
            currentUpgradesCount--;
        });
        currentUpgradesCount++;
    }
}
