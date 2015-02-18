using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
    Enemy Manager
 
    The Enemy Manager manages all things having to deal with the enemies such as creating and having a handle on all enemy entities and bosses. This manager is initialized by the Scene Manager and can change the player state.
 
    Responsibilities include:
 
    Loading enemies from a file?
    Holding a reference to all entities with an enemy tag
    Handling cases where enemies are destroyed
    Spawning multiple enemies at some location

    Cases where enemy damages player will be handled by the Enemy Controller
*/

public class EnemyManager : MonoBehaviour
{
    private LinkedList<GameObject> enemies = new LinkedList<GameObject>();
    private int numEnemies;

    // Need to be dragged in from Unity Editor
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);

        // Get reference to all enemies
        GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemiesArray)
        {
            enemies.AddLast(enemy);
        }

        // Set number of enemies
        numEnemies = enemies.Count;
    }

    // Spawn an enemy at target location given the enemy index
    // Enemy Index is determined by the prefab enemy number
    public void spawnEnemy(int enemyIndex, Vector3 location)
    {
        switch (enemyIndex)
        {
            case 1: enemies.AddLast(Instantiate(enemyPrefab1, location, Quaternion.identity) as GameObject);
                numEnemies++;
                break;
            case 2: enemies.AddLast(Instantiate(enemyPrefab2, location, Quaternion.identity) as GameObject);
                numEnemies++;
                break;
            case 3: enemies.AddLast(Instantiate(enemyPrefab3, location, Quaternion.identity) as GameObject);
                numEnemies++;
                break;
            default: Debug.Log("Invalid enemy index");
                break;
        }
    }

    // Called by enemy controllers when the enemy is done playing death animation after dieing
    public void removeEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        numEnemies--;
    }

    // Get Number of enemies currently in scene, used to display cleared scene
    public int numberOfEnemies()
    {
        return numEnemies;
    }

}
