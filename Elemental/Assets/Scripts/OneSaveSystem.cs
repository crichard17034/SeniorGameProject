using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSaveSystem : MonoBehaviour
{
    static OneSaveSystem instance;
    
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
