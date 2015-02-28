using UnityEngine;
using System.Collections;

public class GrappleController : MonoBehaviour
{
    SceneManager mSceneManager;

    public bool ThrowRight;
    public Vector3 hookedPos;

    public float grappleExtendDur = 1.0f;
    public float timeThrown = 0.0f;

    // Use this for initialization
    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        hookedPos = new Vector3();
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
                break;
            case PlayerManager.GrappleState.GrappleExtending:
                if (ThrowRight)
                {
                    this.gameObject.rigidbody2D.velocity = new Vector2(8,8);
                }
                else
                {
                    this.gameObject.rigidbody2D.velocity = new Vector2(-8, 8);
                }

                if (Time.time - timeThrown > grappleExtendDur)
                {
                    mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
                }
                break;
            case PlayerManager.GrappleState.GrappleHooked:
                this.gameObject.rigidbody2D.velocity = new Vector2(0, 0);
                break;
            case PlayerManager.GrappleState.GrappleRetracting:
                this.gameObject.rigidbody2D.velocity = new Vector2((mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x - this.gameObject.transform.position.x) * 5.0f, (mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y - this.gameObject.transform.position.y)* 5.0f);
                break;
            default:
                break;
        }
    }

    public float radius = 5.0f;
    public bool swinging = false;
    float timeStamp = 0.0f;
    float translationTime = 1.0f;

    void FixedUpdate()
    {
        if (swinging)
        {
            float currX;
            float currY;

            if (mSceneManager.mPlayerManager.playerController.FaceRight)
            {
                currX = Mathf.Cos(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + this.gameObject.transform.position.x;
                currY = Mathf.Sin(5 * Mathf.PI / 4 + (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + this.gameObject.transform.position.y;
            }
            else
            {
                currX = Mathf.Cos(7 * Mathf.PI / 4 - (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + this.gameObject.transform.position.x;
                currY = Mathf.Sin(7 * Mathf.PI / 4 - (Time.time - timeStamp) / translationTime * Mathf.PI / 2) * radius + this.gameObject.transform.position.y;
            }
            mSceneManager.mPlayerManager.playerController.gameObject.transform.position = new Vector3(currX, currY, 0);
        }
    }

    IEnumerator SwingStart()
    {
        mSceneManager.mPlayerManager.lockPlayerInput = true;
        timeStamp = Time.time;
        swinging = true;
        radius = Mathf.Sqrt((this.gameObject.transform.position.x - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x) * (this.gameObject.transform.position.x - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.x) + (this.gameObject.transform.position.y - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y) * (this.gameObject.transform.position.y - mSceneManager.mPlayerManager.playerController.gameObject.transform.position.y));

        // Exit State, Duration, Collided by Wall, or Collided by Ground
        while (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Swinging)
        {
            if (mSceneManager.mPlayerManager.playerController.WallCollide || mSceneManager.mPlayerManager.playerController.GroundCollide || (Time.time - timeStamp > translationTime))
            {
                mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Idling;
                mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
            }

            if (mSceneManager.mPlayerManager.playerController.WallCollide)
            {
                if (mSceneManager.mPlayerManager.playerController.WallCollideRight)
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(-1000, 500));
                }
                else
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(1000, 500));
                }
            }

            if (mSceneManager.mPlayerManager.playerController.GroundCollide)
            {
                if (mSceneManager.mPlayerManager.playerController.FaceRight)
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(1000, 0));
                }
                else
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(-1000, 0));
                }
            }

            if (Time.time - timeStamp > translationTime)
            {
                if (mSceneManager.mPlayerManager.playerController.FaceRight)
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(200, 1000));
                }
                else
                {
                    mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.AddForce(new Vector2(-200, 1000));
                }
            }




            yield return new WaitForSeconds(0.05f);
        }

        mSceneManager.mPlayerManager.lockPlayerInput = false;
        swinging = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Ceiling1")
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleHooked;
            mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Swinging;

            hookedPos = this.gameObject.transform.position;
            StartCoroutine("SwingStart");
        }

        if (other.name == "Wall1")
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
        }

        if (other.name == "Ground1")
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleRetracting;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player" && (mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleRetracting || mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleHooked))
        {
            mSceneManager.mPlayerManager.grappleState = PlayerManager.GrappleState.GrappleReady;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }
}
