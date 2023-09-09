using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    const float BONUS_BLOOD_ATTACK_RANGE = 0.25f;

    public event EventHandler OnSkillUpgrade;
    [SerializeField] private Transform attackPoint;
    
    private PlayerStats ps;
    //on button press attacks
    private Action attackFunction;
    [SerializeField] private Transform waterBall;
    [SerializeField] private int waterBallLevel = 0;
    [SerializeField] private Transform waterCannon;

    //automated attacks
    private Action autoAttackFunction;
    [SerializeField] private float autoAttackInterval;
    [SerializeField] private Transform attackPointMid;
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private float attackRange;
    [SerializeField] private int CrescentAttackLevel = 0;
    [SerializeField] private GameObject waterCrescentAttack;
    [SerializeField] private GameObject bloodCrescentAttack;

    //perma attacks
    [SerializeField] private GameObject jellyfishSoulAttack;
    [SerializeField] private GameObject octopusSoulAttack;
    [SerializeField] private GameObject mergedSoulsAttack;
    private int jellyfishSoulLevel = 0;
    private int octopusSoulLevel = 0;
    private bool isMergedSoulsAttackActivated = false;
    private void Awake()
    {
        ps = GetComponent<PlayerStats>();
    }

    void Start()
    {
        WaterAttackActivate();

        StartCoroutine(ActivateAutoAttackFunction());
        ps.OnPlayerDeath += PlayerStats_OnPlayerDeath;
        
        
        
    }
    

    private void MergedSoulsAttackActivate()
    {
        if (octopusSoulLevel + jellyfishSoulLevel >= 10 && (octopusSoulLevel >= 1 && jellyfishSoulLevel >= 1))
        {
            if (!isMergedSoulsAttackActivated)
            {
                mergedSoulsAttack = Instantiate(mergedSoulsAttack, GameObject.Find("Player").transform);
                isMergedSoulsAttackActivated = true;
            }
            SoulsAttack[] souls = mergedSoulsAttack.GetComponentsInChildren<SoulsAttack>();
            souls[0].SetLevel(ps.GetAttack() + octopusSoulLevel + jellyfishSoulLevel);
            souls[1].SetLevel(ps.GetAttack() + octopusSoulLevel + jellyfishSoulLevel);
        }
    }
    public void OctopusAttackActivate()
    {
        if (octopusSoulLevel == 0)
        {
            octopusSoulAttack = Instantiate(octopusSoulAttack, GameObject.Find("Player").transform);
            
        }
        octopusSoulLevel++;
        octopusSoulAttack.GetComponent<SoulsAttack>().SetLevel(ps.GetAttack() + octopusSoulLevel);
        MergedSoulsAttackActivate();
    }
    public void JellyfishAttackActivate()
    {
        if (jellyfishSoulLevel == 0)
        {
            jellyfishSoulAttack = Instantiate(jellyfishSoulAttack, GameObject.Find("Player").transform);
        }
        jellyfishSoulLevel++;
        jellyfishSoulAttack.GetComponent<SoulsAttack>().SetLevel(ps.GetAttack() + jellyfishSoulLevel);
        MergedSoulsAttackActivate();
    }

    private void PlayerStats_OnPlayerDeath(object sender, EventArgs e)
    {
        autoAttackFunction = null;
        waterCrescentAttack.SetActive(false);
        bloodCrescentAttack.SetActive(false);
    }

    private IEnumerator ActivateAutoAttackFunction()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoAttackInterval);
            if (autoAttackFunction != null)
                autoAttackFunction();
        }
    }

    public Action GetAttackFunction()
    {
        return attackFunction;
    }
    public void WaterCrescentAttackActivate()
    {
        CrescentAttackLevel++;
        if (CrescentAttackLevel == 1)
        {
            waterCrescentAttack.SetActive(true);
            autoAttackFunction += WaterCrescentAttack;
        }
        else if (CrescentAttackLevel == 5)
        {
            autoAttackFunction -= WaterCrescentAttack;
            waterCrescentAttack.SetActive(false);
            bloodCrescentAttack.SetActive(true);
            autoAttackFunction += BloodCrescentAttack;
        }
            

    }
    private void DamageMonstersInArea(Collider2D[] monsters)
    {
        foreach(Collider2D monster in monsters)
        {
            AudioManager.Instance.PlayRandomCutSound(transform.position);
            monster.gameObject.GetComponent<Monster>().TakeDamage(ps.GetAttack() + CrescentAttackLevel);
        }
    }
    private void WaterCrescentAttack()
    {

        Collider2D[] monsters = Physics2D.OverlapCircleAll(attackPointMid.position, attackRange, monsterLayer);
        DamageMonstersInArea(monsters);
    }
    private void BloodCrescentAttack()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(attackPointMid.position, attackRange + BONUS_BLOOD_ATTACK_RANGE, monsterLayer);
        DamageMonstersInArea(monsters);
    }
    private void WaterCannonActivate()
    {
        attackFunction -= WaterAttack;
        attackFunction += WaterCannonAttack;
        OnSkillUpgrade?.Invoke(this, EventArgs.Empty);
    }
    private void WaterCannonAttack()
    {
        Transform projectile = Instantiate(waterCannon, attackPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Setup(attackPoint.position - transform.position, ps.GetAttack(), waterBallLevel);
    }
    public void WaterAttackActivate()
    {
        waterBallLevel++;
        if (waterBallLevel == 1)
        {
            attackFunction += WaterAttack;
            OnSkillUpgrade?.Invoke(this, EventArgs.Empty);
        }
        else if (waterBallLevel == 5)
            WaterCannonActivate();
        
        
    }
    
    private void WaterAttack()
    {
        Transform projectile = Instantiate(waterBall, attackPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Setup(attackPoint.position - transform.position, ps.GetAttack(), waterBallLevel);
    }
}
