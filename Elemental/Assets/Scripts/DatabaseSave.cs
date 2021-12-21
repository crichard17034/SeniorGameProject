using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class DatabaseSave : MonoBehaviour
{
    private string dbName = "URI=file:PlayerStats.db";

    void Start()
    {
        createDB();
        updateStats(100, 100, 15, 1, 0, 30);
    }

    void Awake()
    {
        viewStats();
    }

    //creates a table to store player stats if no table containing the data can be located

    public void createDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS player (maxHealth INT, currentHealth INT, " +
                    "attackStrength INT, level INT, xp INT, xpGoal INT);";
                command.ExecuteNonQuery();
            }
            
            connection.Close();
        }
    }

    //When starting a new game, any current entries in the player table are removed and the starting stats are inserted in their place

    public void newGame(int maxHealth, int currentHealth, int attackStrength, int level, int xp, int xpGoal)
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM player; INSERT INTO player (maxHealth, currentHealth, " +
                    "attackStrength, level, xp, xpGoal) VALUES'"+ maxHealth +"', '"+ currentHealth +"', '"+ attackStrength +"', '"+ level +"', '"+ xp +"', '"+ xpGoal+"';";
                command.ExecuteNonQuery();
            }
            
            connection.Close();
        }
    }

    //takes in the current stats of the player and updates the values within the table

    public void updateStats(int maxHP, int currentHP, int attackSTR, int lv, int exp, int expGoal)
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE player SET maxHealth = '"+ maxHP +"', currentHealth = '" + currentHP +"'," +
                "attackStrength = '" + attackSTR + "', level = '" + lv + "', xp = '" + exp + "', xpGoal = '" + expGoal + "';";
                command.ExecuteNonQuery();
            }
            
            connection.Close();
        }
    }

    //opens a connection and IDataReader for the player table and reads off each value to the player's controller to update stats.

    public void sendToPlayer(GameObject playerObject)
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM player;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playerObject.GetComponent<PlayerController>().setStats(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
                    }

                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    //reads off the values of player stats in a debug log. Mainly used for testing purposes

    public void viewStats()
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            
            using(var command = connection.CreateCommand())
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
