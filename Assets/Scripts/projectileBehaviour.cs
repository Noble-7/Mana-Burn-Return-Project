using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class projectileBehaviour : MonoBehaviour
{
    public float speed;
    public float damage;

    [SerializeField]
    private Rigidbody2D rb;


    private void Start()
    {
        Destroy(gameObject, 1);
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed;


    }

    public void Setup(float s_speed, float s_damage)
    {
        speed = s_speed;
        damage = s_damage;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with " + other.gameObject);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().takeDamage(damage);
        }
    }



}
