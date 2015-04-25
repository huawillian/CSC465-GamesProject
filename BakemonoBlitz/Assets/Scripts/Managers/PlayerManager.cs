using UnityEngine;
using System.Collections;
using System;

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
    private SceneManager mSceneManager;

    public string gender;
    public int health, energy, lives, gems, score, time;
    public bool weapon1, weapon2, weapon3;
    public float x, y, z;

    public float startTime;
    public float savedTime;
    private float currentTime;

    public bool lockPlayerCoordinates = false;
    public bool lockPlayerInput = false;
    public Vector2 lockedPlayerVel;

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
    public bool throwReady, dashReady, jumpReady, reelReady, dashReadyFromGrapple;

    public float maxSpeed = 20.0f;
    public float dashCooldown = 3.0f;
    public float dashTimeStamp = 0.0f;
    public bool longDashReady = false;

    public bool braked = false;
    public bool wasSpeeding = false;

    public bool invincible = false;

    public int maxLives = 5;
    public int maxHealth = 3;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

	// Use this for initialization
	void Start ()
    {
        this.mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        Debug.Log("Initializing " + this.gameObject.name);

        grappleController = GameObject.Find("Grapple").GetComponent<GrappleController>();
        dashReady = true;
        dashReadyFromGrapple = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (mSceneManager.state == SceneManager.SceneState.MainMenu) return;

        if (!lockPlayerCoordinates && (mSceneManager.state == SceneManager.SceneState.Locked || mSceneManager.state == SceneManager.SceneState.SceneComplete || mSceneManager.state == SceneManager.SceneState.Paused))
        {
            lockPlayerCoordinates = true;
            lockedPlayerVel = player.rigidbody2D.velocity;
            player.rigidbody2D.isKinematic = true;

        }
        
        if (lockPlayerCoordinates && !((mSceneManager.state == SceneManager.SceneState.Locked || mSceneManager.state == SceneManager.SceneState.SceneComplete || mSceneManager.state == SceneManager.SceneState.Paused)))
        {
            lockPlayerCoordinates = false;
            player.rigidbody2D.isKinematic = false;
            player.rigidbody2D.velocity = lockedPlayerVel;
        }

        // Update time
        currentTime = Time.time;
        time = (int)(currentTime - startTime + savedTime);

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
                longDashReady = false;
                break;
            case PlayerState.Running:
                canRun = true;
                canDash = true;
                canJump = true;
                canThrow = true;
                canSlide = false;
                longDashReady = false;
                break;
            case PlayerState.Dashing:
                canRun = false;
                canDash = true;
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
                longDashReady = false;
                break;
            case PlayerState.Damaged:
                canRun = false;
                canDash = false;
                canJump = false;
                canThrow = false;
                canSlide = false;
                longDashReady = false;
                break;
            default:
                break;
        }

        switch (grappleState)
        {
            case GrappleState.GrappleReady:
                throwReady = true;
                reelReady = false;
                dashReadyFromGrapple = true;
                break;
            case GrappleState.GrappleExtending:
                throwReady = false;
                reelReady = false;
                dashReadyFromGrapple = false;
                break;
            case GrappleState.GrappleHooked:
                throwReady = false;
                reelReady = true;
                dashReadyFromGrapple = false;
                break;
            case GrappleState.GrappleRetracting:
                throwReady = false;
                reelReady = true;
                dashReadyFromGrapple = true;
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
                if (Math.Abs(player.rigidbody2D.velocity.x) > 0.0f) player.rigidbody2D.velocity = player.rigidbody2D.velocity * 0.95f;
                if (Math.Abs(player.rigidbody2D.velocity.x) < 1.0f) player.rigidbody2D.velocity = new Vector2(0, player.rigidbody2D.velocity.y);
                // Dash By Input
                // Jump By Input
                // Throw By Input
                player.rigidbody2D.gravityScale = 2.0f;

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
                player.rigidbody2D.gravityScale = 2.0f;

                break;
            case PlayerState.Jumping:
                if (player.rigidbody2D.velocity.y < 0.0f || (!playerController.GroundCollide && player.rigidbody2D.velocity.y == 0.0f))
                {
                    state = PlayerState.Falling;
                    break;
                }
                // Dash By Input
                // Throw By Input
                player.rigidbody2D.gravityScale = 2.0f;

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
                player.rigidbody2D.gravityScale = 2.0f;

                break;
            case PlayerState.Dashing:
                /*
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
                 */
                player.rigidbody2D.gravityScale = 0.25f;
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
                }

                if (LTH)
                {
                    grappleController.reelingIn = true;
                }
                else
                {
                    grappleController.reelingIn = false;
                }
                // Will set Player State to falling after completing Swing
                break;
            case PlayerState.WallSliding:
                
                /*if (playerController.WallCollide && playerController.WallCollideRight && LX < 0.0f)
                {
                    state = PlayerState.Falling;
                    break;
                }

                if (playerController.WallCollide && !playerController.WallCollideRight && LX > 0.0f)
                {
                    state = PlayerState.Falling;
                    break;
                }*/

                if (playerController.WallCollide && !playerController.GroundCollide)
                {
                    break;

                }
                else
                {
                    state = PlayerState.Falling;
                    break;
                }
            case PlayerState.Damaged:
                // Player will be set to Idle after a while
                break;
            default:
                break;
        }

        if (state == PlayerState.Falling)
        {
            // Initiate WallSlide
            //if ((playerController.WallCollide && playerController.WallCollideRight && LX > 0.0f) || (playerController.WallCollide && !playerController.WallCollideRight && LX < 0.0f)) 
            //    state = PlayerState.WallSliding;

            if (playerController.WallCollide && !playerController.GroundCollide)
                state = PlayerState.WallSliding;
        }

        if(state != PlayerState.Dashing && Time.time - dashTimeStamp > dashCooldown && !dashReady && !longDashReady)
        {
            dashReady = true;
            longDashReady = false;
        }

        if (state == PlayerState.Dashing && Time.time - dashTimeStamp > dashCooldown / 3.0f && longDashReady)
        {
            dashReady = false;
            longDashReady = false;
            state = PlayerState.Falling;
        }



        /*
        if (longDashReady)
        {
            player.rigidbody2D.velocity = Vector2.zero;
        }*/

        if (canRun && !braked)
        {
            if (player.rigidbody2D.velocity.x > maxSpeed)
            {
                player.rigidbody2D.velocity = new Vector2(maxSpeed, player.rigidbody2D.velocity.y);
                wasSpeeding = true;
            }
            else if (player.rigidbody2D.velocity.x < -maxSpeed)
            {
                player.rigidbody2D.velocity = new Vector2(-maxSpeed, player.rigidbody2D.velocity.y);
                wasSpeeding = true;
            }
            else if ((player.rigidbody2D.velocity.x > 0.0f && LX < 0.0f) || (player.rigidbody2D.velocity.x < 0.0f && LX > 0.0f))
            {
                player.rigidbody2D.AddForce(new Vector2(LX * maxSpeed * 2.0f, 0));

                if (Math.Abs(player.rigidbody2D.velocity.x) < 1.0f && wasSpeeding)
                {
                    braked = true;
                    wasSpeeding = false;
                    player.rigidbody2D.velocity = Vector2.zero;
                    StartCoroutine("SetBraked");
                }
            }
            else if (player.rigidbody2D.velocity.x > 0.0f && LX == 0.0f)
            {
                player.rigidbody2D.AddForce(new Vector2(maxSpeed * -0.5f, 0));
            }
            else if (player.rigidbody2D.velocity.x < 0.0f && LX == 0.0f)
            {
                player.rigidbody2D.AddForce(new Vector2(maxSpeed * 0.5f, 0));
            }
            else if (Math.Abs(player.rigidbody2D.velocity.x) < maxSpeed)
            {
                player.rigidbody2D.AddForce(new Vector2(LX * maxSpeed, 0));
                wasSpeeding = false;
            }
        }


        if ((state == PlayerState.Falling || state == PlayerState.Jumping) && longDashReady)
        {
            //player.rigidbody2D.velocity = new Vector2(0, player.rigidbody2D.velocity.y * 0.0f);
        }
        else
        if (state == PlayerState.Falling || state == PlayerState.Jumping)
        {
            player.rigidbody2D.AddForce(new Vector2(LX * maxSpeed / 1.25f, 0));
        }

        if (state == PlayerState.WallSliding)
        {
            player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, player.rigidbody2D.velocity.y * 0.5f);
        }

        if (player.rigidbody2D.velocity.x > maxSpeed && state != PlayerState.Dashing)
        {
            player.rigidbody2D.velocity = new Vector2(maxSpeed, player.rigidbody2D.velocity.y);
        }
        else if (player.rigidbody2D.velocity.x < -maxSpeed && state != PlayerState.Dashing)
        {
            player.rigidbody2D.velocity = new Vector2(-maxSpeed, player.rigidbody2D.velocity.y);
        }

        if (player.rigidbody2D.velocity.y > maxSpeed && state != PlayerState.Dashing)
        {
            player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, maxSpeed);
        }
        else if (player.rigidbody2D.velocity.y < -maxSpeed && state != PlayerState.Dashing)
        {
            player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, -maxSpeed);
        }

        if (lives > maxLives)
        {
            lives = maxLives;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
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
                if (LY < 0)
                {
                    state = PlayerState.Falling;
                    if (playerController.WallCollideRight)
                    {
                        StartCoroutine("DashUpLeftSmallSmall");
                    }
                    else
                    {
                        StartCoroutine("DashUpRightSmallSmall");
                    }
                }
                else
                {

                    if (playerController.WallCollideRight)
                    {
                        StartCoroutine("DashUpLeft");
                    }
                    else
                    {
                        StartCoroutine("DashUpRight");
                    }
                    jumpReady = false;
                    state = PlayerState.Jumping;
                }
            }
            else
            {
                player.rigidbody2D.AddForce(new Vector2(0, 400));
                GameObject.Find("SmokeController").GetComponent<SmokeController>().StartCoroutine("TwoSmokeGeneratorStart");
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

        if (canDash && dashReady && dashReadyFromGrapple && weapon1)
        {
            if (playerController.GetComponentInChildren<DashController>().dashState == DashController.DashState.DashReady)
            {
                playerController.GetComponentInChildren<DashController>().StartCoroutine("Dash");
                GameObject.Find("DashingLineController").GetComponent<DashingLineController>().StartCoroutine("DashAnimationStart");

            }

            if (playerController.GetComponentInChildren<DashController>().dashState == DashController.DashState.DashLong || playerController.GetComponentInChildren<DashController>().dashState == DashController.DashState.DashExit)
            {
                playerController.GetComponentInChildren<DashController>().StopCoroutine("Dash");
                GameObject.Find("DashingLineController").GetComponent<DashingLineController>().StopCoroutine("DashAnimationStart");
                playerController.gameObject.rigidbody2D.velocity = Vector2.zero;
                playerController.GetComponentInChildren<DashController>().StartCoroutine("Dash");
                GameObject.Find("DashingLineController").GetComponent<DashingLineController>().StartCoroutine("DashAnimationStart");
            }
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
        if (canThrow && throwReady && weapon2)
        {
            grappleState = GrappleState.GrappleExtending;
            grappleController.timeThrown = Time.time;
            grappleController.incre = 0;
        }
    }

    public IEnumerator DashUpRight()
    {
        float timeStart = Time.time;
        float dashDuration = 0.2f;

        while(Time.time - timeStart < dashDuration)
        {
            player.rigidbody2D.velocity = new Vector2(7, 6);
            yield return new WaitForSeconds(0.005f);
        }
    }

    public IEnumerator DashUpRightSmall()
    {
        for (int i = 0; i < 5; i++)
        {
            player.rigidbody2D.velocity = new Vector2(4, 4);
            yield return new WaitForSeconds(0.005f);
        }
    }

    public IEnumerator DashUpRightSmallSmall()
    {
        for (int i = 0; i < 5; i++)
        {
            player.rigidbody2D.velocity = new Vector2(3, 1);
            yield return new WaitForSeconds(0.005f);
        }
    }

    public IEnumerator DashUpLeft()
    {
        float timeStart = Time.time;
        float dashDuration = 0.2f;

        while (Time.time - timeStart < dashDuration)
        {
            player.rigidbody2D.velocity = new Vector2(-7, 6);
            yield return new WaitForSeconds(0.005f);
        }

    }

    public IEnumerator DashUpLeftSmall()
    {
        for (int i = 0; i < 5; i++)
        {
            player.rigidbody2D.velocity = new Vector2(-4, 4);
            yield return new WaitForSeconds(0.005f);
        }
    }

    public IEnumerator DashUpLeftSmallSmall()
    {
        for (int i = 0; i < 5; i++)
        {
            player.rigidbody2D.velocity = new Vector2(-3, 1);
            yield return new WaitForSeconds(0.005f);
        }
    }

    public IEnumerator SetBraked()
    {
        yield return new WaitForSeconds(0.2f);
        braked = false;
    }

    public IEnumerator SetInvincible()
    {
        invincible = true;

        for (int i = 0; i < 10; i++)
        {
            player.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 10; i++)
        {
            player.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
            player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.1f);
        invincible = false;
    }

    // Initialization called by Scene Manager
    public void InitializeManager()
    {

    }

}
