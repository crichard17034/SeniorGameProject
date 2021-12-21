using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    private int health;
    private int attackValue;
    private string element;

    public Enemy(int health, int attackValue, string element)
    {
        this.health = health;
        this.attackValue = attackValue;
        this.element = element;
    }

    public int getHealth()
    {
        return health;
    }

    public int getAttackValue()
    {
        return attackValue;
    }

    public string getElement()
    {
        return element;
    }
}
