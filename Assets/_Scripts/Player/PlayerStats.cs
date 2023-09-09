using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnSpeedChange;
    [SerializeField] private Animator anim;
    [SerializeField] public string playerName { get; private set; }
    [SerializeField] private int level;
    [SerializeField] private float currentEXP;
    [SerializeField] private int maxEXP;
    [SerializeField] private int currentHealth;
    [SerializeField] private int currentMana;
    
    [field: SerializeField] private float attackSpeed;
    [field: SerializeField] private int att;
    [field: SerializeField] private int maxHealth;
    [field: SerializeField] private int maxMana;
    [field: SerializeField] private int speed;
    private float manaRegen = 2;//2 per second
    private float healthRegen = 1;//1 per second
    private float hpRegenTimer = 0;
    private float mpRegenTimer = 0;
    private bool isDead = false;
    public const int TELEPORT_MANA_COST = 20;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        ExperienceManager.Instance.OnExpChange += HandleExperienceChange;
        GetComponent<PlayerController>().OnTeleport += PlayerController_OnTeleport;
        InitializeHealth();
        InitializeMana();
        InitializeExp();
        
    }

    private void Update()
    {
        if(hpRegenTimer >= 1 / healthRegen && !isDead)
        {
            AddHealth(1);
            hpRegenTimer = 0;
        }
        if (mpRegenTimer >=  1 / manaRegen && !isDead)
        {
            AddMana(1);
            mpRegenTimer = 0;
        }
        hpRegenTimer += Time.deltaTime;
        mpRegenTimer += Time.deltaTime;
    }

    private void PlayerController_OnTeleport(object sender, EventArgs e)
    {

        AddMana(-TELEPORT_MANA_COST);
    }
    public int GetMana()
    {
        return currentMana;
    }
    private void HandleExperienceChange(float exp)
    {
        currentEXP += exp;
        if (currentEXP >= maxEXP)
        {
            LevelUp();
        }
        UpdateEXPUI();
    }
    private void LevelUp()
    {
        AddHealth(maxHealth);
        AddMana(maxMana);
        currentEXP = currentEXP - maxEXP;
        level++;
        maxEXP = (int)((maxEXP + level) * 1.5);
        UpdateMaxEXPUI();
        Player.Instance.LevelUp();
    }
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    public float GetSpeedFinalValue()
    {
        return speed;
    }

    public void TakeDamage(int damage)
    {

        AddHealth(-damage);
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0)
            Die();
            
    }
    public virtual void Die()
    {
        isDead = true;
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        StartCoroutine(DelayDeath());
        anim.SetTrigger("Death");
        GameManager.Instance.SetGameOverState();
    }
    private IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(0.5f);
    }
    private void AddHealth(int healthBonus)
    {
        currentHealth += healthBonus;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        UpdateHPUI();
    }
    private void AddMana(int manaBonus)
    {
        currentMana += manaBonus;
        if (currentMana > maxMana)
            currentMana = maxMana;
        UpdateMPUI();
    }
    public int GetAttack()
    {
        return att;
    }

    public void InitializeHealth()
    {
        currentHealth = maxHealth;
        UpdateMaxHPUI();
        UpdateHPUI();
    }
    public void InitializeMana()
    {
        currentMana = maxMana;
        UpdateMaxMPUI();
        UpdateMPUI();
    }
    private void InitializeExp()
    {
        UpdateMaxEXPUI();
        UpdateEXPUI();
    }

    private void UpdateMaxHPUI()
    {
        PlayerStatsUI.Instance.SetMaxHP(maxHealth);
    }
    private void UpdateHPUI()
    {
        PlayerStatsUI.Instance.SetHP(currentHealth);
    }
    private void UpdateMaxMPUI()
    {
        PlayerStatsUI.Instance.SetMaxMP(maxMana);
    }
    private void UpdateMPUI()
    {
        PlayerStatsUI.Instance.SetMP(currentMana);
    }
    private void UpdateMaxEXPUI()
    {
        PlayerStatsUI.Instance.SetMaxEXP(maxEXP);
    }
    private void UpdateEXPUI()
    {
        PlayerStatsUI.Instance.SetEXP((int)currentEXP);
    }
    public void IncreaseAttack(int amount)
    {
        att += amount;
    }
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        UpdateMaxHPUI();
    }
    public void IncreaseSpeed(int amount)
    {
        speed += amount;
        OnSpeedChange?.Invoke(this, EventArgs.Empty);
    }
    public void IncreaseAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

}
