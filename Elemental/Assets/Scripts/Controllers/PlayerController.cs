using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private float speed = 15; 
    private float sprintSpeed = 30; 
    private float gravity = -29.43f; 
    private float jumpHeight = 5f; 
    private float invincibility = 0f; 
    private int currentHealth;
    private int maxHealth;
    private int attackStrength;
    private int level;
    private int xp;
    private int xpGoal;
    private string currentElement;
    private string elementWeakness;
    private string elementResistance;
    private bool fireStone;
    private bool waterStone;
    private bool windStone;
    public Transform groundCheck; 
    public Transform headCheck; 
    public float groundDistance = 5f; 
    public LayerMask groundMask;
    Vector3 velocity;
    private bool isWalking = false;
    private bool isSprinting = false; 
    public GameObject staminaBar; 
    public GameObject healthBar;
    public GameObject levelText;
    public GameObject expText;
    public GameObject atkText;
    public GameObject sword;
    public float sprintCooldown; 
    [SerializeField] Footsteps soundGenerator;
    [SerializeField] ElementSounds elementGenerator;
    [SerializeField] float footStepTimer;
    public AudioSource ouchSound;
    public AudioSource levelUpSound;


    void Awake()
    {
        currentElement = "None";
        elementWeakness = "None";
        elementResistance = "None";
    }

    void Update()
    {
        checkForMovement();
        checkForGround();
        checkForCeiling();
        checkForJump();
        checkForSwap();
        checkForStaminaDrain();
        checkInvincibility();
    }

    //Checks if the player is pressing a movement button and transforms the player position.
    public void checkForMovement()
    {
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        Vector3 move = transform.right * x + transform.forward * z; 

        if(move.x < 0 || move.x > 0 || move.y < 0 || move.y > 0 || move.z < 0 || move.z > 0)
        {
            checkForSprint();

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
        controller.Move(move * speed * Time.deltaTime);
    }

    //Checks if the player is holding shift and the stamina bar is greater than 0. If true, sprinting will occur.
    private void checkForSprint()
    {
        if (Input.GetButton("Sprint") && controller.isGrounded && staminaBar.GetComponent<Slider>().value > 0)
        {
            isSprinting = true;
            speed = sprintSpeed;
            staminaBar.GetComponent<Slider>().value -= Time.deltaTime * 10;
            sprintCooldown = 5;
        }
        else
        {
            isSprinting = false;
            if (speed > 15)
            {
                speed--;
            }

            if (sprintCooldown > 0)
            {
                sprintCooldown -= Time.deltaTime;
            }
        }
    }

    //Checks if the player is currently touching solid ground
    private void checkForGround()
    {
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime); 

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void checkForJump()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    //checkForCeiling - uses the collsionFlags on the controller to check if a collision occured at the top of the player.

    private void checkForCeiling()
    {
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            velocity.y = 0f;
        }
    }


    //Checks if the player is holding down a button and if the element has been unlocked. If true, it will adjust the element weaknesses.
    private void checkForSwap()
    {
        if(Input.GetButtonDown("FireSwitch") && fireStone == true)
        {
            if(currentElement != "Fire" && staminaBar.GetComponent<Slider>().value > 0)
            {
                currentElement = "Fire";
                elementWeakness = "Water";
                elementResistance = "Wind";
                playElementSound(0);
            }
            else
            {
                currentElement = "None";
                elementWeakness = "None";
                elementResistance = "None";
            }
        }
        else if(Input.GetButtonDown("WaterSwitch") && waterStone == true)
        {
            if(currentElement != "Water" && staminaBar.GetComponent<Slider>().value > 0)
            {
                currentElement = "Water";
                elementWeakness = "Wind";
                elementResistance = "Fire";
                playElementSound(1);
            }
            else
            {
                currentElement = "None";
                elementWeakness = "None";
                elementResistance = "None";
            }
        }
        else if(Input.GetButtonDown("WindSwitch") && windStone == true)
        {
            if(currentElement != "Wind" && staminaBar.GetComponent<Slider>().value > 0)
            {
                currentElement = "Wind";
                elementWeakness = "Fire";
                elementResistance = "Water";
                playElementSound(2);
            }
            else
            {
                currentElement = "None";
                elementWeakness = "None";
                elementResistance = "None";
            }
        }
        sword.GetComponent<SwordAttack>().changeElement(currentElement);
    }

    //Checks if element usage or sprinting is draining stamina. Otherwise, if the sprint cooldown is <= 0, regain stamina.
    private void checkForStaminaDrain()
    {
        if (sprintCooldown > 0)
        {
            sprintCooldown -= Time.deltaTime;
        }

        if (staminaBar.GetComponent<Slider>().value < 100 && sprintCooldown <= 0)
        {
            staminaBar.GetComponent<Slider>().value += Time.deltaTime * 7;
        }        
        
        if(currentElement != "None")
        {
            staminaBar.GetComponent<Slider>().value -= Time.deltaTime * 10;
        }

        if(staminaBar.GetComponent<Slider>().value == 0)
        {
            currentElement = "None";
            elementWeakness = "None";
            elementResistance = "None";
        }
    }

    //Causes the player to lose health based on the element of the damage source and the damage value.
    public void loseHealth(int damageValue, string element)
    {
        if(invincibility <= 0f)
        {
            if(element == elementWeakness && currentElement != "None")
            {
                currentHealth -= (damageValue * 2);
            }
            else if(element == elementResistance && currentElement != "None")
            {
                currentHealth -= (damageValue /2);
            }
            else
            {
                currentHealth -= damageValue;
            }
        
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                FindObjectOfType<GameManager>().gameOver();
            } 
            healthBar.GetComponent<PlayerHealthManager>().setHealthBar(currentHealth);
            invincibility += 120f;
            ouchSound.Play();
        }
    }

    //recovers player health while also keeping it from going over the maxHealth value
    public void gainHealth(int healthValue)
    {
        currentHealth += healthValue;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        } 

        healthBar.GetComponent<PlayerHealthManager>().setHealthBar(currentHealth);    
    }

    //Grants the player XP and calls to check for a level increase.
    public void gainXP(int xpValue)
    {
        if(level != 20)
        {
            xp += xpValue;
            expText.GetComponent<TextMeshProUGUI>().text = "" + xp + " / " +  xpGoal + "XP";
            checkLevelUp();
        }
    }

    //Checks if the player's current XP is greater than or equal to the goal and levels up the player.
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
        attackStrength += 10;
        maxHealth += 10;
        currentHealth = maxHealth;
        xpGoal += 250;

        sword.GetComponent<SwordAttack>().updateAttackStr(attackStrength);
        healthBar.GetComponent<PlayerHealthManager>().levelUpHealth(maxHealth);
        levelText.GetComponent<TextMeshProUGUI>().text = "LV: " + level;
        atkText.GetComponent<TextMeshProUGUI>().text = "ATK: " + attackStrength;
        
        if(level == 20)
        {
            expText.GetComponent<TextMeshProUGUI>().text = "MAX LVL";
        }
        else
        {
            expText.GetComponent<TextMeshProUGUI>().text = "" + xp + " / " +  xpGoal + "XP";
        }
        levelUpSound.Play();
    }

    //Sends current stats to the GameManager
    public void sendStats()
    {
        int fireStoneInt;
        int waterStoneInt;
        int windStoneInt;

        if(fireStone == false)
        {
            fireStoneInt = 0;
        }
        else
        {
            fireStoneInt = 1;
        }
        if(waterStone == false)
        {
            waterStoneInt = 0;
        }
        else
        {
            waterStoneInt = 1;
        }
        if(windStone == false)
        {
            windStoneInt = 0;
        }
        else
        {
            windStoneInt = 1;
        }

        FindObjectOfType<GameManager>().updateDatabase(maxHealth, currentHealth, attackStrength, level, xp, xpGoal, fireStoneInt, waterStoneInt, windStoneInt);
    }

    //gets the values of each stat from the database and applies them to the corresponding player stats.
    public void setStats(int savedMHP, int savedCHP, int savedATK, int savedLV, int savedXP, int savedGoalXP, int savedFireStone, int savedWaterStone, int savedWindStone)
    {
        maxHealth = savedMHP;
        currentHealth = savedCHP;
        attackStrength = savedATK;
        level = savedLV;
        xp = savedXP;
        xpGoal = savedGoalXP;

        if(savedFireStone == 0)
        {
            fireStone = false;
        }
        else
        {
            fireStone = true;
        }
        if(savedWaterStone == 0)
        {
            waterStone = false;
        }
        else
        {
            waterStone = true;
        }
        if(savedWindStone == 0)
        {
            windStone = false;
        }
        else
        {
            windStone = true;
        }

        sword.GetComponent<SwordAttack>().updateAttackStr(attackStrength);
        healthBar.GetComponent<PlayerHealthManager>().setHealthBar(currentHealth);
        levelText.GetComponent<TextMeshProUGUI>().text = "LV: " + level;
        atkText.GetComponent<TextMeshProUGUI>().text = "ATK: " + attackStrength;

        if(level == 20)
        {
            expText.GetComponent<TextMeshProUGUI>().text = "MAX LVL";
        }
        else
        {
            expText.GetComponent<TextMeshProUGUI>().text = "" + xp + " / " +  xpGoal + "XP";
        }

        Debug.Log("Stats Recieved");
    }

    //The name of the gem collected is passed into the collectGem function and either sets the corresponding element to true or grants 500 xp.

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
        else
        {
            if (gemName == "wind")
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
        }
    }

    //Checks if the player has active I-Frames (Invincibiltiy)
    private void checkInvincibility()
    {
        if(invincibility > 0f)
        {
            invincibility --;
        }
    }

    private void PlayFootstep()
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

    private void playElementSound(int entryNumber)
    {
        elementGenerator.audioSource.clip = elementGenerator.elementSoundsList[entryNumber];

        elementGenerator.audioSource.Play();
    }
}
