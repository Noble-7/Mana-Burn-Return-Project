using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float health = 10.0f;
    public float armor = 1.0f;


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
            StartCoroutine(SpawnEnemy());
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5.0f);
        Instantiate(this.gameObject);
    }
}
