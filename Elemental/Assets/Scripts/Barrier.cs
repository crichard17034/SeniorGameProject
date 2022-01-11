using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public string elementWeakness;
    public AudioSource breakSound;

    //checks if the player's element from the sword is the same as the barrier's weakness and destroys it if true
    public void checkForBreak(string playerElement)
    {
        if(playerElement == elementWeakness)
        {
            breakSound.Play();
            Destroy (gameObject);
            Debug.Log("Destroyed");
        }
        else
        {
            Debug.Log("Wrong Element");
        }
        
    }
}
