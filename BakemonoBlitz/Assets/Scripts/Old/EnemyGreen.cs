using UnityEngine;
using System.Collections;

// Name: Willian Hua
// Date: 11/24/14
// Project: Attack on Ninja, Team Koga
// Description: Controls Green Enemy, Patrols, and Movement towards player on sight
// Usage: Used in EnemyGreen Prefab object

public class EnemyGreen : MonoBehaviour
{
	// Patrol Coordinates
	GameObject patrolA;
	GameObject patrolB;
	public Vector3 patrolACoordinates;
	public Vector3 patrolBCoordinates;

	// Player Found
	public bool playerFound;
	GameObject player;
	public float speed;

    public float maxSpeed = 25.0f;

	void Awake ()
	{
		foreach (Transform t in this.transform)
		{
			if(t.name == "PatrolA")
				patrolA = t.gameObject;

			if(t.name == "PatrolB")
				patrolB = t.gameObject;
		}

		patrolACoordinates = patrolA.transform.position;
		patrolBCoordinates = patrolB.transform.position;

		playerFound = false;
		player = GameObject.Find("Player");
		speed = 350f;
	}

	// Methods to Patrol the Object
	IEnumerator Start () 
	{
		while (true) 
		{
			yield return StartCoroutine(MoveObject(transform, patrolACoordinates, patrolBCoordinates, 3.0f));
			if(playerFound)
				break;

			yield return StartCoroutine(MoveObject(transform, patrolBCoordinates, patrolACoordinates, 3.0f));
			if(playerFound)
				break;
		}
	}
	
	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) 
	{
		// Patrol only if player hasn't been found
			double i = 0.0d;
			double rate = 1.0d / time;
			while (i < 1.0d)
			{
				i += Time.deltaTime * rate;
				thisTransform.position = Vector3.Lerp (startPos, endPos, (float)i);
				yield return null;
			}
	}

	void OnTriggerEnter(Collider other)
	{
		if (string.Compare(other.gameObject.name, "Player") == 0)
		{
			playerFound = true;
			InvokeRepeating("MoveToPlayer", 1, 1.0f);
		}
	}

	void MoveToPlayer()
	{
		rigidbody.AddForce((player.transform.position - transform.position) * speed * Time.smoothDeltaTime);
	}

    void Update()
    {
        // Set maximum speed 
        if (Mathf.Abs(rigidbody.velocity.x) > maxSpeed || Mathf.Abs(rigidbody.velocity.y) > maxSpeed)
        {
            Vector3 newVelocity = rigidbody.velocity.normalized;
            newVelocity *= maxSpeed;
            rigidbody.velocity = newVelocity;
        }
    }
}