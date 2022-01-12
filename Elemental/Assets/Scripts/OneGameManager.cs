using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneGameManager : MonoBehaviour
{
    static OneGameManager instance;
    
    //Ensures that only one game manager is active at all times.
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
