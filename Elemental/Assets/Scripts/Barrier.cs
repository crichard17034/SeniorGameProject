using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public string elementNeeded;

    void OnTriggerEnter(Collider other)
    {
        //other.gameObject.GetComponent<PlayerController>().collectGem(gemName);
        Destroy(gameObject);
    }
}
