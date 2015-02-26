using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public bool WallCollide;
    public bool WallCollideRight;

    public bool GroundCollide;

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
        if (other.name == "Wall1")
        {
            WallCollide = true;
            if (this.gameObject.transform.position.x > other.gameObject.transform.position.x) WallCollideRight = false;
            else WallCollideRight = true;
        }

        if (other.name == "Ground1")
        {
            GroundCollide = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Wall1")
        {
            WallCollide = false;
        }

        if (other.name == "Ground1")
        {
            GroundCollide = false;
        }
    }
}
