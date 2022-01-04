using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStone : MonoBehaviour
{
    public string gemName;

    //checks if the object colliding with the hitbox is a player object and deletes the gem while adding it to the player
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().collectGem(gemName);
            Destroy(gameObject);
        }
        
    }
}
