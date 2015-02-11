using UnityEngine;
using System.Collections;

/*
    Enemy Manager
 
    The Enemy Manager manages all things having to deal with the enemies such as creating and having a handle on all enemy entities and bosses. This manager is initialized by the Scene Manager and can change the player state.
 
    Responsibilities include:
 
    Loading enemies from a file
    Holding a reference to all entities with an enemy tag
    Manipulating the Enemy Controllers depending on their interactions with the player
    Creating more enemies via events or triggers
    Handling cases where enemies are destroyed
    Handling cases where enemies damage the player
*/

public class EnemyManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
    }
}
