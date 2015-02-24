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

    public bool lockPlayerCoordinates = false;
    public bool lockPlayerInput = false;

    public bool LTH, RTH;
    public float LTA, LX, LY;

    GameObject player;

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

        // Lock Player coordinates
        if (lockPlayerCoordinates)
        {
            this.gameObject.transform.position = new Vector3(x,y,z);
        }
        else
        {
            x = player.gameObject.transform.position.x;
            y = player.gameObject.transform.position.y;
            z = player.gameObject.transform.position.z;
        }

        if (lockPlayerInput)
        {
            LTH = false;
            RTH = false;
            LTA = 0.0f;
            LX = 0.0f;
            LY = 0.0f;
        }

	}

    public void A()
    {
        if (lockPlayerInput) return;
    }

    public void B()
    {
        if (lockPlayerInput) return;

    }

    public void X()
    {
        if (lockPlayerInput) return;

    }

    public void Y()
    {
        if (lockPlayerInput) return;

    }

    public void LT()
    {
        if (lockPlayerInput) return;

    }

    public void RT()
    {
        if (lockPlayerInput) return;

    }


    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
        player = GameObject.Find("Player");
    }

}
