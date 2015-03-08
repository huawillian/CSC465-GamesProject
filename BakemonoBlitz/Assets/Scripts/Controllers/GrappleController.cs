using UnityEngine;
using System.Collections;

public class GrappleController : MonoBehaviour
{
    SceneManager mSceneManager;

    public bool ThrowRight;
    public Vector3 hookedPos;

    public float grappleExtendDur = 0.20f;
    public float grappleSpeed = 12.0f;
    public float timeThrown = 0.0f;
    public float reelingSpeed = 0.075f;
    public float retractingSpeed = 17.5f;
    public int incre;

    public bool reelingIn;

    // Use this for initialization
    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        hookedPos = new Vector3();
        this.rigidbody2D.gravityScale = 0.0f;
        reelingIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (mSceneManager.mPlayerManager.grappleState)
        {
            case PlayerManager.GrappleState.GrappleReady:
                ThrowRight = mSceneManager.mPlayerManager.playerController.FaceRight;
                mSceneManager.mPlayerManager.throwReady = true;
                this.transform.position = mSceneManager.mPlayerManager.playerController.gameObject.transform.position;
                this.renderer.enabled = false;
                break;
            case PlayerManager.GrappleState.GrappleExtending:
                if (ThrowRight)
                {
                    //this.gameObject.rigidbody2D.velocity = new Vector2(grappleSpeed, grappleSpeed);
                    this.gameObject.transform.position = new Vector3(grappleSpeed / 40.0f * incre + mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x,
                        grappleSpeed / 40.0f * incre + mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y,
                        this.gameObject.transform.position.z);

                    incre++;
                }
                else
                {
                    //this.gameObject.rigidbody2D.velocity = new Vector2(-grappleSpeed, grappleSpeed);
                    this.gameObject.transform.position = new Vector3(-grappleSpeed / 40.0f * incre + mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x,
                        grappleSpeed / 40.0f * incre + mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y,
                        this.gameObject.transform.position.z);

                    incre++;
                }

                if (Time.time - timeThrown > grappleExtendDur)
                {
                    mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
                }
                this.renderer.enabled = true;
                break;
            case PlayerManager.GrappleState.GrappleHooked:
                this.gameObject.rigidbody2D.velocity = new Vector2(0, 0);
                break;
            case PlayerManager.GrappleState.GrappleRetracting:
                //this.gameObject.rigidbody2D.velocity = new Vector2((mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x - this.gameObject.transform.position.x) * 5.0f, (mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y - this.gameObject.transform.position.y)* 10.0f);
                this.transform.position = Vector3.MoveTowards(transform.position, mSceneManager.mPlayerManager.playerController.gameObject.transform.position, retractingSpeed * Time.deltaTime);

                break;
            default:
                break;
        }

