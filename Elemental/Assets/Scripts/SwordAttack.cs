using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject player;
    Animator anim;
    Collider swordHitbox;
    private int damageValue;
    private string currentElement;
    bool attacking;

    private void Start()
    {
        anim = GetComponent<Animator>();
        swordHitbox = GetComponent<Collider>();
        swordHitbox.isTrigger = false;
    }

    private void Update()
    {
        checkForAttack();
    }

    public void checkForAttack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetBool("attacking", true);
            swordHitbox.isTrigger = true;
            attacking = true;

        }
        else if (Input.GetButtonUp("Attack"))
        {
            anim.SetBool("attacking", false);
            swordHitbox.isTrigger = false;
            attacking = false;
        }
    }

    //checks if the game object that collided with the sword is an enemy
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        if(other.tag == "Enemy" && attacking == true)
        {
            other.gameObject.GetComponent<EnemyController>().loseHealth(damageValue);
        }
    }

    public void updateAttackStr(int newAtkStr)
    {
        damageValue = newAtkStr;
    }

    public void changeElement(string newElement)
    {
        currentElement = newElement;
    }
}
