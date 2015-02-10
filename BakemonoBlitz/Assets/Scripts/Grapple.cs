using UnityEngine;
using System.Collections;
using System;

// Name: Willian Hua
// Date: 12/4/14
// Project: Attack on Ninja, Team Koga
// Description: Grapple and swinging mechanics for the player
// Usage: Attatched to Grapple prefab as a component.

public class Grapple : MonoBehaviour
{
    public bool hasCollided; // Flag setting if the Grapple has collided with another object of interest
    public bool isMovingRight; // Set direction of grappling moving motion
    public float speed; // Speed at which grappling moves out
    public GameObject player; // Get reference to player
    public GameObject chain;
    GameObject[] chains;

    public Transform grapple;
    public float timer = 0.0f;
    public float translationTime = 0.85f;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 bending = Vector3.down;
    public float timeStamp = Time.time;
    Vector3 playerPos;
    Vector3 grapplePos;
    Vector3 posDiff;

    // Time to live variable
    public float time;
    public float ttl;
    public int increSpeed = 0;
    public float radius;

    // Reeling in variables
    bool reelingIn = false;
    bool retractIn = false;
    public static Grapple i;

     void Awake () {
         if(!i) {
             i = this;
             DontDestroyOnLoad(gameObject);
         }else 
         Destroy(gameObject);
     }
 

	// Use this for initialization
	void Start ()
    {
        hasCollided = false;
        speed = 0.4f;
        player = GameObject.Find("Player");
        isMovingRight = player.GetComponentInChildren<PlayerController>().isFacingRight;

        // Set Swinging motion variables
        grapple = this.transform;

        // Set player is chaining
        player.GetComponentInChildren<PlayerController>().Chaining = true;

        time = Time.time;
        ttl = 0.2f;

        chains = new GameObject[10];

        for (int i = 0; i < chains.Length; i++)
        {
            chains[i] = Instantiate(chain) as GameObject;
            chains[i].transform.parent = this.transform.parent;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time > ttl && !hasCollided)
        {
            reelingIn = true;
        }

	}

    // This is called every update frame
    void FixedUpdate()
    {
        for (int i = 0; i < chains.Length; i++)
        {
            Vector3 diff = player.transform.position - this.transform.position;
            chains[i].transform.position = new Vector3(diff.x * i / chains.Length + this.transform.position.x, diff.y * i / chains.Length + this.transform.position.y, 0);
        }

        if (hasCollided == false && !reelingIn)
        {

            if (isMovingRight)
                this.transform.position = new Vector3(speed * increSpeed + player.transform.position.x, speed * increSpeed + player.transform.position.y, 0);
            else
                this.transform.position = new Vector3(-speed * increSpeed + player.transform.position.x, speed * increSpeed + player.transform.position.y, 0);

            increSpeed++;
        }

        if (reelingIn)
        {
            ReelIn();
        }

        if (retractIn)
        {
            RetractIn();

            if (!player.GetComponentInChildren<PlayerController>().hasCollided)
            {
                retractIn = false;
                player.GetComponent<Rigidbody>().isKinematic = false;
                player.rigidbody.AddForce(0, 300, 0f);
                reelingIn = true;
            }
        }

        if (player.GetComponentInChildren<PlayerController>().isSwinging == true)
        {

            if ((Time.time < timeStamp + translationTime) && !(player.GetComponentInChildren<PlayerController>().hasCollided))
            {
                float currX;
                float currY;
                if (isMovingRight)
                {
                    currX = Mathf.Cos(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + grapple.transform.position.x;
                    currY = Mathf.Sin(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + grapple.transform.position.y;
                }
                else
                {
                    currX = Mathf.Cos(7 * Mathf.PI / 4 - (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + grapple.transform.position.x;
                    currY = Mathf.Sin(7 * Mathf.PI / 4 - (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + grapple.transform.position.y;
                }

                player.transform.position = new Vector3(currX, currY, 0);

            }
            else
            {
                if (player.GetComponentInChildren<PlayerController>().isGrounded)
                {
                    player.GetComponentInChildren<PlayerController>().isSwinging = false;
                    player.rigidbody.AddForce(posDiff.x * 200, 0, 0f);
                    reelingIn = true;
                }
                else
                {
                    if (player.GetComponentInChildren<PlayerController>().hasCollided)
                    {
                        player.GetComponentInChildren<PlayerController>().isSwinging = false;
                        player.rigidbody.velocity = Vector3.zero;
                        retractIn = true;
                    }
                    else
                    {
                        player.GetComponentInChildren<PlayerController>().isSwinging = false;
                        player.rigidbody.velocity = Vector3.zero;
                        player.rigidbody.AddForce(posDiff.x * 30, 300f, 0f);
                        reelingIn = true;
                    }
                }
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // Initiate swinging if it has collided with an object
        if (string.Compare(other.name, "Platform") == 0 && !reelingIn)
        {
            hasCollided = true;
            player.GetComponentInChildren<PlayerController>().Swinging = true;
            Swing1();
        }

        // Initiate swinging if it has collided with an object
        if (string.Compare(other.name, "Something") == 0 && !reelingIn)
        {
            hasCollided = true;
            reelingIn = true;
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    void Swing1()
    {
        if (player.GetComponentInChildren<PlayerController>().isFacingRight)
        {
            playerPos = player.transform.position;
            grapplePos = this.transform.position;
            posDiff = grapplePos - playerPos;

            if (player.GetComponentInChildren<PlayerController>().isGrounded || posDiff.x < 0.0f)
            {
                player.rigidbody.AddForce(posDiff.x * 20, 0, 0);
                reelingIn = true;
            }
            else
            {
                startPos = playerPos;
                endPos = new Vector3(playerPos.x + 2 * posDiff.x, playerPos.y, playerPos.z);

                timeStamp = Time.time;
                player.GetComponentInChildren<PlayerController>().isSwinging = true;
                radius = Vector3.Distance(grapple.transform.position, player.transform.position);
            }
        }
        else
        {

            playerPos = player.transform.position;
            grapplePos = this.transform.position;
            posDiff = grapplePos - playerPos;

            if (player.GetComponentInChildren<PlayerController>().isGrounded || posDiff.x > 0.0f)
            {
                player.rigidbody.AddForce(posDiff.x * 20, 0, 0);
                reelingIn = true;
            }
            else
            {
                startPos = playerPos;
                endPos = new Vector3(playerPos.x + 2 * posDiff.x, playerPos.y, playerPos.z);

                timeStamp = Time.time;
                player.GetComponentInChildren<PlayerController>().isSwinging = true;
                radius = Vector3.Distance(grapple.transform.position, player.transform.position);
            }
        }
    }

    void OnDestroy()
    {
        try
        {
            player.GetComponentInChildren<PlayerController>().Swinging = false;
            player.GetComponentInChildren<PlayerController>().Chaining = false;
            player.GetComponentInChildren<PlayerController>().isSwinging = false;
            foreach(GameObject obj in chains)
            {
                Destroy(obj);
            }
        }
        catch (Exception e)
        {
        }
    }

    void ReelIn()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 20 * Time.deltaTime);
        if (this.transform.position == player.transform.position) Destroy(this.gameObject);
    }

    void RetractIn()
    {
        if (Input.GetAxis("Trigger Axis") > 0)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, this.transform.position, Input.GetAxis("Trigger Axis") * 5 * Time.deltaTime);
            player.GetComponent<Rigidbody>().isKinematic = true;

        }

        if (this.transform.position == player.transform.position)
        {
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.rigidbody.AddForce(0, 500, 0f);
            Destroy(this.gameObject);
        }
    }

}
