using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{
    //Game Manager
    public GameManager gameManager;

    //Stuff that moves with the player
    public Animator anim;
    public Rigidbody2D rb;
    [SerializeField]
    private GameObject projectileRef;
    [SerializeField]
    private meleeBehaviour meleeRef;
    private string lastDirection = "";


    //Necessary for controls to work
    private Vector3 mousePosition = new Vector3(0, 0, 0);
    private Vector3 playerMovement = new Vector3(0, 0, 0);
    private Vector2 rightJoystickInput;
    private bool flipRotation = true;
    public GameObject firepoint;

    //Player Stats
    public int gold;
    public int maxHealth;
    public int health;
    public float speed;
    public float meleeDamage;
    public float projectileDamage;
    public float projectileSpeed;
    private float mAttackCooldownTime;
    private float sAttackCooldownTime;
    private float sAttackDuration;

    //Player mid-game stats?
    public bool hasKey;


    //Controls/Keybinds
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private InputActionReference movement;
    [SerializeField]
    private InputActionReference look;
    [SerializeField]
    private InputActionReference mAttack;
    [SerializeField]
    private InputActionReference sAttack;

    //Needed for calculations regarding attacking
    private bool mAttackCooldown = false;
    private bool sAttackCooldown = false;









    private void Start()
    {
        //Enable the controls whenever the game starts
        movement.action.Enable();
        look.action.Enable();
        mAttack.action.Enable();
        sAttack.action.Enable();

        //Make sure the player's melee weapon isn't attacking when the game starts
        meleeRef.gameObject.SetActive(false);

        //Get and set a reference to the game manager to populate stats
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gold = gameManager.playerGold;
            maxHealth = gameManager.playerMaxHealth;
            health = gameManager.playerHealth;
            speed = gameManager.playerSpeed;
            sAttackDuration = gameManager.playerSAttackDuration;
            projectileSpeed = gameManager.playerProjectileSpeed;
            projectileDamage = gameManager.playerProjectileDamage;
            meleeDamage = gameManager.playerMeleeDamage;
            mAttackCooldownTime = gameManager.playerMAttackCooldownTime;
            sAttackCooldownTime = gameManager.playerSAttackCooldownTime;
        }
    }

    void Update()
    {
        //CONTROLS, here we check to see if we are on K&M
        if (playerInput.currentControlScheme == "Keyboard and Mouse")
        {
            //The position of the mouse is read with the new unity input system
            mousePosition = look.action.ReadValue<Vector2>();
            //Shenanigans (to be honest it doesn't work without this code so, I guess just leave this here)
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //Stuff regarding the player's look rotation, so like, for shooting
            Quaternion rotation = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
            firepoint.transform.rotation = rotation;
            firepoint.transform.eulerAngles = new Vector3(0, 0, firepoint.transform.eulerAngles.z);

            //Not sure about this atm
            playerMovement = movement.action.ReadValue<Vector2>();

           
        }
        else if (playerInput.currentControlScheme == "Controller")
        {
            //Run this otherwise if we are on controller (In theory this could be a switch statement if we wanna include like, touch controls. But like lol mobile

            
            rightJoystickInput = look.action.ReadValue<Vector2>();
            float horizontal = rightJoystickInput.x;
            float vertical = rightJoystickInput.y;

            float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
            angle = flipRotation ? -angle : angle;

            playerMovement = movement.action.ReadValue<Vector2>();

            if (look.action.IsPressed())
            {
                firepoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                float moveAngle = Mathf.Atan2(playerMovement.x, playerMovement.y) * Mathf.Rad2Deg;
                moveAngle = flipRotation ? -moveAngle : moveAngle;
                firepoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, moveAngle));
            }

            
            //transform.position += playerMovement * speed * Time.deltaTime;

        }

        

        if (mAttack.action.triggered)
        {
            if (!mAttackCooldown)
            {
                Fire();
                mAttackCooldown = true;
                StartCoroutine(MainAttackCooldown());
            }
        }

        if (sAttack.action.triggered)
        {
            if (!sAttackCooldown)
            {
                Swing();
                sAttackCooldown = true;
                StartCoroutine(SecondaryAttackCooldown());
            }
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter(playerMovement);
        HandleAnimations();
    }

    public void Fire()
    {
        GameObject new_projectile = Instantiate(projectileRef, transform.position, firepoint.transform.rotation);
        new_projectile.GetComponent<projectileBehaviour>().Setup(projectileSpeed, projectileDamage);
    }

    private void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }

    private void HandleAnimations()
    {
        // Vector3 dir = ((Vector2)transform.position - look.action.ReadValue<Vector2>()).normalized;

        GetDirection(movement.action.ReadValue<Vector2>().normalized);

        if (movement.action.ReadValue<Vector2>() == Vector2.zero)
        {
            anim.Play(lastDirection + "_Idle");
        }
        else
        {
            anim.Play(lastDirection + "_Walk");
        }
    }

    private Vector3 GetDirection(Vector3 input)
    {
        Vector3 finalDirection = Vector2.zero;
        if (input.y > 0.01f)
        {
            lastDirection = "Up";
            finalDirection = new Vector2(0, 1);
        }
        else if (input.y < -0.01f)
        {
            lastDirection = "Down";
            finalDirection = new Vector2(0, -1);
        }
        else if (input.x > 0.01f)
        {
            lastDirection = "Right";
            finalDirection = new Vector2(1, 0);
        }
        else if (input.x < -0.01f)
        {
            lastDirection = "Left";
            finalDirection = new Vector2(-1, 0);
        }
        else
            finalDirection = Vector2.zero;

        return finalDirection;
    }



    IEnumerator MainAttackCooldown()
    {
        yield return new WaitForSeconds(mAttackCooldownTime);
        mAttackCooldown = false;
    }

    public void Swing()
    {
        meleeRef.gameObject.SetActive(true);
        meleeRef.damage = meleeDamage;
        StartCoroutine(PerformSAttack());
    }

    IEnumerator SecondaryAttackCooldown()
    {
        yield return new WaitForSeconds(sAttackCooldownTime);
        sAttackCooldown = false;

    }

    IEnumerator PerformSAttack()
    {
        yield return new WaitForSeconds(sAttackDuration);
        meleeRef.gameObject.SetActive(false);

    }

    public void takeDamage(float damage)
    {

    }

}
