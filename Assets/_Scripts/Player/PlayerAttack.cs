using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerSkills pSkills;
    private PlayerStats ps;
    private Action attackFunction;
    private Animator anim;
    private GameInput GI;
    private float attackCooldownTimer = Mathf.Infinity;

    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        ps = GetComponent<PlayerStats>();
        pSkills = GetComponent<PlayerSkills>();
        pSkills.OnSkillUpgrade += PlayerSkills_OnSkillUpgrade;
    }
    void Start()
    {
       
        GI = GameInput.Instance;
    }

    private void PlayerSkills_OnSkillUpgrade(object sender, EventArgs e)
    {
        attackFunction = pSkills.GetAttackFunction();
        //Debug.Log("Function set");
    }

    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if (GI.IsAttackPressed() && attackCooldownTimer >= 1 / ps.GetAttackSpeed())
        {
            anim.SetTrigger("Attack");
            //StartCoroutine(AttackWaitTime());
            attackFunction();
            attackCooldownTimer = 0;
        }
        attackCooldownTimer += Time.deltaTime;
    }
   /* private void WaterAttack()
    {
        Transform projectile = Instantiate(waterBall, attackPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Setup(attackPoint.position - transform.position, ps.GetAttack(), ps.attackRange);
    }*/
    private IEnumerator AttackWaitTime()
    {
        yield return new WaitForSeconds(0.3f);
        attackFunction();
    }
}
