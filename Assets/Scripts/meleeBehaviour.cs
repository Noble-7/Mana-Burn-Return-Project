using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeBehaviour : MonoBehaviour
{
    public float damage;
    private string thisObjectName;


    public void OnTriggerEnter2D(Collider2D other)
    {
        thisObjectName = gameObject.tag;
        if (thisObjectName == "Player"){
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyBehaviour>().takeDamage(damage);
            }
        }
        else if (thisObjectName == "Enemy")
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerBehaviour>().takeDamage(damage);
            }
        }
        
    }
}
