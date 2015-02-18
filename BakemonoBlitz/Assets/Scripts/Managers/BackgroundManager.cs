using UnityEngine;
using System.Collections;

/*
    Background Manager
 
    The Background Manager controls the display that the player cannot interact with. The background or backgrounds should move depending on the player and the distance from the stage’s z-axis. Backgrounds can be a picture or a dynamic renderer. The background Manager is initialized by the Scene Manager.
 
    Responsibilities include:
 
    Displaying backgrounds depending on the player and distance
    Possibly displaying things in front of the stage
*/

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] images;
    private float zPos = 10.0f;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move Background Based on Player position and z axis...
        foreach (GameObject image in images)
        {
            //image.transform.position = new Vector3(image.transform.position.x + 0.005f, image.transform.position.y, zPos);
        }
	}

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
        images = GameObject.FindGameObjectsWithTag("Background");
    }
}
