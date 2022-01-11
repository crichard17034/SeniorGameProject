using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public AudioSource buttonSound;
    
    private void playSound()
    {
        buttonSound.time = .2f;
        buttonSound.Play();
    }

    public void newGame()
    {
        playSound();
        FindObjectOfType<GameManager>().newGame();
    }

    public void continueGame()
    {
        playSound();
        FindObjectOfType<GameManager>().continueGame();
    }

    public void quitGame()
    {
        playSound();
        FindObjectOfType<GameManager>().quitGame();
    }

    public void titleScreen()
    {
        playSound();
        FindObjectOfType<GameManager>().titleScreen();
    }

    public void saveAndTitleScreen()
    {
        playSound();
        FindObjectOfType<GameManager>().getPlayerStats();
        FindObjectOfType<GameManager>().titleScreen();
    }
}
