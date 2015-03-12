using UnityEngine;
using System.Collections;
using System;

public class ChainController : MonoBehaviour
{
    SceneManager mSceneManager;
    public GameObject chain;
    public float chainSize = 0.5f;
    int numChains = 0;
    GameObject[] chains;

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        chains = new GameObject[100];

        for (int i = 0; i < 100; i++)
        {
            chains[i] = (GameObject)Instantiate(chain);
            chains[i].transform.parent = this.gameObject.transform;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (mSceneManager.mPlayerManager.grappleState != PlayerManager.GrappleState.GrappleReady)
        {
            if (mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleRetracting)
            {
                for (int i = 0; i < 100; i++)
                {
                    chains[i].gameObject.renderer.enabled = false;
                }
            }

            // Calculate number of chains
            Vector3 displacement = mSceneManager.mPlayerManager.grappleController.gameObject.transform.position - mSceneManager.mPlayerManager.player.transform.position;
            float distance = (float)Math.Sqrt((double)(displacement.x * displacement.x + displacement.y * displacement.y));

            numChains = (int)(distance / chainSize);

            // Calculate displacement x,y for each chain
            float disX = displacement.x / numChains;
            float disY = displacement.y / numChains;

            // Create New Chain Objects and set Chain location
            float x = 0;
            float y = 0;

            for (int i = 0; i < numChains; i++)
            {
                chains[i].gameObject.transform.position = new Vector3(x + mSceneManager.mPlayerManager.player.transform.position.x, y + mSceneManager.mPlayerManager.player.transform.position.y, 0.0f);
                chains[i].gameObject.renderer.enabled = true;
                x += disX;
                y += disY;
            }
        }
        else
        {
            for (int i = 0; i < 100; i++)
            {
                chains[i].gameObject.renderer.enabled = false;
            }
        }
	}

}
