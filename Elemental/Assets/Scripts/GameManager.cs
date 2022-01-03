using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance; 
    private Scene currentScene;
    private string sceneName;
    public GameObject databaseSave;
    public GameObject player;

    void Start()
    {
        searchForDatabase();
        searchForPlayer();
    }

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        searchForDatabase();
        searchForPlayer();
    }

    void OnSceneLoaded()
    {
        checkForOneManager();
        searchForPlayer();
    }

    private void checkForOneManager()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void newGame()
    {
        SceneManager.LoadScene(1);
        databaseSave.GetComponent<DatabaseSave>().newGameStart(100, 100, 15, 1, 0, 100);
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
    }

    public void winGame()
    {
        SceneManager.LoadScene(2);
        unlockMouse();
    }

    //checks if a game object with the tag "Player" is present and sets the value of the player variable
    public void searchForPlayer()
    {
        if(sceneName == "Wind Territory")
        {
            player = GameObject.FindWithTag("Player");
            sendStatsToPlayer(player);
        }
    }

    //checks if a game object with the tag "Database" is present within the current scene and sets the value of
    //the databaseSave value accordingly

    public void searchForDatabase()
    {
        if (databaseSave == null)
        {
            databaseSave = GameObject.FindWithTag("Database");
        }
    }

    public void sendStatsToPlayer(GameObject player)
    {
        databaseSave.GetComponent<DatabaseSave>().sendToPlayer(player);
    }

    public void updateDatabase(int mHP, int cHP, int aTK, int lV, int xP, int goalXP)
    {
        databaseSave.GetComponent<DatabaseSave>().updateStats(mHP, cHP, aTK, lV, xP, goalXP);
    }

    public void getPlayerStats()
    {
        player.GetComponent<PlayerController>().sendStats();
    }
    
    //allows the cursor to freely move on screen when on menus
    public void unlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
