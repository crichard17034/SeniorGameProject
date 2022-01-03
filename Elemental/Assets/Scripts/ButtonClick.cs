using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public void newGame()
    {
        FindObjectOfType<GameManager>().newGame();
    }

    public void continueGame()
    {
        FindObjectOfType<GameManager>().continueGame();
    }

    public void quitGame()
    {
        FindObjectOfType<GameManager>().quitGame();
    }

    public void titleScreen()
    {
        FindObjectOfType<GameManager>().titleScreen();
    }
}
