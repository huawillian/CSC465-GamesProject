using UnityEngine;
using System.Collections;
using System;

public class DashController : MonoBehaviour
{
    SceneManager mSceneManager;
    CircleCollider2D bigCollider;
    BoxCollider2D smallCollider;

    public enum DashState { DashReady, DashShort, DashLong, DashExit, DashCooldown};
    public DashState dashState;

    float dashSpeedShort;
    float dashLongCoeff;
    float dashCooldown;
    float dashExitPauseDuration;
    float dashDurationShort;
    float dashDurationLong;

    bool initialGrounded;

    float dashAngle;
    Vector2 dashDirection;
    float dashTimestamp;

    int chainedDashHits;

	// Use this for initialization
	void Start ()
    {
        this.mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        bigCollider = this.gameObject.GetComponent<CircleCollider2D>();
        smallCollider = this.gameObject.GetComponent<BoxCollider2D>();

        dashState = DashState.DashReady;

        dashSpeedShort = 12.0f;
        dashLongCoeff = 1.8f;
        dashCooldown = 3.0f;
        dashExitPauseDuration = 0.5f;
        dashDurationShort = 0.15f;
        dashDurationLong = 0.3f;

        initialGrounded = true;

        dashAngle = 0.0f;
        dashTimestamp = 0.0f;
        dashDirection = Vector2.zero;

        chainedDashHits = 0;
	
	}

    public IEnumerator Dash()
    {
        if (dashState == DashState.DashReady)
        {
            dashState = DashState.DashShort;
        }
        else if (dashState == DashState.DashLong)
        {
            mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity = Vector2.zero;
            dashState = DashState.DashShort;
        }
        
        initialGrounded = mSceneManager.mPlayerManager.playerController.GroundCollide;
        dashDirection = new Vector2(mSceneManager.mPlayerManager.LX, mSceneManager.mPlayerManager.LY);

        // Set up dash direction depending on grounded
        // Get dash Angle
        // Use angle to get dash direction for x and y coordinates
        if (Math.Abs(dashDirection[0]) < 0.01f && Math.Abs(dashDirection[1]) < 0.01f)
        {
            // If no input, the dash direction depends on player facing direction
            if (mSceneManager.mPlayerManager.playerController.FaceRight) dashDirection = new Vector2(dashSpeedShort, 0.0f);
            else dashDirection = new Vector2(dashSpeedShort * -1.0f, 0.0f);
        }
        else
        if (initialGrounded)
        {
            dashAngle = Mathf.Atan2(dashDirection[0], dashDirection[1]);

            // Transform the radians to standard
            dashAngle -= Mathf.PI / 2.0f;
            if (dashAngle < Mathf.PI * -1.0f) dashAngle = dashAngle + Mathf.PI * 2.0f;
            dashAngle = dashAngle * -1.0f;
            if (dashAngle < 0.0f) dashAngle = dashAngle + Mathf.PI * 2.0f;

            if(dashAngle >= Mathf.PI && dashAngle <= Mathf.PI * 2.0f)
            {
                // If in lower quadrant, then dash depending on angle
                if (dashAngle > 3.0f * Mathf.PI / 2.0f) dashDirection = new Vector2(dashSpeedShort * 1.0f, 0.0f);
                else dashDirection = new Vector2(dashSpeedShort * -1.0f, 0.0f);

                if (Math.Abs(dashAngle - 3 * Mathf.PI / 2) < 0.3f)
                {
                    // If almost downwards, then dash depend on player direction
                    if (mSceneManager.mPlayerManager.playerController.FaceRight) dashDirection = new Vector2(dashSpeedShort, 0.0f);
                    else dashDirection = new Vector2(dashSpeedShort * -1.0f, 0.0f);
                }
            }
            else
            {
                dashDirection[0] = Mathf.Cos(dashAngle) * dashSpeedShort * 1.0f;
                dashDirection[1] = Mathf.Sin(dashAngle) * dashSpeedShort * 1.0f;
            }
        }
        else
        {
            dashAngle = Mathf.Atan(Math.Abs(mSceneManager.mPlayerManager.LY) / Math.Abs(mSceneManager.mPlayerManager.LX));

            if (dashDirection[0] < 0.0f)
            {
                dashDirection[0] = Mathf.Cos(dashAngle) * dashSpeedShort * -1.0f;
                mSceneManager.mPlayerManager.playerController.FaceRight = false;
            }
            else
            {
                dashDirection[0] = Mathf.Cos(dashAngle) * dashSpeedShort;
                mSceneManager.mPlayerManager.playerController.FaceRight = true;
            }

            if (dashDirection[1] < 0.0f)
            {
                dashDirection[1] = Mathf.Sin(dashAngle) * dashSpeedShort * -1.0f;
            }
            else
            {
                dashDirection[1] = Mathf.Sin(dashAngle) * dashSpeedShort;
            }
        }

        // Set velocity depending on current velocity
        if (dashDirection[0] > 0.0f && dashDirection[0] < mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.x) dashDirection[0] = mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.x + 5.0f;
        if (dashDirection[0] < 0.0f && dashDirection[0] > mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.x) dashDirection[0] = mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.x - 5.0f;

        // Reset Player Velocity
        mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity = Vector2.zero;

        // Start dash animation
        dashTimestamp = Time.time;
        mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Dashing;
        float dashDuration = dashDurationShort;

        while(Time.time - dashTimestamp < dashDuration)
        {
            mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity = dashDirection;

            if (mSceneManager.mPlayerManager.playerController.EnemyCollide && dashState != DashState.DashLong)
            {
                mSceneManager.mPlayerManager.playerController.EnemyCollide = false;
                dashState = DashState.DashLong;
                dashDirection = dashDirection * dashLongCoeff;
                dashDuration = dashDurationLong;

                chainedDashHits++;
            }
            else if (mSceneManager.mPlayerManager.playerController.EnemyCollide)
            {
                mSceneManager.mPlayerManager.playerController.EnemyCollide = false;

                chainedDashHits++;
            }

            yield return new WaitForSeconds(0.005f);
        }

        if (dashState == DashState.DashShort)
        {
            mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity = new Vector2(mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.x, mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.y * 0.5f);
        }
        else if (dashState == DashState.DashLong)
        {
            float dashExitTimestamp = Time.time;
            dashState = DashState.DashExit;

            while (Time.time - dashExitTimestamp < dashExitPauseDuration)
            {
                mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity = new Vector2(0.0f, -0.5f);
                yield return new WaitForSeconds(0.005f);
            }
        }
        else
        {
            mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity = new Vector2(mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.x, mSceneManager.mPlayerManager.playerController.gameObject.rigidbody2D.velocity.y * 0.5f);
        }

        // Start cooldowns
        dashState = DashState.DashCooldown;
        mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Idling;
        float dashCooldownTimestamp = Time.time;

        while (Time.time - dashCooldownTimestamp < dashCooldown)
        {
            yield return new WaitForSeconds(0.01f);
        }

        // Go to ready after cooldown is done
        dashState = DashState.DashReady;

    }

