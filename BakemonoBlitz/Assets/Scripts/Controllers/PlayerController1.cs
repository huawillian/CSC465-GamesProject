using UnityEngine;
using System.Collections;

public class PlayerController1 : MonoBehaviour
{
	SpriteRenderer renderer;	
	public bool isFacingRight;
	public bool isGrounded;
	public bool isMoving;
	public bool isSpeeding;
    public bool isSwinging;
    public bool hasCollided;
	public float movinSpeed;
	public float movin;
	RigidbodyConstraints constraints;

	// FINE TUNING VARIABLES
	public float maxSpeed = 25.0f;
	public float groundCheckingDistance = 0.5f;
	public float playerMovingMinimumValue = 1.0f;
	public float playerSpeedingMinimumValue = 10.0f;
	public float maxBrakeTime = 0.25f;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    public Sprite sprite6;
    public Sprite sprite7;

    public bool Idle = false;
    public bool Running = false;
    public bool Jumping = false;
    public bool Falling = false;
    public bool Chaining = false;
    public bool Swinging = false;
    public bool Damaged = false;

	void Awake()
	{
        renderer = GameObject.Find("Ninja_Male").GetComponent<SpriteRenderer>();
        renderer.sprite = sprite1;
		isFacingRight = true;
		isGrounded = true;
		isMoving = false;
		isSpeeding = false;
		movin = 0f;
		movinSpeed = 0f;
		constraints = rigidbody.constraints;
        playerMovingMinimumValue = 1.0f;
	}

	void Update()
	{
		// Freeze z axis movement
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Set player sprite
        setPlayerSprite();
    }
	
	void FixedUpdate()
	{
		// Set maximum speed 
		if(Mathf.Abs(rigidbody.velocity.x) > maxSpeed || Mathf.Abs(rigidbody.velocity.y) > maxSpeed)
		{
			Vector3 newVelocity = rigidbody.velocity.normalized;
			newVelocity *= maxSpeed;
			rigidbody.velocity = newVelocity;
		}

		// Check if player is grounded
		isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckingDistance);

		// Check if player is moving
		if (rigidbody.velocity.magnitude > playerMovingMinimumValue || !isGrounded) 
		{
			isMoving = true;
		} 
		else
		{
			isMoving = false;
		}

		// Check if player is speeding
		if (rigidbody.velocity.x > playerSpeedingMinimumValue && rigidbody.velocity.x > movinSpeed)
		{
			isSpeeding = true;
			movinSpeed = rigidbody.velocity.magnitude;
		}

		// Check if player is braking
		if (isSpeeding) 
		{
			if (rigidbody.velocity.x < playerMovingMinimumValue && isGrounded)
			{
				isSpeeding = false;
				rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
				Invoke("UnFreezePlayer", (movinSpeed / maxSpeed * maxBrakeTime));
				movinSpeed = 0;
			}
		}

	}
	
	void UnFreezePlayer()
	{
		rigidbody.constraints = constraints;
	}

    void setPlayerSprite()
    {
        // Change Renderer based on input
        if (Input.GetAxis("Left Analog Stick X") > 0 || Input.GetKeyDown(KeyCode.D))
        {
            renderer.transform.localEulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }

        if (Input.GetAxis("Left Analog Stick X") < 0 || Input.GetKeyDown(KeyCode.A))
        {
            renderer.transform.localEulerAngles = new Vector3(0, 180, 0);
            isFacingRight = false;
        }

        if (!isMoving)
        {
            renderer.sprite = sprite1;
        }

        if (Mathf.Abs(Input.GetAxis("Left Analog Stick X")) > 0 || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            renderer.sprite = sprite2;
        }

        if (!isGrounded && rigidbody.velocity.y > 0)
        {
            renderer.sprite = sprite3;
        }

        if (!isGrounded && rigidbody.velocity.y < 0)
        {
            renderer.sprite = sprite4;
        }

        if (Chaining)
        {
            renderer.sprite = sprite6;
        }

        if (Swinging)
        {
            renderer.sprite = sprite5;
        }

        if (Damaged)
        {
            renderer.sprite = sprite7;
        }

    }


    IEnumerator UnFreezeDamaged()
    {
        yield return new WaitForSeconds(0.5f);
        Damaged = false;

    }




/*
 * Player Health Bar, still working on it...
 * * * * * * * * * * * * * */

	float Health = 100f;
	Rect position = new Rect (10,10,200,20);
	
	void OnGUI()
	{
		GUI.backgroundColor = Color.green;
		GUI.HorizontalScrollbar(position, 0, Health,0, 100);
	}

	void OnTriggerEnter(Collider other)
	{
		if (string.Compare (other.name, "Something") == 0)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(other.gameObject.transform.parent.gameObject.rigidbody.velocity*50);
			other.gameObject.transform.parent.gameObject.rigidbody.AddForce(-other.gameObject.transform.parent.gameObject.rigidbody.velocity*100);
			Health -= 20;
            Damaged = true;
            StartCoroutine("UnFreezeDamaged");
		}

        if (string.Compare(other.name, "Platform") == 0)
        {
            hasCollided = true;
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (string.Compare(other.name, "Platform") == 0)
        {
            hasCollided = false;
        }
    }
}
