using UnityEngine;
using System.Collections;

public class Animation : MonoBehaviour {

	Animator anim;

	public bool run, idle, jump, swing, wallSlide, damaged, falling, dashing, wasRunning, runJumpPossible, runJump, grapple, jumpGrapple;

	private SceneManager mSceneManager;

	private PlayerController mPlayerController;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
		mPlayerController = mSceneManager.mPlayerManager.playerController;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Idling)
		{
			idle = true;
			anim.SetBool("idle", true);
		}
		else 
		{
			idle = false;
			anim.SetBool("idle", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Running)
		{
			run = true;
			wasRunning = true; // Set this variable to false after 1/2 second
			anim.SetBool("run", true);
		}
		else 
		{
			run = false;
			anim.SetBool("run", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Jumping)
		{
			jump = true;
			anim.SetBool("jump", true);
		}
		else 
		{
			jump = false;
			anim.SetBool("jump", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Falling)
		{
			falling = true;
			anim.SetBool("falling", true);
		}
		else 
		{
			falling = false;
			anim.SetBool("falling", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.WallSliding)
		{
			wallSlide = true;
			anim.SetBool("wallSlide", true);
		}
		else 
		{
			wallSlide = false;
			anim.SetBool("wallSlide", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.WallSliding)
		{
			wallSlide = true;
			anim.SetBool("wallSlide", true);
		}
		else 
		{
			wallSlide = false;
			anim.SetBool("wallSlide", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Swinging)
		{
			swing = true;
			anim.SetBool("swing", true);
		}
		else 
		{
			swing = false;
			anim.SetBool("swing", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Dashing)
		{
			dashing = true;
			anim.SetBool("dashing", true);
		}
		else 
		{
			dashing = false;
			anim.SetBool("dashing", false);
		}
		if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Damaged)
		{
			damaged = true;
			anim.SetBool("damaged", true);
		}
		else 
		{
			damaged = false;
			anim.SetBool("damaged", false);
		}


		//Check if exited the running state
		if(run != true && wasRunning == true)
		{
			wasRunning = false;
			runJumpPossible = true;
			StartCoroutine("UnsetRunJumpPossible");
		}

		//Check runJump state
		if(runJumpPossible == true && jump == true && Mathf.Abs(mPlayerController.gameObject.rigidbody2D.velocity.x) > 5.0f)
		{
			runJump = true;
			anim.SetBool("runJump", true);
		}
		else
		{
			runJump = false;
			anim.SetBool("runJump", false);
		}

		//Check grappling animation states
		if(mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleExtending)
		{
			if(idle == true)
			{
				grapple = true;
				anim.SetBool("grapple", true);
			}
			else
			{
				grapple = false;
				anim.SetBool("grapple", false);
			}
			if(mPlayerController.GroundCollide != true)
			{
				jumpGrapple = true;
				anim.SetBool("jumpGrapple", true);
			}
			else
			{
				jumpGrapple = false;
				anim.SetBool("jumpGrapple", false);
			}
		}
		else
		{
			grapple = false;
			anim.SetBool("grapple", false);
			jumpGrapple = false;
			anim.SetBool("jumpGrapple", false);
		}
	}

	public IEnumerator UnsetRunJumpPossible()
	{
		yield return new WaitForSeconds(0.5f);
			runJumpPossible = false;
	}
}
