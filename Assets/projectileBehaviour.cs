using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class projectileBehaviour : MonoBehaviour
{
    public float speed;

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

    public void Setup(float s_speed)
    {
        speed = s_speed;
    }

}
