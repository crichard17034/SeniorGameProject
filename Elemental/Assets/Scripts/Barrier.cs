using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public string elementWeakness;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.GetComponent<PlayerController>().getElement() == elementWeakness)
            {
                Destroy(gameObject);
            }
        }
    }
}
