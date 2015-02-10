using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Name: Willian Hua
// Date: 11/24/14
// Project: Attack on Ninja, Team Koga
// Description: Controls all input from the XBox controller. All keys are mapped.
// Usage: Attached to GameControllers object as component

/* 
 * Input Manager must be configured properly in order for this script to work...
 */ 

public class InputController : MonoBehaviour
{
	// Get Reference to Player Object
	public GameObject player;

	// FINE TUNING VARIABLES
	public float jumpForce = 450.0f;
	public float dashForce = 500.0f;
	public float brakeMovementCoefficient = 5.0f;
	public float groundMovementCoefficient = 30.0f;
	public float airMovementCoefficient = 20.0f;

    public GameObject shuriken;
    public GameObject grapple;
    public GameObject grappleholder;


    // Camera Panning variables
    public bool disableControls = false;
    public GameObject camera;
    float cameraOriginalSize = 3.5f;
    Vector3 cameraPanPos = Vector3.zero;
    bool panning = false;

    // Dashing variables
    public float shortDashDist = 3.0f;
    public Vector3 dashStart = Vector3.zero;
    public Vector3 dashEnd = Vector3.zero;
    public bool dashing = false;
    public float dashStartTime = 0.0f;
    public float dashDuration = 0.2f;

	void Start ()
	{
		Debug.Log ("InputController starting up...");
		player = GameObject.Find ("Player");
	}
	
	void Update ()
	{
        if (!disableControls)
        {
            // JUMP button
            // Jumps only when grounded
            // Adds an upward force
            if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("A has been pressed");
                if (player.GetComponentInChildren<PlayerController>().isGrounded)
                    player.rigidbody.AddForce(0, jumpForce, 0);
            }

            // DASH button
            // Adds Horizontal force based on the direction the player is facing
            if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log(Input.GetAxis("Left Analog Stick X") + " " + Input.GetAxis("Left Analog Stick Y"));

                if (!dashing)
                {
                    if (Input.GetAxis("Left Analog Stick X") == 0 && Input.GetAxis("Left Analog Stick Y") == 0)
                    {
                        if (player.GetComponentInChildren<PlayerController>().isFacingRight)
                        {
                            dashStart = player.transform.position;
                            dashEnd = new Vector3(player.transform.position.x + shortDashDist, player.transform.position.y, 0);
                            dashStartTime = Time.time;
                            dashing = true;
                        }
                        else
                        {
                            dashStart = player.transform.position;
                            dashEnd = new Vector3(player.transform.position.x - shortDashDist, player.transform.position.y, 0);
                            dashStartTime = Time.time;
                            dashing = true;
                        }
                    }
                    else
                    {
                        //player.rigidbody.AddForce(Input.GetAxis("Left Analog Stick X") * dashForce, Input.GetAxis("Left Analog Stick Y") * dashForce, 0);
                        dashStart = player.transform.position;
                        dashEnd = new Vector3(player.transform.position.x + Input.GetAxis("Left Analog Stick X") * shortDashDist, player.transform.position.y + Input.GetAxis("Left Analog Stick Y") * shortDashDist, 0);
                        dashStartTime = Time.time;
                        dashing = true;
                    }
                }
            }

