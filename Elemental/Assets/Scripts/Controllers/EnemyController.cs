using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    public int currentHealth;
    public int maxHealth;
    public int xp;
    public string element;
    public string elementWeakness;
    public string elementResistence;
    NavMeshAgent agent;
    Transform target;
    Vector3 velocity;
    public float lookRadius = 10f;
    public float attackRange = 7f;
    public float attackTimer = 120f;
    public GameObject enemyHealthBar; 
    Collider slimeHitbox;


    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        setElement();
    }

    void Update()
    {
        checkLookRadius();
        checkAttackRadius();
    }

    //Checks if the player is within the enemy's look range while also not within attack range and chases them if true
    private void checkLookRadius()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius && distance > attackRange)
        {
            agent.SetDestination(target.position);
            anim.SetBool("Chasing", true);
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            faceTarget();

            if (agent.remainingDistance > 10f && attackTimer > 0f)
            {
                anim.SetBool("Chasing", true);
            }
        }
        else
        {
            anim.SetBool("Chasing", false);
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    //Checks if the player's distance is within the attack range. If the attackTimer is less than 0, the enemy will attack before resetting the timer to 120.
    private void checkAttackRadius()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= attackRange)
        {
            attackTimer --;
            faceTarget();
            if(attackTimer < 0f)
            {
                anim.SetBool("Attacking", true);
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                attackTimer = 120f;
            }
        }
        else
        {
            anim.SetBool("Attacking", false);
            attackTimer = 120f;
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    //Causes the enemy to face the player when in range
    private void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //When moving, the object's root position is set to move with the animation, preventing the enemy from getting soft locked in place.
    private void OnAnimatorMove()
    {
        Vector3 position = anim.rootPosition;
        transform.position = position;
    } 

    //Creates a visual representation of the look range and attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //On startup, the enemy is given an elemental value
    private void setElement()
    {
        gameObject.GetComponent<DamagePlayer>().setElementDamage(element);
    }

    public void loseHealth(string atkElement, int damageValue)
    {
        if(atkElement == elementWeakness && element != "None")
        {
            currentHealth -= (damageValue * 2);
        }
        else if(atkElement == elementResistence && element != "None")
        {
            currentHealth -= (damageValue /2);
        }
        else
        {
            currentHealth -= damageValue;
        }

        setHealthBar(currentHealth);

        if(currentHealth <= 0)
        {
            die();
        }
    }

    //Sets the enemy health bar to match the current value
    public void setHealthBar(int newCurrentHealth)
    {
        enemyHealthBar.GetComponent<Slider>().value = newCurrentHealth;
    }

    //When out of health, the enemy grants XP to the player and then destroys itself.
    public void die()
    {
        FindObjectOfType<PlayerController>().gainXP(xp);
        Destroy(gameObject);
    }
}
