using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    int jumpCounter = 0;

    //States
    public float sightRange; //Is the range that the slime can see and look around
    public bool playerInSightRange; //This is what tells if the player is with the sight range.

    private void Awake()
    {
        player = GameObject.Find("First Person Player").transform; //Thiss finds what I am assigning as the player object.
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange) Patroling(); //this is what controles the random movment of the slime while player not in sight
        if (playerInSightRange) ChasePlayer(); //this is what teels the slime if the player is in range to chase the player

    }

    private void Patroling()
    {
            if (!walkPointSet) SearchWalkPoint(); //This calls the SearchWalkpoint method to make the slime move randomly if a set walk point insent there

            if (walkPointSet)
                agent.SetDestination(walkPoint); //This is what sets the walkpoint to the player to get the slime to chase

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 3f)
                walkPointSet = false;

            jumpCounter -= 1;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange); //This is the code that looks for a random point in search range to walk to
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //This is the code that sets the walk point to the random position that was selected

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position); //This sets the destionation to the player when the code is called
        transform.LookAt(player.position); //This has the slime look towards the player.
    }

    private void OnDrawGizmosSelected() //This makes it so that when in you click on the slime you can see the range of its sight
    {
        Gizmos.color = Color.red;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}