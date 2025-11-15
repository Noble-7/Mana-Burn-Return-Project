using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private GameManager gameManager;

    private float health;
    private float armor;



    private float projectileSpeed;
    private float projectileDamage;
    public GameObject projectileRef;

    private float speed;
    private Rigidbody2D rb;
    private float attackRate;
    private float attackDuration;
    private Vector2 direction = new Vector2(0, 0);

    private bool onCooldown = false;

    public GameObject firepoint;
    public PlayerBehaviour playerRef;
    public enum enemyType {MAGE, FIGHTER };

    public enemyType enemy;


    public void takeDamage(float damage)
    {
        if(damage > armor)
        {
            health = health - damage + armor;
        }
        else
        {
            Debug.Log("Enemy armor is greater than your damage!");
        }

        if(health <= 0)
        {
            gameManager.allEnemies.Remove(this);
            if (gameManager.allEnemies.Count == 0)
            {
                playerRef.hasKey = true;
                Debug.Log("Player has key");
            }
            Destroy(gameObject);
        }
    }

    private void Awake()
    {

        gameManager = FindObjectOfType<GameManager>();
        

        switch (enemy)
        {
            case enemyType.MAGE:
            health = 10;
            armor = 0;
            projectileSpeed = 10;
            projectileDamage = 2;
            attackRate = 2.0f;
            break;

            case enemyType.FIGHTER:
            health = 10;
            armor = 3;
            projectileDamage = 5;
            attackRate = 1.75f;
            attackDuration = 0.2f;
            rb = GetComponent<Rigidbody2D>();
            speed = 1.0f;
            firepoint.gameObject.SetActive(false);
            break;

        }

        playerRef = FindAnyObjectByType<PlayerBehaviour>();

    }

    private void FixedUpdate()
    {
        switch (enemy)
        {
            case enemyType.MAGE:
            break;
            case enemyType.FIGHTER:
            direction = playerRef.transform.position - transform.position;
            rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
            break;

        }
    }

    private void Update()
    {


        switch (enemy)
        {
            case enemyType.MAGE:

                firepoint.transform.up = playerRef.transform.position - transform.position;
                if (!onCooldown)
                {
                    onCooldown = true;
                    GameObject new_projectile = Instantiate(projectileRef, transform.position, firepoint.transform.rotation);
                    new_projectile.GetComponent<EnemyProjectileBehaviour>().Setup(projectileSpeed, projectileDamage);
                    StartCoroutine(cooldown());
                }
            break;

            case enemyType.FIGHTER:
            Quaternion rotation = Quaternion.LookRotation(transform.position - playerRef.transform.position, Vector3.forward);
            firepoint.transform.rotation = rotation;
            firepoint.transform.eulerAngles = new Vector3(0, 0, firepoint.transform.eulerAngles.z - 90);
            if (!onCooldown)
            {
                onCooldown = true;
                firepoint.gameObject.SetActive(true);
                firepoint.gameObject.GetComponent<meleeBehaviour>().damage = projectileDamage;
                StartCoroutine(cooldown());
            }
            break;

        }
    }

    IEnumerator cooldown()
    {

        switch (enemy)
        {
            case enemyType.MAGE:
            yield return new WaitForSeconds(attackRate);
            onCooldown = false;
            break;
            case enemyType.FIGHTER:
                yield return new WaitForSeconds(attackDuration);
                firepoint.gameObject.SetActive(false);
                yield return new WaitForSeconds(attackRate);
                onCooldown = false;
            break;
        }

    }


}
