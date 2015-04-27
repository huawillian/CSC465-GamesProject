using UnityEngine;
using System.Collections;

public class FlamingSkullController : MonoBehaviour
{
    SceneManager mSceneManager;

    // Disappearing variables
    float duration = 0.35f;
    float durationInv;
    float timer = 0f;

    float bounceDelay = 2.0f;

    int bounceIncrement = 0;
    float frac = 0;
    float timestampStart = 0.0f;

    // Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

        // Set duration to invisible
        durationInv = 1f / (duration != 0f ? duration : 1f);

         StartCoroutine("StartBouncing");

        pos1 = this.gameObject.transform.position;
        pos2 = new Vector3(this.gameObject.transform.position.x - 3, this.gameObject.transform.position.y, 0);
        pos3 = new Vector3(this.gameObject.transform.position.x + 3, this.gameObject.transform.position.y, 0);

	}

    void Update()
    {
        switch (bounceIncrement)
        {
            case 0:
                currentStartPos = pos1;
                currentEndPos = pos2;
                this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                break;
            case 1:
                currentStartPos = pos2;
                currentEndPos = pos1;
                this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                break;
            case 2:
                currentStartPos = pos1;
                currentEndPos = pos3;
                this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                break;
            case 3:
                currentStartPos = pos3;
                currentEndPos = pos1;
                this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                break;
            default:
                break;
        }


        frac = (Time.time - timestampStart) / bounceDelay;

        MoveObjectTo(frac);

    }

    IEnumerator StartBouncing()
    {
        while (true)
        {
                yield return new WaitForSeconds(bounceDelay);

                timestampStart = Time.time;
                bounceIncrement++;
                bounceIncrement = bounceIncrement % 4;
        }

      
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        // Patrol only if player hasn't been found
        double i = 0.0d;
        double rate = 1.0d / time;
        while (i < 1.0d)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, (float)i);
            yield return null;
        }
    }

    Vector3 pos1, pos2, pos3;
    Vector3 currentStartPos, currentEndPos;

    void MoveObjectTo(float fFraction)
    {
        Vector3 v3Delta = currentEndPos - currentStartPos;
        Vector3 v3Pos = currentStartPos;
        v3Pos.x += v3Delta.x * fFraction;
        v3Pos.y += v3Delta.y * fFraction + Mathf.Sin(fFraction * Mathf.PI) * 2.5f;
        v3Pos.z += v3Delta.z * fFraction + Mathf.Sin(fFraction * Mathf.PI) * 2.5f;
        transform.position = v3Pos;
    }

    IEnumerator DestroyScript()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        timer = 0.0f;
        float timestampExit = Time.time;

        mSceneManager.mSoundManager.playSound("coin", mSceneManager.mCameraManager.getCamera("Main Camera").transform.position);
        StartCoroutine(MoveObject(transform, this.gameObject.transform.position, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 2, this.gameObject.transform.position.z), 1.0f));

        while (Time.time - timestampExit < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer * durationInv);
            timer += 0.05f;
            renderer.material.color = new Color(renderer.material.color.a, renderer.material.color.g, renderer.material.color.b, alpha);
            yield return new WaitForSeconds(0.05f);
        }

        mSceneManager.mEnemyManager.removeEnemy(this.gameObject);
        Destroy(this.gameObject);
    }

}
