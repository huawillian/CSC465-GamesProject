using UnityEngine;
using System.Collections;

public class TwoSmallSmokeController : MonoBehaviour
{
    // Disappearing variables
    float duration = 0.3f;
    float durationInv;
    float timer = 0f;

    GameObject smoke1;
    GameObject smoke2;

	// Use this for initialization
	void Start ()
    {
        // Set duration to invisible
        durationInv = 1f / (duration != 0f ? duration : 1f);

        foreach (Transform t in this.transform)
        {
            if (t.name == "Smoke1")
                smoke1 = t.gameObject;

            if (t.name == "Smoke2")
                smoke2 = t.gameObject;
        }

        StartCoroutine("ExitRoutine");
	}
	
	// Update is called once per frame
	void Update ()
    {
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

    IEnumerator ExitRoutine()
    {
        timer = 0.0f;
        float timestampExit = Time.time;

        StartCoroutine(MoveObject(smoke1.transform, this.gameObject.transform.position, new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, this.gameObject.transform.position.z), 0.5f));
        StartCoroutine(MoveObject(smoke2.transform, this.gameObject.transform.position, new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, this.gameObject.transform.position.z), 0.5f));

        while (Time.time - timestampExit < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer * durationInv);
            timer += 0.05f;
            smoke1.renderer.material.color = new Color(smoke1.renderer.material.color.a, smoke1.renderer.material.color.g, smoke1.renderer.material.color.b, alpha);
            smoke2.renderer.material.color = new Color(smoke2.renderer.material.color.a, smoke2.renderer.material.color.g, smoke2.renderer.material.color.b, alpha);

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(this.gameObject);
    }
}
