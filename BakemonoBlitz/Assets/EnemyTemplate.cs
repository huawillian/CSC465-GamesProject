using UnityEngine;
using System.Collections;

public class EnemyTemplate : MonoBehaviour
{
    SceneManager mSceneManager;
    GameObject player;

    // Disappearing variables
    float duration = 0.35f;
    float durationInv;
    float timer = 0f;

    // Patrol Coordinates
    GameObject patrolA;
    GameObject patrolB;
    public Vector3 patrolACoordinates;
    public Vector3 patrolBCoordinates;


	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        player = mSceneManager.mPlayerManager.player;

        // Set duration to invisible
        durationInv = 1f / (duration != 0f ? duration : 1f);

        foreach (Transform t in this.transform)
        {
            if (t.name == "PatrolA")
                patrolA = t.gameObject;

            if (t.name == "PatrolB")
                patrolB = t.gameObject;
        }

        patrolACoordinates = patrolA.transform.position;
        patrolBCoordinates = patrolB.transform.position;

        StartCoroutine("StartPatrol");
	}

    // Methods to Patrol the Object
    IEnumerator StartPatrol()
    {
        while (true)
        {
            yield return StartCoroutine(MoveObject(transform, patrolACoordinates, patrolBCoordinates, 3.0f));
            yield return StartCoroutine(MoveObject(transform, patrolBCoordinates, patrolACoordinates, 3.0f));
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

	
	// Update is called once per frame
	void Update ()
    {
	
	}

    IEnumerator DestroyScript()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        StopCoroutine("MoveObject");
        StopCoroutine("StartPatrol");

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
