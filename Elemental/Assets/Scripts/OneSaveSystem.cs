using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSaveSystem : MonoBehaviour
{
    static OneSaveSystem instance;
    
    //Ensures that only one save system is active at all times.
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
