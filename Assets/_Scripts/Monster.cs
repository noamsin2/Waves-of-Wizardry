using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private const float HIT_COOLDOWN = 1.5f;
    public static EventHandler OnEnemyDeath;

    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private int maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int attack;
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    private Transform target;
    [SerializeField] private int exp;
    [SerializeField] private int gold;
    private float hitTimer = Mathf.Infinity;
    private Vector2 moveDirection;
    private int health;
    private bool isDead = false;
    private float wave;
    private Vector2 scale;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        wave = WaveManager.Instance.wave;
        maxHealth += (int)wave / 2;
        moveSpeed += wave / 20 ;
        attack += (int)wave / 5;
        exp += (int)wave / 2;
        health = maxHealth;
        target = GameObject.Find("PlayerLogic").transform;
        scale = new Vector2(transform.localScale.x, transform.localScale.y);
        
    }



    void Update()
    {
        if (target && !isDead)
        {
            if(transform.position.x - target.position.x > WaveManager.SPAWN_DISTANCE_RADIUS + 1)
            {
                transform.position = new Vector2(target.position.x - WaveManager.SPAWN_DISTANCE_RADIUS, transform.position.y);
            }
            else if(target.position.x - transform.position.x > WaveManager.SPAWN_DISTANCE_RADIUS + 1)
            {
                transform.position = new Vector2(target.position.x + WaveManager.SPAWN_DISTANCE_RADIUS, transform.position.y);
            }
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            if(direction.x < 0)
                transform.localScale = new Vector2(scale.x, scale.y* -1);
            else
                transform.localScale = new Vector2(scale.x, scale.y);
            moveDirection = direction;
            
        }
        

        hitTimer += Time.deltaTime;
        
    }
    private void FixedUpdate()
    {
        if (target && !isDead)
        {
            if(Mathf.Abs(target.position.x - transform.position.x) + Mathf.Abs(target.position.y - transform.position.y) <= 3)
                rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            else
                rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed*1.5f;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        //Debug.Log(health);
        anim.SetTrigger("Hurt");
        AudioSource.PlayClipAtPoint(hurtClip, transform.position, AudioManager.Instance.GetTotalVolume());
        
        if (health <= 0 && !isDead)
            Die();

    }
    private void Die()
    {
        isDead = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        rb.velocity = new Vector2(0, 0);
        ExperienceManager.Instance.AddExperience(exp);
        StartCoroutine(delayDeath());
        AudioSource.PlayClipAtPoint(dieClip, transform.position, AudioManager.Instance.GetTotalVolume());
        anim.SetTrigger("Death");
        ItemDropManager.Instance.DropItem(transform.position);
        GoldManager.Instance.DropGold(gold, transform.position);
        Destroy(gameObject, 1);

        WaveManager.Instance.DecreaseEnemyOnDeath();
    }
    private IEnumerator delayDeath()
    {
        yield return new WaitForSeconds(0.5f);
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if (hitTimer >= HIT_COOLDOWN)
            {
                col.gameObject.GetComponent<PlayerStats>().TakeDamage(attack);
                hitTimer = 0;
            }
        }
    }
}
