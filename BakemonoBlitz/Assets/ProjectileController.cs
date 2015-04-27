using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    public float velocity = 5.0f;
    public Vector3 originalDirection;
    public Vector3 direction;
    SceneManager mSceneManager;

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        originalDirection = this.gameObject.transform.position - mSceneManager.mPlayerManager.player.gameObject.transform.position;
        direction = Vector3.Normalize(originalDirection);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (mSceneManager.state != SceneManager.SceneState.Paused && mSceneManager.state != SceneManager.SceneState.Locked)
        {
            this.gameObject.rigidbody2D.velocity = new Vector2(direction.x * velocity * -1.0f, direction.y * velocity * -1.0f);
        }
        else
        {
            this.gameObject.rigidbody2D.velocity = Vector2.zero;
        }

        if(Vector3.Distance(mSceneManager.mPlayerManager.player.gameObject.transform.position, this.gameObject.transform.position) > 30.0f)
        {
            Destroy(this.gameObject);
        }
	}
}
