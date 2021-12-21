using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPlayer : MonoBehaviour
{
    public int healValue;

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerController>().gainHealth(healValue);
    }
}
