using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillsUI : MonoBehaviour
{
    public static SkillsUI Instance { get; private set; }
    public delegate void HandleSkillAssign(float time);
    public event EventHandler OnSkillAssign;
    [SerializeField] private Transform []itemSkillContainers;
    private List<IUpgrade> totalUpgrades;
    private List<IUpgrade> upgradeHolder;
    private void Awake()
    {
        Instance = this;
    }
    public void ChooseUpgrade(int index)
    {
        /*foreach(IUpgrade upgrade in totalUpgrades)
        {
            if(upgrade.GetId() == upgradeHolder[index].GetId())
        }*/
        //upgradeHolder[index].Upgrade();
        UpgradeList.upgrades[index].Upgrade();
        //totalUpgrades.Add(upgradeHolder[index]);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpgradeList.Initialize();
        totalUpgrades = new List<IUpgrade>();
        Hide();
    }

    public void Show()
    {
        ShowUpgradeChoices();
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void AssignSkills()
    {
        OnSkillAssign?.Invoke(this, EventArgs.Empty);
        //foreach(Transform container in itemSkillContainers)
        Hide();

    }
    private void ShowUpgradeChoices()
    {

        Extensions.Shuffle(UpgradeList.upgrades);

        //upgradeHolder = new List<IUpgrade>();
        int i = 0;
        foreach (Transform container in itemSkillContainers)
        {
           // int upgradeIndex = UnityEngine.Random.Range(0, tempUpgrades.Count);
            container.Find("Upgrade").GetComponent<Image>().sprite = UpgradeList.upgrades[i].GetSprite();
            container.Find("SkillName").GetComponent<TextMeshProUGUI>().text = UpgradeList.upgrades[i].GetDescription();
            container.Find("SkillLevel").GetComponent<TextMeshProUGUI>().text = "Skill Lv: " + UpgradeList.upgrades[i].GetLevel().ToString();
          //  upgradeHolder.Add(tempUpgrades[upgradeIndex]);
            i++;
            // upgrade.onClick.AddListener(tempUpgrades[upgradeIndex].Upgrade);
            /*Button upgrade = container.Find("Upgrade").GetComponent<Button>();
            upgrade = tempUpgrades[upgradeIndex];*/
            //tempUpgrades.RemoveAt(upgradeIndex);
        }
    }
    
}
