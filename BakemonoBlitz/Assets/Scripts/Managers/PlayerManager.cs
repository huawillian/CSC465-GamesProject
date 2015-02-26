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

    public GameObject player;
    public PlayerController playerController;
    public GrappleController grappleController;

    // Holds are used for grapple
    public bool LTH, RTH;
    // Used for movement, LT is used to control reeling speed
    public float LTA, LX, LY;

    // Player States, each of these states has a list of do-able actions
    public enum PlayerState{Idling, Running, Dashing, Swinging, WallSliding, Damaged, Jumping, Falling};
    public enum GrappleState { GrappleReady, GrappleExtending, GrappleHooked, GrappleRetracting}
    public PlayerState state = PlayerState.Idling;
    public GrappleState grappleState = GrappleState.GrappleReady;
    // Actions:
    // Run - grounded and Left stick X axis
    // Dash - dash button, any time, but must be done with cooldown
    // Jump - grounded and jump button. Can do so while running or idling
    // Throw - throw grapple button. Grapple must be completely reeled in
    // Slide - close to wall and pressing left stick towards the wall
    // Swing - only during throwing and it hooks to a wall or ceiling
    // Reel - Reel Button, but only when swinging
    public bool canRun, canDash, canJump, canThrow, canSlide;
    public bool throwReady, dashReady, jumpReady, reelReady;

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
            player.gameObject.transform.position = new Vector3(x,y,z);
        }
        else
        {
            // Or receive Coordinate information
            x = player.gameObject.transform.position.x;
            y = player.gameObject.transform.position.y;
            z = player.gameObject.transform.position.z;
        }

        // If we lock Player Input, we set reset everything, and, we don't do the input functions
        if (lockPlayerInput)
        {
            LTH = false;
            RTH = false;
            LTA = 0.0f;
            LX = 0.0f;
            LY = 0.0f;
        }

        // Constrain list of do-able actions from the current State
        // We will use Input and Player Controller flags to determine changes in State
        switch (state)
        {
            case PlayerState.Idling:
                canRun = true;
                canDash = true;
                canJump = true;
                canThrow = true;
                canSlide = false;
                jumpReady = true;
                break;
            case PlayerState.Running:
                canRun = true;
                canDash = true;
                canJump = true;
                canThrow = true;
                canSlide = false;
                break;
            case PlayerState.Dashing:
                canRun = false;
                canDash = false;
                canJump = false;
                canThrow = false;
                canSlide = false;
                break;
            case PlayerState.Jumping:
                canRun = false;
                canDash = true;
                canJump = false;
                canThrow = true;
                canSlide = false;
                break;
            case PlayerState.Falling:
                canRun = false;
                canDash = true;
                canJump = false;
                canThrow = true;
                canSlide = true;
                break;
            case PlayerState.WallSliding:
                canRun = false;
                canDash = true;
                canJump = true;
                canThrow = false;
                canSlide = true;
                jumpReady = true;
                break;
            case PlayerState.Damaged:
                canRun = false;
                canDash = false;
                canJump = false;
                canThrow = false;
                canSlide = false;
                break;
            default:
                break;
        }

        switch (grappleState)
        {
            case GrappleState.GrappleReady:
                throwReady = true;
                reelReady = false;
                break;
            case GrappleState.GrappleExtending:
                throwReady = false;
                reelReady = false;
                break;
            case GrappleState.GrappleHooked:
                throwReady = false;
                reelReady = true;
                break;
            case GrappleState.GrappleRetracting:
                throwReady = false;
                reelReady = true;
                break;
            default: 
                break;
        }


        // Change States On Player Flags, Exit States
        switch (state)
        {
            case PlayerState.Idling:
                if (player.rigidbody2D.velocity.y < 0.0f)
                {
                    state = PlayerState.Falling;
                    break;
                }
                if (LX != 0.0f) 
                {
                    state = PlayerState.Running;
                }
                // Dash By Input
                // Jump By Input
                // Throw By Input
                break;
            case PlayerState.Running:
                if (player.rigidbody2D.velocity.y < 0.0f)
                {
                    state = PlayerState.Falling;
                    break;
                }
                if (LX == 0.0f) 
                {
                    state = PlayerState.Idling;
                }
                // Dash By Input
                // Jump By Input
                // Throw By Input
                break;
            case PlayerState.Jumping:
                if (player.rigidbody2D.velocity.y < 0.0f || (!playerController.GroundCollide && player.rigidbody2D.velocity.y == 0.0f))
                {
                    state = PlayerState.Falling;
                    break;
                }
                // Dash By Input
                // Throw By Input
                break;
            case PlayerState.Falling:
                if (player.rigidbody2D.velocity.y == 0.0f)
                {
                    state = PlayerState.Idling;
                    break;
                }
                // Dash By Input
                // Throw By Input
                // Slide By Input
                break;
            case PlayerState.Dashing:
                if (player.rigidbody2D.velocity.y < 0.0f)
                {
                    state = PlayerState.Falling;
                    break;
                }

                if (player.rigidbody2D.velocity.y == 0.0f)
                {
                    state = PlayerState.Idling;
                    break;
                }
                break;
            case PlayerState.Swinging:
                /*
                if ((player.rigidbody2D.velocity.y < 0.0f) || !RTH)
                {
                    state = PlayerState.Falling;
                    break;
                }*/
                if (!RTH)
                {
                    state = PlayerState.Falling;
                    grappleState = GrappleState.GrappleRetracting;
                    grappleController.swinging = false;
                }
                if (LTH)
                {
                    grappleController.radius -= 0.1f;
                }
                // Will set Player State to falling after completing Swing
                break;
            case PlayerState.WallSliding:
                if (playerController.WallCollide && playerController.WallCollideRight && LX <= 0.0f)
                {
                    state = PlayerState.Falling;
                    break;
                }

                if (playerController.WallCollide && !playerController.WallCollideRight && LX >= 0.0f)
                {
                    state = PlayerState.Falling;
                    break;
                }

                if (!playerController.WallCollide)
                {
                    state = PlayerState.Falling;
                    break;
                }
                break;
            case PlayerState.Damaged:
                // Player will be set to Idle after a while
                break;
            default:
                break;
        }

        // Grapple States will be changed via Grapple Scripts

        if (canRun)
        {
            player.rigidbody2D.velocity = new Vector2(LX*3.0f,player.rigidbody2D.velocity.y);
        }

        if (state == PlayerState.Falling || state == PlayerState.Jumping)
        {
            player.rigidbody2D.velocity = new Vector2(LX*1.0f, player.rigidbody2D.velocity.y);
        }

        if (state == PlayerState.Falling)
        {
            if ((playerController.WallCollide && playerController.WallCollideRight && LX > 0.0f) || (playerController.WallCollide && !playerController.WallCollideRight && LX < 0.0f)) state = PlayerState.WallSliding;
        }

        if (state == PlayerState.WallSliding)
        {
            player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, player.rigidbody2D.velocity.y* 0.95f);
        }

	}


    // Jump
    public void A()
    {
        if (lockPlayerInput) return;

        if (canJump && jumpReady)
        {
            if (state == PlayerState.WallSliding)
            {

                if(playerController.WallCollideRight) player.rigidbody2D.AddForce(new Vector2(-1500.0f, 300.0f));
                else player.rigidbody2D.AddForce(new Vector2(1500.0f, 300.0f));
                jumpReady = false;
                state = PlayerState.Jumping;
            }
            else
            {
                player.rigidbody2D.AddForce(new Vector2(0, 300.0f));
                jumpReady = false;
                state = PlayerState.Jumping;
            }
        }
    }

    // Nothing
    public void B()
    {
        if (lockPlayerInput) return;

    }

    // Dash
    public void X()
    {
        if (lockPlayerInput) return;

        if (canDash && dashReady)
        {
            player.rigidbody2D.AddForce(new Vector2(LX*500.0f, LY*500.0f));
            dashReady = false;
            state = PlayerState.Dashing;
            StartCoroutine("SetDash");
        }

    }

    // Nothing
    public void Y()
    {
        if (lockPlayerInput) return;

    }

    // Reel
    public void LT()
    {
        if (lockPlayerInput) return;

    }

    // Throw Grapple
    public void RT()
    {
        if (lockPlayerInput) return;
        if (canThrow && throwReady)
        {
            grappleState = GrappleState.GrappleExtending;
            grappleController.timeThrown = Time.time;
        }
    }

    IEnumerator SetDash()
    {
        yield return new WaitForSeconds(3.0f);
        dashReady = true;
    }

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);

        if (!(Application.loadedLevelName == "MainMenuScene"))
        {
            player = GameObject.Find("Player");
            playerController = player.GetComponent<PlayerController>();
            grappleController = GameObject.Find("Grapple").GetComponent<GrappleController>();
            dashReady = true;
        }
    }

}
