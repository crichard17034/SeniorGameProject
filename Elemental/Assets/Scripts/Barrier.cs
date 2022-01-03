using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public string elementWeakness;

    public void checkBarrierBreak(string playerElement)
    {
        if(elementWeakness == playerElement)
        {
            Destroy(gameObject);
        }
    }
}
