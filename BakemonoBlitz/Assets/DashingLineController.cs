using UnityEngine;
using System.Collections;
using System;

public class DashingLineController : MonoBehaviour
{
    SceneManager mSceneManager;
    GameObject player;

    public bool dashStarted = false;
    public GameObject line;

    private GameObject tempLine;

    // Disappearing variables
    float duration = 0.35f;
    float durationInv;
    float timer = 0f;

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        player = mSceneManager.mPlayerManager.player;

        // Set duration to invisible
        durationInv = 1f / (duration != 0f ? duration : 1f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        try
        {
            if (dashStarted)
            {
                tempLine.GetComponent<LineRenderer>().SetPosition(1, player.transform.position);
                tempLine.GetComponent<LineRenderer>().renderer.enabled = true;
            }

            if (dashStarted && mSceneManager.mPlayerManager.state != PlayerManager.PlayerState.Dashing)
            {
                dashStarted = false;
                StartCoroutine("DashAnimationEnd");
            }
        }
        catch (Exception e)
        {
        }
	}

    IEnumerator DashAnimationStart()
    {
        if (!dashStarted)
        {
            dashStarted = true;
        }

        tempLine = (GameObject) Instantiate(line,Vector3.zero, Quaternion.identity);
        tempLine.GetComponent<LineRenderer>().renderer.enabled = false;
        tempLine.GetComponent<LineRenderer>().SetPosition(0, player.transform.position);

        yield return new WaitForSeconds(0.05f);
    }

    IEnumerator DashAnimationEnd()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("DashingLine");

        timer = 0.0f;
        float timestampExit = Time.time;

        while (Time.time - timestampExit < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer * durationInv);
            timer += 0.03f;

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].gameObject.renderer.material.color = new Color(lines[i].gameObject.renderer.material.color.a, lines[i].gameObject.renderer.material.color.g, lines[i].gameObject.renderer.material.color.b, alpha);
            }

            yield return new WaitForSeconds(0.03f);
        }

        int length = lines.Length;
        for (int i = 0; i < length; i++)
        {
            Destroy(lines[i].gameObject);
        }
    }

}
