using UnityEngine;
using System.Collections;

/*
    Stage Manager
 
    The Stage Manager manages and controls the stage elements such as the static platforms (walls and ground), dynamic platforms (moving ground, obstacles, destructible objects), and Objects that trigger changings in these dynamic platforms.
 
    Responsibilities include:
 
    Loading and creating platform elements and objects
    Handling all entities with platform tag
    Handling events and triggers from objects that trigger dynamic platforms
    Changing Player Manager’s state depending on their interactions with the platform objects
 
*/

public class StageManager : MonoBehaviour {

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
