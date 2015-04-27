using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour
{
    SceneManager mSceneManager;
    GameObject player;

    public Sprite Running;

    GameObject shadow1;
    GameObject shadow2;
    GameObject shadow3;
    GameObject shadow4;

    PlayerManager.PlayerState[] playerStates;
    Vector3[] playerCoordinates;
    bool[] playerDirections;

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        player = mSceneManager.mPlayerManager.player;

        playerStates = new PlayerManager.PlayerState[4];
        playerCoordinates = new Vector3[4];
        playerDirections = new bool[4];

        // Get the physical collider and sync it with physical variable
        Transform[] ts = GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "Shadow1")
            {
                shadow1 = t.gameObject;
            }

            if (t.gameObject.name == "Shadow2")
            {
                shadow2 = t.gameObject;
            }

            if (t.gameObject.name == "Shadow3")
            {
                shadow3 = t.gameObject;
            }

            if (t.gameObject.name == "Shadow4")
            {
                shadow4 = t.gameObject;
            }
        }

        StartCoroutine("RecordStart");
	}

    IEnumerator RecordStart()
    {
        while (true)
        {
            playerCoordinates[3] = playerCoordinates[2];
            playerCoordinates[2] = playerCoordinates[1];
            playerCoordinates[1] = playerCoordinates[0];
            playerCoordinates[0] = player.gameObject.transform.position;

            playerStates[3] = playerStates[2];
            playerStates[2] = playerStates[1];
            playerStates[1] = playerStates[0];
            playerStates[0] = mSceneManager.mPlayerManager.state;

            playerDirections[3] = playerDirections[2];
            playerDirections[2] = playerDirections[1];
            playerDirections[1] = playerDirections[0];
            playerDirections[0] = player.GetComponent<PlayerController>().FaceRight;

            yield return new WaitForSeconds(0.05f);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (mSceneManager.mPlayerManager.wasSpeeding && Mathf.Abs(Mathf.Abs(player.rigidbody2D.velocity.x) - mSceneManager.mPlayerManager.maxSpeed) < 1.0f)
        {
            shadow1.transform.position = playerCoordinates[0];
            shadow2.transform.position = playerCoordinates[1];
            shadow3.transform.position = playerCoordinates[2];
            shadow4.transform.position = playerCoordinates[3];

            shadow1.GetComponent<SpriteRenderer>().sprite = Running;
            shadow2.GetComponent<SpriteRenderer>().sprite = Running;
            shadow3.GetComponent<SpriteRenderer>().sprite = Running;
            shadow4.GetComponent<SpriteRenderer>().sprite = Running;

            shadow1.GetComponent<SpriteRenderer>().renderer.material.color = new Color(shadow1.GetComponent<SpriteRenderer>().renderer.material.color.a, shadow1.GetComponent<SpriteRenderer>().renderer.material.color.g, shadow1.GetComponent<SpriteRenderer>().renderer.material.color.b, 0.75f);
            shadow2.GetComponent<SpriteRenderer>().renderer.material.color = new Color(shadow2.GetComponent<SpriteRenderer>().renderer.material.color.a, shadow2.GetComponent<SpriteRenderer>().renderer.material.color.g, shadow2.GetComponent<SpriteRenderer>().renderer.material.color.b, 0.5f);
            shadow3.GetComponent<SpriteRenderer>().renderer.material.color = new Color(shadow3.GetComponent<SpriteRenderer>().renderer.material.color.a, shadow3.GetComponent<SpriteRenderer>().renderer.material.color.g, shadow3.GetComponent<SpriteRenderer>().renderer.material.color.b, 0.25f);
            shadow4.GetComponent<SpriteRenderer>().renderer.material.color = new Color(shadow4.GetComponent<SpriteRenderer>().renderer.material.color.a, shadow4.GetComponent<SpriteRenderer>().renderer.material.color.g, shadow4.GetComponent<SpriteRenderer>().renderer.material.color.b, 0.1f);

            if (playerDirections[0])
            {
                shadow1.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else shadow1.transform.rotation = new Quaternion(0, 180, 0, 0);

            if (playerDirections[1])
            {
                shadow2.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else shadow2.transform.rotation = new Quaternion(0, 180, 0, 0);

            if (playerDirections[2])
            {
                shadow3.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else shadow3.transform.rotation = new Quaternion(0, 180, 0, 0);

            if (playerDirections[3])
            {
                shadow4.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else shadow4.transform.rotation = new Quaternion(0, 180, 0, 0);


            shadow1.GetComponent<SpriteRenderer>().enabled = true;
            shadow2.GetComponent<SpriteRenderer>().enabled = true;
            shadow3.GetComponent<SpriteRenderer>().enabled = true;
            shadow4.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            shadow1.GetComponent<SpriteRenderer>().enabled = false;
            shadow2.GetComponent<SpriteRenderer>().enabled = false;
            shadow3.GetComponent<SpriteRenderer>().enabled = false;
            shadow4.GetComponent<SpriteRenderer>().enabled = false;
        }
	}
}