        if (ThrowRight)
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }

    }

    public float radius = 5.0f;
    public bool swinging = false;
    float timeStamp = 0.0f;
    float translationTime = 1.0f;

    void FixedUpdate()
    {
        if (swinging && !mSceneManager.mPlayerManager.playerController.EnemyCollide && !mSceneManager.mPlayerManager.playerController.WallCollide && !mSceneManager.mPlayerManager.playerController.GroundCollide)
        {
            float currX;
            float currY;

            if (ThrowRight)
            {
                currX = Mathf.Cos(5.0f * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2.0f) * radius + this.gameObject.transform.position.x;
                currY = Mathf.Sin(5.0f * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2.0f) * radius + this.gameObject.transform.position.y;
            }
            else
            {
                currX = Mathf.Cos(7.0f * Mathf.PI / 4 - (Time.time - timeStamp) / translationTime * Mathf.PI / 2.0f) * radius + this.gameObject.transform.position.x;
                currY = Mathf.Sin(7.0f * Mathf.PI / 4 - (Time.time - timeStamp) / translationTime * Mathf.PI / 2.0f) * radius + this.gameObject.transform.position.y;
            }
            mSceneManager.mPlayerManager.playerController.gameObject.transform.position = new Vector3(currX, currY, 0);

            if (reelingIn)
            {
                radius -= reelingSpeed;
            }
        }
    }

    IEnumerator SwingStart()
    {
        mSceneManager.mPlayerManager.lockPlayerInput = true;
        timeStamp = Time.time;
        swinging = true;
        radius = Mathf.Sqrt((this.gameObject.transform.position.x - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x) * (this.gameObject.transform.position.x - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x) + (this.gameObject.transform.position.y - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y) * (this.gameObject.transform.position.y - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y));
        translationTime = 1.0f * radius / 8.0f;


        // Exit State, Duration, Collided by Wall, or Collided by Ground
        while (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Swinging && swinging && mSceneManager.mPlayerManager.RTH && Time.time - timeStamp <= translationTime)
        {
            yield return new WaitForSeconds(0.01f);

            if (mSceneManager.mPlayerManager.playerController.WallCollide || mSceneManager.mPlayerManager.playerController.GroundCollide || (Time.time - timeStamp > translationTime))
            {
                mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Idling;
                mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
            }

            if (mSceneManager.mPlayerManager.playerController.WallCollide)
            {
                if (mSceneManager.mPlayerManager.playerController.WallCollideRight)
                {
                    //mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(-1000, 500));
                    mSceneManager.mPlayerManager.StartCoroutine("DashUpLeft");
                }
                else
                {
                    //mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(1000, 500));
                    mSceneManager.mPlayerManager.StartCoroutine("DashUpRight");
                }

                swinging = false;
            }

            if (mSceneManager.mPlayerManager.playerController.GroundCollide)
            {
                if (ThrowRight)
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(200, 0));
                    //mSceneManager.mPlayerManager.StartCoroutine("DashUpRight");
                }
                else
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(-200, 0));
                    //mSceneManager.mPlayerManager.StartCoroutine("DashUpLeft");
                }

                swinging = false;
            }


            if (mSceneManager.mPlayerManager.playerController.EnemyCollide)
            {
                if (ThrowRight)
                {
                    //mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(-200, 0));
                    mSceneManager.mPlayerManager.StartCoroutine("DashUpLeft");
                }
                else
                {
                    //mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(200, 0));
                    mSceneManager.mPlayerManager.StartCoroutine("DashUpRight");
                }

                swinging = false;
            }
        }

        // Add Force depend on Angle created by player and grapple
        if (Time.time - timeStamp > translationTime || !mSceneManager.mPlayerManager.RTH)
        {
            if (ThrowRight)
            {
                float angleY = Mathf.Cos(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2 + 1 / 8 * Mathf.PI);
                float angleX = -1.0f * Mathf.Sin(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2 + 1 / 8 * Mathf.PI);

                if (angleY < 0.0f)
                {
                    angleY = 0.2f;
                    mSceneManager.mPlayerManager.player.rigidbody2D.velocity = new Vector2(mSceneManager.mPlayerManager.player.rigidbody2D.velocity.x, 0);
                }

                mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(350 * angleX, 750 * angleY));
            }
            else
            {
                float angleY = Mathf.Cos(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2 + 1 / 8 * Mathf.PI);
                float angleX = Mathf.Sin(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2 + 1 / 8 * Mathf.PI);

                if (angleY < 0.0f)
                {
                    angleY = 0.2f;
                    mSceneManager.mPlayerManager.player.rigidbody2D.velocity = new Vector2(mSceneManager.mPlayerManager.player.rigidbody2D.velocity.x, 0);
                }

                mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(350 * angleX, 750 * angleY));
            }
        }


        mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Falling;
        mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
        swinging = false;
        mSceneManager.mPlayerManager.lockPlayerInput = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ceiling")
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleHooked;
            mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Swinging;

            hookedPos = this.gameObject.transform.position;
            StartCoroutine("SwingStart");
        }

        if (other.tag == "Wall")
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
        }

        if (other.tag == "Ground")
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
        }

        if (other.tag == "Enemy")
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && (mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleRetracting || mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleHooked))
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleReady;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }
}
