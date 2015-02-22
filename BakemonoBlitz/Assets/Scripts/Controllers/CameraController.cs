using UnityEngine;
using System.Collections;

// Name: Willian Hua
// Date: 11/24/14
// Project: Attack on Ninja, Team Koga
// Description: Controls camera movement and camera options
// Usage: Attatched to Main Camera as Component

public class CameraController : MonoBehaviour
{
	public GameObject player;

    public float smoothTime = 1.5f;
    private Vector2 velocity;
    public bool panning = false;

	void Start ()
	{
		Debug.Log ("CameraController starting up...");
		player = GameObject.Find ("Player");
	}

	// Smoothly move towards player X and Y positions
	void FixedUpdate ()
	{

        Vector3 vec = this.transform.position;
        //vec.x = Mathf.SmoothDamp(this.transform.position.x,
        //    player.transform.position.x, ref velocity.x, smoothTime);
        vec.x = player.transform.position.x;
        vec.y = Mathf.SmoothDamp(this.transform.position.y,
            player.transform.position.y + 1, ref velocity.y, smoothTime);
        this.transform.position = vec;
    }
}
