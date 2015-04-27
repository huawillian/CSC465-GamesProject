using UnityEngine;
using System.Collections;

public class SmallSmokeController : MonoBehaviour
{
    // Disappearing variables
    float duration = 0.75f;
    float durationInv;
    float timer = 0f;

	// Use this for initialization
	void Start ()
    {
        // Set duration to invisible
        durationInv = 1f / (duration != 0f ? duration : 1f);

        StartCoroutine("DestroyScript");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    IEnumerator DestroyScript()
    {
        timer = 0.0f;
        float timestampExit = Time.time;

        while (Time.time - timestampExit < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer * durationInv);
            timer += 0.05f;
            renderer.material.color = new Color(renderer.material.color.a, renderer.material.color.g, renderer.material.color.b, alpha);
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(this.gameObject);
    }
}
