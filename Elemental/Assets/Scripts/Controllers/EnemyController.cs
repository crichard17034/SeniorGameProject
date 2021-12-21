using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    public int currentHealth;
    public int maxHealth;
    public int xp;
    public string element;
    NavMeshAgent agent;
    Transform target;
    Vector3 velocity;
    public float lookRadius = 10f;
    public float attackRange = 7f;
    public float attackTimer = 120f;
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

    public void checkLookRadius()
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

    public void checkAttackRadius()
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

    public void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //When moving, the object's root position is set to move with the animation, preventing the enemy from getting soft locked in place.
    public void OnAnimatorMove()
    {
        Vector3 position = anim.rootPosition;
        transform.position = position;
    } 

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void setElement()
    {
        gameObject.GetComponent<DamagePlayer>().setElementDamage(element);
    }

    public void loseHealth(int damageValue)
    {
        currentHealth -= damageValue;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            die();
        }
    }

    public void die()
    {
        FindObjectOfType<PlayerController>().gainXP(xp);
        Destroy(gameObject);
    }
}
