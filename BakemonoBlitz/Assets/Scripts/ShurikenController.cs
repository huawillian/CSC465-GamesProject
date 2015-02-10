using UnityEngine;
using System.Collections;

// Name: Willian Hua
// Date: 11/24/14
// Project: Attack on Ninja, Team Koga
// Description: Shooting Shuriken Controller
// Usage: Must be referenced by Input Manager and Initialized

public class ShurikenController : MonoBehaviour
{
    // Specify direction and speed in while the shurkien will move every update
	public Vector3 direction;
    public float speed = 0.25f;

    // Get Player Object and set the direction depending on the player's facing direction
    public GameObject player;

    // Time to live variable
    // Shuriken will be destroyed after a set amount of time
    public float time;
    public float ttl;

	void Start ()
	{
        time = Time.time;
        ttl = 5.0f;
        player = GameObject.Find("Player");

        if (player.GetComponentInChildren<PlayerController>().isFacingRight)
            direction = new Vector3(speed, 0f, 0f);
        else
            direction = new Vector3(-speed, 0f, 0f);
	}

    // Set Direction
    public void setDirection(Vector3 vect3)
    {
        direction = vect3;
    }

	// Rotate and move Shuriken in given velocity
	void FixedUpdate()
	{
        transform.position += direction;
		transform.Rotate (new Vector3(0,0,30));

        // Destroy after 5 seconds
        if (Time.time - time > ttl)
            Destroy(this.gameObject);
	}

	// Destroy when collide with enemy collider
	void OnTriggerEnter(Collider other)
	{
        // Something is currently the enemy
        // Destroy the enemy and this object
		if (string.Compare (other.name, "Something") == 0)
        {
			Destroy (other.gameObject.transform.parent.gameObject);
			Destroy (this.gameObject);
		}

        // Destroy this object if collides with the platform
        if (string.Compare(other.name, "Platform") == 0)
        {
            Destroy(this.gameObject);
        }

	}
}
