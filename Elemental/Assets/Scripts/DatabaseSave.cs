using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class DatabaseSave : MonoBehaviour
{
    private string dbName;
    private string dbPath; 

    void Start()
    {   
        dbName = "URI=file:" + Application.persistentDataPath + "/PlayerStats.db";
        createDB();
        Debug.Log("dataPath" + dbName);
    }
    
    //creates a table to store player stats if no table containing the data can be located
    public void createDB()
    {
        using (IDbConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS player (maxHealth INT, currentHealth INT, " +
                    "attackStrength INT, level INT, xp INT, xpGoal INT, fireStone INT, waterStone INT, windStone INT);";
                command.ExecuteNonQuery();
            }
            
            connection.Close();
        }
    }

    //When starting a new game, any current entries in the player table are removed and the starting stats are inserted in their place
    public void newGameStart(int maxHealth, int currentHealth, int attackStrength, int level, int xp, int xpGoal, int fireStone, int waterStone, int windStone)
    {
        using(IDbConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM player; INSERT INTO player (maxHealth, currentHealth, " +
                    "attackStrength, level, xp, xpGoal, fireStone, waterStone, windStone)" + 
                    "VALUES('"+ maxHealth +"', '"+ currentHealth +"', '"+ attackStrength +"', '"+ level +"', '"+ xp +"', '"+ xpGoal +"', '"+ fireStone +"', '"+ waterStone +"', '"+ windStone +"');";
                command.ExecuteNonQuery();
            }
            
            connection.Close();
        }
    }

    //takes in the current stats of the player and updates the values within the table
    public void updateStats(int maxHP, int currentHP, int attackSTR, int lv, int exp, int expGoal, int fireStone, int waterStone, int windStone)
    {
        using(IDbConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE player SET maxHealth = '"+ maxHP +"', currentHealth = '" + currentHP +"'," +
                "attackStrength = '" + attackSTR + "', level = '" + lv + "', xp = '" + exp + "', xpGoal = '" + expGoal + "',"
                + "fireStone = '" + fireStone + "', waterStone = '" + waterStone + "', windStone = '" + windStone + "';";
                command.ExecuteNonQuery();
            }
            
            connection.Close();
        }
    }

    //opens a connection and IDataReader for the player table and reads off each value to the player's controller to update stats.
    public void sendToPlayer(GameObject playerObject)
    {
        using(IDbConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM player;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playerObject.GetComponent<PlayerController>().setStats(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), 
                        + reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8));
                    }

                    reader.Close();
                }
            }
            connection.Close();
            Debug.Log("Stats Sent to Player");
        }
    }

    //reads off the values of player stats in a debug log. Mainly used for testing purposes
    public void viewStats()
    {
        using(IDbConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM player;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log("Max Health: " + reader["maxHealth"] + " Current Health: " + reader["currentHealth"] + " Attack Strength: " 
                            + reader["attackStrength"] + " Level: " + reader["level"] + " XP: " + reader["xp"] + " Xp to Next Level: " + reader["xpGoal"]);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }
    
}
