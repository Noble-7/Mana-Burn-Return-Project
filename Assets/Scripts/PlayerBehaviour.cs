using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{

    private Vector3 mousePosition = new Vector3(0, 0, 0);
    private Vector3 playerMovement = new Vector3(0, 0, 0);


    private Vector2 rightJoystickInput;
    private bool flipRotation = true;

    public float speed = 5;

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

    private bool mAttackCooldown = false;
    private bool sAttackCooldown = false;
    private float mAttackCooldownTime = 1.0f;
    private float sAttackCooldownTime = 1.0f;

    [SerializeField]
    private GameObject projectileRef;

    public float projectileSpeed = 5;

    private void Start()
    {
        movement.action.Enable();
        look.action.Enable();
        mAttack.action.Enable();
        sAttack.action.Enable();
    }

    void Update()
    {

        if (playerInput.currentControlScheme == "Keyboard and Mouse")
        {
            mousePosition = look.action.ReadValue<Vector2>();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Quaternion rotation = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
            transform.rotation = rotation;
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);


            playerMovement = movement.action.ReadValue<Vector2>();
            //Debug.Log(playerMovement);

            transform.position += playerMovement * speed * Time.deltaTime;
        }
        else if (playerInput.currentControlScheme == "Controller")
        {

            //Debug.Log(look.action.ReadValue<Vector2>());

            
            rightJoystickInput = look.action.ReadValue<Vector2>();
            float horizontal = rightJoystickInput.x;
            float vertical = rightJoystickInput.y;

            float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
            angle = flipRotation ? -angle : angle;

            playerMovement = movement.action.ReadValue<Vector2>();

            if (look.action.IsPressed())
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                float moveAngle = Mathf.Atan2(playerMovement.x, playerMovement.y) * Mathf.Rad2Deg;
                moveAngle = flipRotation ? -moveAngle : moveAngle;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, moveAngle));
            }

            
            transform.position += playerMovement * speed * Time.deltaTime;

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
        Debug.Log(mAttackCooldown);

    }

    public void Fire()
    {
        GameObject new_projectile = Instantiate(projectileRef, transform.position, transform.rotation);
        new_projectile.GetComponent<projectileBehaviour>().Setup(projectileSpeed);
    }

    IEnumerator MainAttackCooldown()
    {
        yield return new WaitForSeconds(mAttackCooldownTime);
        mAttackCooldown = false;
    }

}
