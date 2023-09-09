using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    private const float OFFSET_X_FIRE = 1f;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float lifetime;
    [SerializeField] private Transform hitPrefab;
    [SerializeField] private int damage;
    [SerializeField] private float fadeoutTime = 0.5f;
    [SerializeField] private int manaCost;
    [SerializeField] private int force;
    [SerializeField] private int skillDamageScale;
    [SerializeField] private float skillLifeScale;
    private Animator anim;
    private Vector3 shootDirection;
    [SerializeField] private int enemiesHitCounter;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private bool isAttachedPlayer = false;
    private Transform player;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (isAttachedPlayer)
            player = GameObject.Find("PlayerLogic").transform;
    }
    void Start()
    {
        if (attackSound != null)
            AudioSource.PlayClipAtPoint(attackSound, transform.position, AudioManager.Instance.GetTotalVolume());
    }
    public int Setup(Vector3 shootDir, int attack, int skillLevel)
    {
        this.damage += attack + skillLevel * skillDamageScale;
        if(skillLifeScale != 0)
            this.lifetime += skillLevel / skillLifeScale;
        shootDirection = shootDir;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDirection));
        Destroy(gameObject, this.lifetime);
        if (fadeoutTime != 0)
        {
            StartCoroutine(AttackFade());
        }
        return manaCost;
    }


    // Update is called once per frame
    private void Update()
    {
        
        if (isAttachedPlayer)
        {
            transform.position = player.transform.position + OFFSET_X_FIRE * shootDirection + shootDirection * projectileSpeed * Time.deltaTime;
        }
        else
            transform.position += shootDirection * projectileSpeed * Time.deltaTime;
    }
    private IEnumerator AttackFade()
    {
        yield return new WaitForSeconds(lifetime - fadeoutTime);
        anim.SetTrigger("Fade");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Monster monster = col.GetComponent<Monster>();
        if(monster!= null)
        {
            enemiesHitCounter--;
            if (enemiesHitCounter == 0)
            {
                Destroy(gameObject);
            }
            if (hitPrefab != null)
            {
                Transform hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);
                Destroy(hit.gameObject, 1f);
            }

            monster.TakeDamage(damage);
            
            
        }
    }
}
