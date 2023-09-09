using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStatsUI : MonoBehaviour
{
    public static PlayerStatsUI Instance { get; private set; }
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Slider expBar;
    [SerializeField] private TextMeshProUGUI hpValue;
    [SerializeField] private TextMeshProUGUI mpValue;
    [SerializeField] private TextMeshProUGUI expValue;

    private void Awake()
    {
        Instance = this;
    }

    public void SetMaxHP(int maxHP)
    {
        healthBar.maxValue = maxHP;
        SetHP((int)healthBar.value);
    }
    public void SetHP(int HP)
    {
        healthBar.value = HP;
        hpValue.text = healthBar.value.ToString() + " / " + healthBar.maxValue.ToString();
    }
    public void SetMaxMP(int maxMP)
    {
        manaBar.maxValue = maxMP;
    }
    public void SetMP(int MP)
    {
        manaBar.value = MP;
        mpValue.text = manaBar.value.ToString() + " / " + manaBar.maxValue.ToString();
    }
    public void SetMaxEXP(int maxEXP)
    {
        expBar.maxValue = maxEXP;
    }
    public void SetEXP(int EXP)
    {
        expBar.value = EXP;
        expValue.text = expBar.value.ToString() + " / " + expBar.maxValue.ToString();
    }

}
