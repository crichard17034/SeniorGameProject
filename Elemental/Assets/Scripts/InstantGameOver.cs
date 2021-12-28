using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantGameOver : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GameManager>().gameOver();
        }
    }
}
