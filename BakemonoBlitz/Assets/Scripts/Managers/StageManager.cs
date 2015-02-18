using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
    Stage Manager
 
    The Stage Manager manages and controls the stage elements such as the static platforms (walls and ground), dynamic platforms (moving ground, obstacles, destructible objects), and Objects that trigger changings in these dynamic platforms.
 
    Responsibilities include:
 
    Loading and creating platform elements and objects
    Handling all entities with platform tag
    Handling events and triggers from objects that trigger dynamic platforms 
*/

public class StageManager : MonoBehaviour
{
    private LinkedList<GameObject> staticPlatforms = new LinkedList<GameObject>();
    private LinkedList<GameObject> dynamicPlatforms = new LinkedList<GameObject>();
    private LinkedList<GameObject> eventPlatforms = new LinkedList<GameObject>();

    private int numStatic;
    private int numDynamic;
    private int numEvent;

    // Polled by Scene Manager as checkpoints or triggers
    // Scene Manager will set variable to false after poll
    // Event Platform collided should delete itself to avoid relapsing triggers
    // Event Platform will set these values to true
    public bool event1;
    public bool event2;
    public bool event3;

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

        // Get reference to all platform types
        GameObject[] staticArray = GameObject.FindGameObjectsWithTag("Static Platform");
        GameObject[] dynamicArray = GameObject.FindGameObjectsWithTag("Dynamic Platform");
        GameObject[] eventArray = GameObject.FindGameObjectsWithTag("Event Platform");

        foreach (GameObject obj in staticArray)
        {
            staticPlatforms.AddLast(obj);
        }

        foreach (GameObject obj in dynamicArray)
        {
            dynamicPlatforms.AddLast(obj);
        }

        foreach (GameObject obj in eventArray)
        {
            eventPlatforms.AddLast(obj);
        }

        numStatic = staticPlatforms.Count;
        numDynamic = dynamicPlatforms.Count;
        numEvent = eventPlatforms.Count;
    }

    // Called by static platforms to remove themselves from list
    public void removeStatic(GameObject obj)
    {
        staticPlatforms.Remove(obj);
        numStatic--;
    }

    // Called by dynamic platforms to remove themselves from list
    public void removeDynamic(GameObject obj)
    {
        dynamicPlatforms.Remove(obj);
        numDynamic--;
    }

    // Called by event platforms to remove themselves from list
    // Will also set the event flag to true
    public void removeEvent(GameObject obj)
    {
        eventPlatforms.Remove(obj);
        numEvent--;
    }





}
