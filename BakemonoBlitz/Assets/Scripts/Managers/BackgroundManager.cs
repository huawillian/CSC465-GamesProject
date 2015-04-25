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
    public GameObject[] horizon;

    private float zPos = 10.0f;

    SceneManager mSceneManager;

    float playerOrigPosx = 0.0f;

    float[] horizonOrigPosx;
    float[] horizonOrigPosy;
    public bool horSet = false;
    GameObject player;

    public GameObject clouds; 

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        player = GameObject.Find("Player");
        Debug.Log("Initializing " + this.gameObject.name);
        images = GameObject.FindGameObjectsWithTag("Background");
        horizon = GameObject.FindGameObjectsWithTag("Horizon");
        horizonOrigPosx = new float[horizon.Length];
        horizonOrigPosy = new float[horizon.Length];

        for (int i = 0; i < horizon.Length; i++)
        {

            horizonOrigPosx[i] = horizon[i].transform.position.x;
            horizonOrigPosy[i] = horizon[i].transform.position.y;
        }

        horSet = true;

        StartCoroutine("playClouds");

	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move Background Based on Player position and z axis...
        for (int i = 0; i < horizon.Length; i++)
        {
            if (horSet)
            {
                horizon[i].transform.position = new Vector3((GameObject.FindGameObjectWithTag("MainCamera").transform.position.x - playerOrigPosx) * 0.4f + horizonOrigPosx[i], horizon[i].transform.position.y, horizon[i].transform.position.z);
            }
        }
	}

    IEnumerator playClouds()
    {
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            Instantiate(clouds, new Vector3(mSceneManager.mPlayerManager.x + 5.0f, horizonOrigPosy[0] + Random.Range(1, 5), 0.0f ), Quaternion.identity);
            yield return new WaitForSeconds(2.0f);
        }

    }



    // Initialization called by Scene Manager
    public void InitializeManager()
    {

    }
}
