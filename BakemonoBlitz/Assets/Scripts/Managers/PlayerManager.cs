using UnityEngine;
using System.Collections;

/*
    The Player Manager manages all things having to deal with the player such as allowable movements and actions, items the player is holding, player statistics, and player state. This manager is initialized by the Scene Manager.
 
    Responsibilities include:
 
    Loading the player state and information from the previous scene
    Passing on the current player state and information to the next scene4
    Initializing the Player Controller
    Invoking methods provided in the Player Controller depending on the current state and the input provided by the user
    Holding the player state (player state can be altered by other managers)
*/

public class PlayerManager : MonoBehaviour
{
    public string gender;
    public int health, energy, lives, gems, score, time;
    public bool weapon1, weapon2, weapon3;
    public float x, y, z;

    private float startTime;
    private float currentTime;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Update time
        currentTime = Time.time;
        time = (int)(currentTime - startTime);



	}

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
    }

}
