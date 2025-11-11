using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeBehaviour : MonoBehaviour
{
    public float damage;


    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with " + other.gameObject);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().takeDamage(damage);
        }
    }
}