	// Update is called once per frame
	void Update ()
    {
        // Change collider size depending on dash state
        if (dashState == DashState.DashShort)
        {
            bigCollider.enabled = true;
            smallCollider.enabled = false;
        }
        else if (dashState == DashState.DashLong)
        {
            bigCollider.enabled = false;
            smallCollider.enabled = true;
        }

        // Reset enemies killed in current dash chain
        if (dashState == DashState.DashReady && chainedDashHits != 0)
        {
            mSceneManager.mPlayerManager.score += chainedDashHits * chainedDashHits * 10;
            chainedDashHits = 0;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Dashing && !mSceneManager.mPlayerManager.playerController.EnemyCollide)
        {
            if (other.gameObject.name == "GreenFloatingHead")
            {
                mSceneManager.mPlayerManager.score += 10;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<GreenFloatingHeadController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "RedFloatingHead")
            {
                mSceneManager.mPlayerManager.score += 20;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<RedFloatingHeadController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "Kappa")
            {
                mSceneManager.mPlayerManager.score += 50;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<KappaController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "FlamingSkull")
            {
                mSceneManager.mPlayerManager.score += 30;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<FlamingSkullController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "AggressiveFloatingHead")
            {
                mSceneManager.mPlayerManager.score += 25;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<AggressiveFloatingHeadController>().StartCoroutine("DestroyScript");
            }
        }

        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state != PlayerManager.PlayerState.Dashing)
        {
            mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Dashing && !mSceneManager.mPlayerManager.playerController.EnemyCollide)
        {
            if (other.gameObject.name == "GreenFloatingHead")
            {
                mSceneManager.mPlayerManager.score += 10;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<GreenFloatingHeadController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "RedFloatingHead")
            {
                mSceneManager.mPlayerManager.score += 20;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<RedFloatingHeadController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "Kappa")
            {
                mSceneManager.mPlayerManager.score += 50;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<KappaController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "FlamingSkull")
            {
                mSceneManager.mPlayerManager.score += 30;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<FlamingSkullController>().StartCoroutine("DestroyScript");
            }

            if (other.gameObject.name == "AggressiveFloatingHead")
            {
                mSceneManager.mPlayerManager.score += 25;
                mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
                other.gameObject.GetComponent<AggressiveFloatingHeadController>().StartCoroutine("DestroyScript");
            }
        }

        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state != PlayerManager.PlayerState.Dashing)
        {
            mSceneManager.mPlayerManager.playerController.EnemyCollide = true;
        }
    }
}
