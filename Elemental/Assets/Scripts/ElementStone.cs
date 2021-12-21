using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStone : MonoBehaviour
{
    public string gemName;

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerController>().collectGem(gemName);
    }
}
