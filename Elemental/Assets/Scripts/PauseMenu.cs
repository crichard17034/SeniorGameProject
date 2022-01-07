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

    public void resumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        sword.GetComponent<SwordAttack>().pauseGameStatus();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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
