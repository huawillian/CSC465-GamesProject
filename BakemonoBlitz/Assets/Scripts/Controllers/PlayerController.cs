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

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Falling || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Jumping || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Running) 
        {
            if (mSceneManager.mPlayerManager.LX > 0) FaceRight = true;
            else if (mSceneManager.mPlayerManager.LX < 0) FaceRight = false;
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

        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Dashing)
        {
            EnemyCollide = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state != PlayerManager.PlayerState.Dashing)
        {
            EnemyCollide = true;
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
    }
}
