using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Door connectedDoor;
    public bool isDoorLocked = false;

    public Transform spawnPoint;

    public Vector3 cameraOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isDoorLocked)
        {
            Camera.main.transform.position = connectedDoor.cameraOffset;
            collision.transform.position = connectedDoor.spawnPoint.position;
        }
        else if (collision.tag == "Player" && isDoorLocked && collision.GetComponent<PlayerBehaviour>().hasKey)
        {
            collision.GetComponent<PlayerBehaviour>().hasKey = false;
            isDoorLocked = false;
            Camera.main.transform.position = connectedDoor.cameraOffset;
            collision.transform.position = connectedDoor.spawnPoint.position;
        }

    }
}
