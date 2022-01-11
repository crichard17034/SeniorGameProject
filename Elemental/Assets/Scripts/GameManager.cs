using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Scene currentScene;
    private string sceneName;
    public GameObject databaseSave;
    public GameObject player;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        searchForDatabase();
        searchForPlayer();
    }

    void Start()
    {
        sendStatsToPlayer(player);
    }

    //Starts a new game by loading in the Wind Territory and setting starting player stats for the user.
    public void newGame()
    {
        SceneManager.LoadScene(1);
        databaseSave.GetComponent<DatabaseSave>().newGameStart(100, 100, 15, 1, 0, 100, 0, 0, 0);
    }
    
    public void continueGame()
    {
        SceneManager.LoadScene(1);
    }

    public void gameOver()
    {
        SceneManager.LoadScene(3);
        unlockMouse();
    }
    
    public void quitGame()
    {
        Application.Quit();
    }

    public void titleScreen()
    {
        SceneManager.LoadScene(0);
        unlockMouse();
        Time.timeScale = 1f;
    }

    public void winGame()
    {
        SceneManager.LoadScene(2);
        unlockMouse();
    }

    //checks if a game object with the tag "Player" is present and sets the value of the player variable
    private void searchForPlayer()
    {
        if(sceneName == "Wind Territory")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Player Found");
        }
        else
        {
            Debug.Log("Not in WInd Territory Scene");
        }
    }

    //Checks if a game object with the tag "Database" is present within the current scene and sets the value of
    //the databaseSave value accordingly
    private void searchForDatabase()
    {
        if (databaseSave == null)
        {
            databaseSave = GameObject.FindWithTag("Database");
        }
    }

    //calls the database to send the stored values to the player
    private void sendStatsToPlayer(GameObject playerObject)
    {
        if(sceneName == "Wind Territory")
        {
            databaseSave.GetComponent<DatabaseSave>().sendToPlayer(playerObject);
        }
        else
        {
            Debug.Log("Stats not sent to player");
        }  
    }

    //Updates the database with the stats taken from the player.
    public void updateDatabase(int mHP, int cHP, int aTK, int lV, int xP, int goalXP, int fireStone, int waterStone, int windStone)
    {
        databaseSave.GetComponent<DatabaseSave>().updateStats(mHP, cHP, aTK, lV, xP, goalXP, fireStone, waterStone, windStone);
    }

    //Calls the player to send its stats to the Game Manager.
    public void getPlayerStats()
    {
        player.GetComponent<PlayerController>().sendStats();
    }
    
    //allows the cursor to freely move on screen when on menus
    private void unlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
