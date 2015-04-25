using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public bool WallCollide;
    public bool WallCollideRight;

    public bool GroundCollide;
    public bool EnemyCollide;
    public bool FaceRight;

    SceneManager mSceneManager;

    public Sprite Idle;
    public Sprite Running;
    public Sprite Jumping;
    public Sprite Landing;
    public Sprite Swinging;
    public Sprite Damaged;
    public Sprite Chaining;

    public SpriteRenderer renderer;

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        renderer = this.GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Falling || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Jumping || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Running) 
        {
            if (mSceneManager.mPlayerManager.LX > 0) FaceRight = true;
            else if (mSceneManager.mPlayerManager.LX < 0) FaceRight = false;
        }


        if (FaceRight) this.gameObject.transform.rotation = new Quaternion(0,0,0,0);
        else this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);

            if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Idling)
            {
                renderer.sprite = Idle;
            }
            else
                if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Running)
                {
                    renderer.sprite = Running;
                }
                else
                    if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Jumping)
                    {
                        renderer.sprite = Jumping;
                    }
                    else
                        if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Falling)
                        {
                            renderer.sprite = Landing;
                        }
                        else
                            if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.WallSliding)
                            {
                                renderer.sprite = Landing;
                            }
                            else
                                if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Swinging)
                                {
                                    renderer.sprite = Swinging;
                                }
                                else
                                    if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Dashing)
                                    {
                                        renderer.sprite = Running;
                                    }
                                    else
                                        if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Damaged)
                                        {
                                            renderer.sprite = Damaged;
                                        }
            if (mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleExtending)
            {
                renderer.sprite = Chaining;
            }

            if (WallCollide)
            {
                if (WallCollideRight)
                {
                    FaceRight = true;
                }
                else
                {
                    FaceRight = false;
                }
            }

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            WallCollide = true;
            if (this.gameObject.transform.position.x > other.gameObject.transform.position.x) WallCollideRight = false;
            else WallCollideRight = true;
        }
        if (other.tag == "Ground")
        {
            GroundCollide = true;
        }

        if (other.tag == "Dynamic Platform" && (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Idling || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Running))
        {
            GroundCollide = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            WallCollide = true;
            if (this.gameObject.transform.position.x > other.gameObject.transform.position.x) WallCollideRight = false;
            else WallCollideRight = true;
        }

        if (other.tag == "Ground")
        {
            GroundCollide = true;
        }

        if (other.tag == "Dynamic Platform" && (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Idling || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Running))
        {
            GroundCollide = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            WallCollide = false;
        }

        if (other.tag == "Ground")
        {
            GroundCollide = false;
        }


        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state != PlayerManager.PlayerState.Dashing)
        {
            EnemyCollide = false;
        }

        if (other.tag == "Dynamic Platform")
        {
            GroundCollide = false;
        }
    }
}
