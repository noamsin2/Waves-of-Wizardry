using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsAttack : MonoBehaviour
{
    private Transform player;
    private const float OFFSET_Y = 1.5f;
    private const float OFFSET_X = 1.5f;
    [SerializeField] private int Multiplier_OFFSET_Y;//different position for each souls attack
    [SerializeField] private int Multiplier_OFFSET_X = 0;//different position for each souls attack
    //private Vector2 newVector;
    private Rigidbody2D rb;
    [SerializeField] private bool isHorizontal = true;
    private float moveX = 0;
    private float moveY = 0;
    private bool isFacingRight = false;
    private float level;
    private float moveDirection = -1;
    private float damageTimer = Mathf.Infinity;
    private void Awake()
    {
        player = GameObject.Find("PlayerLogic").transform;
        rb = GetComponent<Rigidbody2D>();
        //newVector = new Vector2();
    }
    void Start()
    {
        player.GetComponent<PlayerStats>().OnSpeedChange += PlayerStats_OnSpeedChange;
        ChangeSpeed();
        transform.position = new Vector2(player.position.x - Multiplier_OFFSET_Y - moveX + OFFSET_X * Multiplier_OFFSET_X,
             player.position.y + OFFSET_Y * Multiplier_OFFSET_Y + moveY + Multiplier_OFFSET_X);
    }

    private void PlayerStats_OnSpeedChange(object sender, System.EventArgs e)
    {
        ChangeSpeed();
    }
    private void ChangeSpeed()
    {
        if (isHorizontal)
        {
            moveX = (float)(player.GetComponent<PlayerStats>().GetSpeedFinalValue() / 40);
        }
        else
        {
            moveY = (float)player.GetComponent<PlayerStats>().GetSpeedFinalValue() / 40;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHorizontal)
        {
            transform.position = new Vector2(transform.position.x + moveX* moveDirection, player.position.y + OFFSET_Y * Multiplier_OFFSET_Y);
            if (transform.position.x - player.position.x > 2 && isFacingRight)
            {
                moveDirection *= -1;
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                isFacingRight = false;

            }
            else if (transform.position.x - player.position.x < -2 && !isFacingRight)
            {
                moveDirection *= -1;
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                isFacingRight = true;

            }
        }
        else
        {
            transform.position = new Vector2(player.position.x + OFFSET_X * Multiplier_OFFSET_X, transform.position.y + moveY * moveDirection);
            if (transform.position.y - player.position.y > 2 && isFacingRight)
            {
                moveDirection *= -1;
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                isFacingRight = false;

            }
            else if (transform.position.y - player.position.y < -2 && !isFacingRight)
            {
                moveDirection *= -1;
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                isFacingRight = true;

            }
        }
        damageTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Monster")
        {
            if (damageTimer >= 0f)
            {
                col.gameObject.GetComponent<Monster>().TakeDamage((int)level);
                damageTimer = 0;
            }
        }
    }
    public void SetLevel(int level)
    {
        this.level = Mathf.Max(level / 5, 1);
        Debug.Log("Level : " + this.level);
    }
}
