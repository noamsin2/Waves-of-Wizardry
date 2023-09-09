using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnTeleport;

    private const float TELEPORT_CD = 1f;
    private Rigidbody2D rb;
    private Animator anim;
    private GameInput GI;
    private PlayerStats ps;
    private CircleCollider2D cCollider;
    [SerializeField] private float speedLimiter;
    private Vector2 inputVector;
    private Vector2 direction;
    private Vector2 lastDirection;
    [SerializeField] private Transform bubble;
    private bool input = true;
    private float teleportTimer = Mathf.Infinity;

    private void Awake()
    {
        ps = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cCollider = GetComponent<CircleCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInput.Instance;
        direction = new Vector2(0, 0);
        GameManager.Instance.OnOpenMenu += GameManager_OnOpenMenu;
        GameManager.Instance.OnCloseMenu += GameManager_OnCloseMenu;
        ps.OnPlayerDeath += PlayerStats_OnPlayerDeath;
    }

    private void GameManager_OnCloseMenu(object sender, EventArgs e)
    {
        input = true;
    }

    private void PlayerStats_OnPlayerDeath(object sender, EventArgs e)
    {
        cCollider.enabled = false;
        //bubble.gameObject.SetActive(false);
        GI.DisablePlayerInput();
    }

    private void GameManager_OnOpenMenu(object sender, EventArgs e)
    {
        input = false;
        //rb.isKinematic = false;
    }


    private void Update()
    {

        //get input vector from GameInput
        if (input)
            inputVector = GI.GetMovementVectorNormalized();
        //change the paramater in the animator
        anim.SetFloat("moveInput", Mathf.Max(Mathf.Abs(inputVector.x), Mathf.Abs(inputVector.y)));
        
        RotateCharacter();
        

    }

    private void FixedUpdate()
    {
        //if both keys are pressed limit the speed (diagonal movement)
        if (inputVector.x != 0 && inputVector.y != 0)
        {
            rb.velocity = new Vector2(inputVector.x * speedLimiter, inputVector.y * speedLimiter) * ps.GetSpeedFinalValue();
        }
        else
        {
            rb.velocity = new Vector2(inputVector.x, inputVector.y) * ps.GetSpeedFinalValue();
        }
        Teleport();

    }

    private void Teleport()
    {
        if (GI.IsTeleportKeyPressed() && teleportTimer >= TELEPORT_CD)
        {
            if (ps.GetMana() >= PlayerStats.TELEPORT_MANA_COST)
            {
                anim.SetTrigger("Teleport");
                Vector2 newPosition = (Vector2)transform.position + inputVector * ps.GetSpeedFinalValue() / 4;
                if (newPosition.y >= 5)
                    newPosition.y = 4.95f;
                else if (newPosition.y <= -4.7)
                    newPosition.y = -4.65f;
                transform.position = newPosition;
                teleportTimer = 0;
                OnTeleport?.Invoke(this, EventArgs.Empty);
            }
        }
        teleportTimer += Time.deltaTime;
    }
    private void RotateCharacter()
    {

        direction.x = inputVector.x;
        direction.y = inputVector.y;
        if (direction != lastDirection)
        {
            if (inputVector.x < -0.01)
            {
                //flip character in the direction
                transform.localScale = new Vector2(-1f, 1f);
            }
            else if (inputVector.x > 0.01)
            {
                //flip character in the direction
                transform.localScale = new Vector2(1f, 1f);

            }
            //rotate character upwards or downwards if only up or down is pressed
            /*if (inputVector.x == 0 && inputVector.y != 0)
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
            }*/
            transform.rotation = Quaternion.FromToRotation(new Vector3(transform.localScale.x, 0, 0), direction);
            

        }
        lastDirection = direction;
    }
   
}
