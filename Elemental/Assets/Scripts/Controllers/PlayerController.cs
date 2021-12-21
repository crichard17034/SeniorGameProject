using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private float speed = 12; 
    private float sprintSpeed = 20; 
    private float gravity = -29.43f; 
    private float jumpHeight = 4f; 
    private int currentHealth;
    private int maxHealth;
    private int attackStrength;
    private int level;
    private int xp;
    private int xpGoal;
    public string currentElement;
    private string elementWeakness;
    private string elementResistence;
    private bool fireStone;
    private bool waterStone;
    private bool windStone;
    private bool darkStone;
    private bool lightStone;
    public Transform groundCheck; 
    public Transform headCheck; 
    public float groundDistance = 5f; 
    public LayerMask groundMask;
    Vector3 velocity;
    public bool isWalking = false;
    public bool isSprinting = false; 
    public GameObject staminaBar; 
    public GameObject healthBar;
    public GameObject sword;
    public float sprintCooldown; 
    [SerializeField] Footsteps soundGenerator;
    [SerializeField] float footStepTimer;


    void Awake()
    {
        currentElement = "None";
        elementWeakness = "None";
        elementResistence = "None";

    }
    void Update()
    {
        checkForMovement();
        checkForGround();
        checkForCeiling();
        checkForJump();
        checkForSwap();
        checkForStaminaDrain();
    }

    public void checkForMovement()
    {
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        Vector3 move = transform.right * x + transform.forward * z; 

        if(move.x < 0 || move.x > 0 || move.y < 0 || move.y > 0 || move.z < 0 || move.z > 0)
        {

            if (!isWalking && controller.isGrounded && isSprinting)
            {
                footStepTimer = 0.2f;
                PlayFootstep();
            }
            if (!isWalking && controller.isGrounded && !isSprinting)
            {
                footStepTimer = 0.33f;
                PlayFootstep();
            }
        }

        if (Input.GetButton("Sprint") && controller.isGrounded && staminaBar.GetComponent<Slider>().value > 0)
        {
            if (move.x < 0 || move.x > 0 || move.y < 0 || move.y > 0 || move.z < 0 || move.z > 0)
            {
                isSprinting = true;
                speed = sprintSpeed;
                staminaBar.GetComponent<Slider>().value -= Time.deltaTime * 10;
                sprintCooldown = 5;
            }
        }
        else
        {
            isSprinting = false;
            if (speed > 12)
            {
                speed--;
            }

            if (sprintCooldown > 0)
            {
                sprintCooldown -= Time.deltaTime;
            }
        }

        if (staminaBar.GetComponent<Slider>().value < 100 && sprintCooldown <= 0)
        {
            staminaBar.GetComponent<Slider>().value += Time.deltaTime * 5;
        }

        controller.Move(move * speed * Time.deltaTime);
    }

    public void checkForGround()
    {
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime); 

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    public void checkForJump()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            sendStats();
        }
    }

    //checkForCeiling - uses the collsionFlags on the controller to check if a collision occured at the top of the player.

    public void checkForCeiling()
    {
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            velocity.y = 0f;
        }
    }


    //Checks if the player is holding down a button and if the element has been unlocked. If true, it will adjust the element weaknesses.
    //NOTE: THIS SECTION IS TO BE REDONE AS A DICTIONARY TO REDUCE CODE REUSAGE
    public void checkForSwap()
    {
        if(Input.GetButtonDown("FireSwitch") && fireStone == true)
        {
            if(currentElement != "Fire" && staminaBar.GetComponent<Slider>().value > 0)
            {
                currentElement = "Fire";
                elementWeakness = "Water";
                elementResistence = "Wind";
            }
            else
            {
                currentElement = "None";
                elementWeakness = "None";
                elementResistence = "None";
            }
        }
        else if(Input.GetButtonDown("WaterSwitch") && waterStone == true)
        {
            if(currentElement != "Water" && staminaBar.GetComponent<Slider>().value > 0)
            {
                currentElement = "Water";
                elementWeakness = "Wind";
                elementResistence = "Fire";
            }
            else
            {
                currentElement = "None";
                elementWeakness = "None";
                elementResistence = "None";
            }
        }
        else if(Input.GetButtonDown("WindSwitch") && windStone == true)
        {
            if(currentElement != "Wind" && staminaBar.GetComponent<Slider>().value > 0)
            {
                currentElement = "Wind";
                elementWeakness = "Fire";
                elementResistence = "Water";
            }
            else
            {
                currentElement = "None";
                elementWeakness = "None";
                elementResistence = "None";
            }
        }
        else if(Input.GetButtonDown("LightSwitch") && lightStone == true)
        {
            if(currentElement != "Light" && staminaBar.GetComponent<Slider>().value > 0)
            {
                currentElement = "Light";
                elementWeakness = "Dark";
                elementResistence = "Light";
            }
            else
            {
                currentElement = "None";
                elementWeakness = "None";
                elementResistence = "None";
            }
        }
        else 
        {
            if(Input.GetButtonDown("DarkSwitch") && darkStone == true)
            {
                if(currentElement != "Dark" && staminaBar.GetComponent<Slider>().value > 0)
                {
                    currentElement = "Dark";
                    elementWeakness = "Light";
                    elementResistence = "Dark";
                }
                else
                {
                    currentElement = "None";
                    elementWeakness = "None";
                    elementResistence = "None";
                }
            }
        }
        sword.GetComponent<SwordAttack>().changeElement(currentElement);
    }

    private void checkForStaminaDrain()
    {
        if(currentElement != "None")
        {
            staminaBar.GetComponent<Slider>().value -= Time.deltaTime * 10;
        }

        if(staminaBar.GetComponent<Slider>().value == 0)
        {
            currentElement = "None";
            elementWeakness = "None";
            elementResistence = "None";
        }
    }

    public void loseHealth(int damageValue, string element)
    {
        if(element == elementWeakness && currentElement != "None")
        {
            currentHealth -= (damageValue * 2);
        }
        else if(element == elementResistence && currentElement != "None")
        {
            currentHealth -= (damageValue /2);
        }
        else
        {
            currentHealth -= damageValue;
        }
        
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.GetComponent<PlayerHealthManager>().setHealthBar(currentHealth);
    }

    public void gainHealth(int healthValue)
    {
        currentHealth += healthValue;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.GetComponent<PlayerHealthManager>().setHealthBar(currentHealth);
    }

    public void gainXP(int xpValue)
    {
        xp += xpValue;
        checkLevelUp();
    }

    private void checkLevelUp()
    {
        if(xp >= xpGoal)
        {
            levelUp();
        }
    }

    //updates the player stats and related components with new values

    private void levelUp()
    {
        level += 1;
        attackStrength += 5;
        maxHealth += 10;
        currentHealth = maxHealth;
        xpGoal += 100;

        sword.GetComponent<SwordAttack>().updateAttackStr(attackStrength);
        healthBar.GetComponent<PlayerHealthManager>().levelUpHealth(maxHealth);
    }

    //gets the values of each stat from the database and applies them to the corresponding player stats.

    public void setStats(int savedMHP, int savedCHP, int savedATK, int savedLV, int savedXP, int savedGoalXP)
    {
        maxHealth = savedMHP;
        currentHealth = savedCHP;
        attackStrength = savedATK;
        level = savedLV;
        xp = savedXP;
        xpGoal = savedGoalXP;
        sword.GetComponent<SwordAttack>().updateAttackStr(attackStrength);
        healthBar.GetComponent<PlayerHealthManager>().newSceneHealth(maxHealth, currentHealth);
    }

    public void sendStats()
    {
        FindObjectOfType<GameManager>().updateDatabase(maxHealth, currentHealth, attackStrength, level, xp, xpGoal);
    }

    //The name of the gem collected is passed into the collectGem function and either sets the corresponding element to true or grants 500 xp.
    //NOTE: THIS SECTION IS TO BE REDONE AS A DICTIONARY TO REDUCE CODE REUSAGE

    public void collectGem(string gemName)
    {
        if(gemName  == "fire")
        {
            if(fireStone == false)
            {
                fireStone = true;            
                Debug.Log("The fire element has been added.");
            }
            else
            {
                gainXP(500);
            }
        }
        else if(gemName == "water")
        {
            if(waterStone == false)
            {
                waterStone = true;            
                Debug.Log("The water element has been added.");
            }
            else
            {
                gainXP(500);
            }
        }
        else if(gemName == "wind")
        {
            if(windStone == false)
            {
                windStone = true;            
                Debug.Log("The wind element has been added.");
            }
            else
            {
                gainXP(500);
            }
        }
        else
        {
            if(lightStone == false && darkStone == false)
            {
                lightStone = true;
                darkStone = true;            
                Debug.Log("The light and dark elements have been added.");
            }
            else
            {
                gainXP(500);
            }
        }
    }

    public void PlayFootstep()
    {
        StartCoroutine("PlayStep", footStepTimer);
    }

    IEnumerator PlayStep(float timer)
    {
        var randomIndex = Random.Range(0, 3);
        soundGenerator.audioSource.clip = soundGenerator.footStepSounds[randomIndex];

        soundGenerator.audioSource.Play();
        isWalking = true;

        yield return new WaitForSeconds(timer);

        isWalking = false;
    }
}
