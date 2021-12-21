using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlayer : MonoBehaviour
{
    public int damageValue;
    private string element;

    public void setElementDamage(string enemyElement)
    {
        element = enemyElement;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().loseHealth(damageValue, element);
        }
    }
}
