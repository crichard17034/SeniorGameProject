using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlayer : MonoBehaviour
{
    public int damageValue;
    public string element;

    //set's the enemy's attack type to an element
    public void setElementDamage(string enemyElement)
    {
        element = enemyElement;
    }

    //When colliding with a player-tagged object, the player's lose health method is called while 
    //taking in the damage taken and element of damage.
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().loseHealth(damageValue, element);
        }
    }
}
