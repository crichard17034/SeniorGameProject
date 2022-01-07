using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private static bool gamePaused = false;
    public GameObject pauseMenu;
    public GameObject sword;

    public void Update()
    {
        //checks if the escape key is pressed and pauses or resumes the game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    //the game's time scale is set to 1f, the cursor is hidden and locked to the center of the screen, and the pause menu is hidden so gameplay can resume
    public void resumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        sword.GetComponent<SwordAttack>().pauseGameStatus();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //the game's time scale is set to 0f, effectively freezing gameplay while setting the pause menu on the canvas to visible. The cursor is made visible and unlocked for movement.
    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        sword.GetComponent<SwordAttack>().pauseGameStatus();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