            // SHOOT PROJECTILE button
            // Create and set forward direction of the projectile
            if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.E))
            {
                Instantiate(shuriken, player.transform.position, Quaternion.identity);
            }

            // SHOOT GRAPPLE button
            // Create at player location
            if (Input.GetButtonDown("Y") || Input.GetKeyDown(KeyCode.Q))
            {
                Instantiate(grapple, player.transform.position, Quaternion.identity);
            }

            /*
            * Still Unset
            * * * * * * * * * * * * * */

            if (Input.GetButtonDown("Start"))
            {
                Debug.Log("Start has been pressed");
                Application.LoadLevel(Application.loadedLevelName);
            }

            if (Input.GetButtonDown("Back"))
            {
                Debug.Log("Back has been pressed");
            }

            if (DPadButtons.up)
            {
                Debug.Log("D Pad Up has been pressed");
            }

            if (DPadButtons.down)
            {
                Debug.Log("D Pad Down has been pressed");
            }

            if (DPadButtons.left)
            {
                Debug.Log("D Pad Left has been pressed");
            }

            if (DPadButtons.right)
            {
                Debug.Log("D Pad Right has been pressed");
            }

            if (Input.GetButtonDown("LB"))
            {
                Debug.Log("LB has been pressed");
            }

            if (Input.GetButtonDown("RB"))
            {
                Debug.Log("RB has been pressed");

            }

            if (TriggerButtons.LT)
            {
                Debug.Log("LT has been pressed");
            }

            if (TriggerButtons.RT)
            {
                Debug.Log("RT has been pressed");

                Instantiate(grapple, player.transform.position, Quaternion.identity);

            }

            // Use the button methods to get Axis Values
            //Debug.Log (Input.GetAxis("Right Analog Stick X"));
            //Debug.Log (Input.GetAxis("Right Analog Stick Y"));
            //Debug.Log (Input.GetAxis("Left Analog Stick Y"));
            //Debug.Log (Input.GetAxis("Left Analog Stick X"));

            /*
             * Keyboard Input Debugging
             * * * * * * * * * * * * * */

            if (Input.GetKey(KeyCode.A))
            {
                player.rigidbody.AddForce(-28, 0, 0);
                player.GetComponentInChildren<PlayerController>().isFacingRight = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                player.rigidbody.AddForce(28, 0, 0);
                player.GetComponentInChildren<PlayerController>().isFacingRight = true;
            }
        }
	}

    public bool groundToggle = false;
    public float initSpeed = 0.0f;
    public bool wasFacingRight = true;
    public Vector3 vel = Vector3.zero;

	void FixedUpdate()
    {
        if (!disableControls)
        {
            // Player movement based on player states:
            // Grounded, Speeding, Jumping
            if (player.GetComponentInChildren<PlayerController>().isGrounded)
            {
                if (player.GetComponentInChildren<PlayerController>().isSpeeding && (Input.GetAxis("Left Analog Stick X") * player.rigidbody.velocity.x < 0))
                {
                    // Player Movement when on the ground, player is speeding, and the input is in the opposite direction of current velocity
                    player.rigidbody.AddForce(Input.GetAxis("Left Analog Stick X") * brakeMovementCoefficient, 0, 0);
                }
                else
                {
                    // Player Movement when on the ground normally
                    player.rigidbody.AddForce(Input.GetAxis("Left Analog Stick X") * groundMovementCoefficient, 0, 0);
                }
            }
            else
            {
                // Player Movement in the Air
                player.rigidbody.AddForce(Input.GetAxis("Left Analog Stick X") * airMovementCoefficient, 0, 0);
            }

            if (groundToggle == true && player.GetComponentInChildren<PlayerController>().isGrounded == false)
            {
                groundToggle = false;
                initSpeed = Input.GetAxis("Left Analog Stick X");
                if (initSpeed > 0) wasFacingRight = true;
                else wasFacingRight = false;

                vel = player.rigidbody.velocity;
            }
            if (groundToggle == false && player.GetComponentInChildren<PlayerController>().isGrounded == true)
            {
                groundToggle = true;
                initSpeed = 0.0f;
            }
        }

        // Pan Camera when Right Analog Stick is Pressed
        if ((Input.GetAxis("Right Analog Stick X") != 0 || Input.GetAxis("Right Analog Stick Y") != 0) && !disableControls)
        {
            disableControls = true;
            panning = true;
            cameraOriginalSize = camera.GetComponent<Camera>().orthographicSize;
            cameraPanPos = camera.GetComponent<Camera>().transform.position;
            camera.GetComponent<CameraController>().enabled = false;
        }

        if (Input.GetAxis("Right Analog Stick X") == 0 && Input.GetAxis("Right Analog Stick Y") == 0 && disableControls)
        {
            disableControls = false;
            panning = false;
            camera.GetComponent<Camera>().orthographicSize = cameraOriginalSize;
            camera.GetComponent<CameraController>().enabled = true;

        }

        if (panning)
        {
            camera.GetComponent<Camera>().orthographicSize = cameraOriginalSize + 1.0f;
            camera.transform.position = new Vector3(cameraPanPos.x + Input.GetAxis("Right Analog Stick X") * 2.5f, cameraPanPos.y + Input.GetAxis("Right Analog Stick Y") * 2.5f, cameraPanPos.z);
        }


        // Reel in grapple
        if (Input.GetAxis("Trigger Axis") > 0)
        {
            try
            {
                GameObject.Find("Grapple(Clone)").GetComponent<Grapple>().radius -= (Input.GetAxis("Trigger Axis")) / 15;
            }
            catch (Exception e) { }
        }

        // Dashing
        if (dashing && Time.time <= dashStartTime + dashDuration)
        {
            player.transform.position = (dashEnd - dashStart) * (Time.time - dashStartTime) / dashDuration + dashStart;
            player.rigidbody.isKinematic = true;
        }
        else if (dashing && Time.time > dashStartTime + dashDuration)
        {
            StartCoroutine("freezePlayer", 0.2f);
        }

        if (dashing && player.GetComponent<PlayerController>().hasCollided == true && !player.GetComponent<PlayerController>().isGrounded)
        {
            dashing = false;
            StartCoroutine("freezePlayer", 0.2f);
        }
    }

    IEnumerator freezePlayer(float duration)
    {
        player.rigidbody.isKinematic = true;
        yield return new WaitForSeconds(duration);
        player.rigidbody.isKinematic = false;
        dashing = false;
    }
}