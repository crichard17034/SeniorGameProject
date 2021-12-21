using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthManager : MonoBehaviour
{
    private int currentHealth;
    private int maxHealth;
    public GameObject healthText;
    public GameObject healthImage;
    public Texture fullHealthIMG;
    public Texture halfHealthIMG;
    public Texture quarterHealthIMG;


    public void setHealthBar(int newHealth)
    {
        currentHealth = newHealth;
        healthText.GetComponent<TextMeshProUGUI>().text = "" + currentHealth;
        setHealthIMG(currentHealth);
    }

    public void setHealthIMG(int healthValue)
    {
        int halfHealth = maxHealth/2;
        int quarterHealth = maxHealth/4;

        if(healthValue > halfHealth)
        {
            healthImage.GetComponent<RawImage>().texture = fullHealthIMG;
        }
        else if(healthValue <= halfHealth && healthValue > quarterHealth)
        {
            healthImage.GetComponent<RawImage>().texture = halfHealthIMG;
        }
        else
        {
            healthImage.GetComponent<RawImage>().texture = quarterHealthIMG;
        }
    }

    public void newSceneHealth(int newMaxHealth, int newCurrentHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = newCurrentHealth;
        setHealthBar(currentHealth);
    }

    public void levelUpHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        setHealthBar(currentHealth);
    }
}
