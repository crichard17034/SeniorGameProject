using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    GameObject player;
    Animator anim;
    Collider swordHitbox;
    public GameObject blade;
    private int damageValue;
    public string currentElement;
    bool attacking;
    public Material windColor;
    public Material fireColor;
    public Material waterColor;
    public Material baseColor;

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
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && attacking == true)
        {
            other.gameObject.GetComponent<EnemyController>().loseHealth(currentElement, damageValue);
        }
    }

    public void updateAttackStr(int newAtkStr)
    {
        damageValue = newAtkStr;
    }

    //changes the element property of the blade and the color
    public void changeElement(string newElement)
    {
        currentElement = newElement;
        
        if(newElement == "Wind")
        {
            blade.GetComponent<MeshRenderer>().material = windColor;
        }
        else if(newElement == "Fire")
        {
            blade.GetComponent<MeshRenderer>().material = fireColor;
        }
        else if(newElement == "Water")
        {
            blade.GetComponent<MeshRenderer>().material = waterColor;
        }
        else
        {
            blade.GetComponent<MeshRenderer>().material = baseColor;
        }
    }
}
