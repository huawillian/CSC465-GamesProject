using UnityEngine;
using System.Collections;
using System;

public class GrapplingPlatformController : MonoBehaviour
{
    SceneManager mSceneManager;
    GameObject player;
    GameObject image;
    BoxCollider2D playerTriggerCollider;
    BoxCollider2D platformTriggerCollider;
    BoxCollider2D physicalCollider;

    public bool vicinity;
    public bool physical;
    public bool crumbling;
    public bool playerCollided;

    // Set through Unity editor
    public Sprite greenImage;
    public Sprite yellowImage;
    public Sprite redImage;
    public Sprite greyImage;

    // Disappearing variables
    float duration = 0.35f;
    float durationInv;
    float timer = 0f;

    // Use this for initialization
    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        player = mSceneManager.mPlayerManager.player;

        vicinity = false; // Set by VicinityScript
        physical = true; // Set if we want platform to become physical
        crumbling = false; // Set if player has stepped on platform
        playerCollided = false; // Set if player has collided with the platform

        playerTriggerCollider = player.GetComponent<BoxCollider2D>();
        platformTriggerCollider = this.gameObject.GetComponent<BoxCollider2D>();

        // Get the physical collider and sync it with physical variable
        Transform[] ts = GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "Physical")
            {
                physicalCollider = t.gameObject.GetComponent<BoxCollider2D>();
            }

            if (t.gameObject.name == "Image")
            {
                image = t.gameObject;
            }
        }

        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // Set duration to invisible
        durationInv = 1f / (duration != 0f ? duration : 1f);
    }


    // Update is called once per frame
    void Update()
    {
        if (physical)
        {
            physicalCollider.enabled = true;
        }
        else
        {
            physicalCollider.enabled = false;
        }

        if (vicinity && (playerTriggerCollider.bounds.center.y - playerTriggerCollider.bounds.extents.y < platformTriggerCollider.bounds.center.y))
        {
            physical = false;
        }
        else
        {
            physical = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerCollided = true;
        }

        if (col.gameObject.name == "Grapple" && (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Swinging || mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleExtending || mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleHooked) && !crumbling)
        {
            StartCoroutine("CrumblingExit");
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerCollided = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerCollided = false;
        }
    }

    IEnumerator CrumblingExit()
    {
        crumbling = true;

        if (image.GetComponent<SpriteRenderer>().sprite == yellowImage)
        {
            for (int i = 0; i < 10; i++)
            {
                image.GetComponent<SpriteRenderer>().sprite = greyImage;
                yield return new WaitForSeconds(0.1f);
                image.GetComponent<SpriteRenderer>().sprite = yellowImage;
                yield return new WaitForSeconds(0.1f);
            }

            image.GetComponent<SpriteRenderer>().sprite = redImage;
        }

        for (int i = 0; i < 10; i++)
        {
            image.GetComponent<SpriteRenderer>().sprite = greyImage;
            yield return new WaitForSeconds(0.05f);
            image.GetComponent<SpriteRenderer>().sprite = redImage;
            yield return new WaitForSeconds(0.05f);
        }

        image.GetComponent<SpriteRenderer>().sprite = greyImage;
        yield return new WaitForSeconds(0.5f);

        physicalCollider.enabled = false;

        StartCoroutine("CrumblingExitAnimation");
    }

    IEnumerator CrumblingExitAnimation()
    {
        timer = 0.0f;
        float timestampExit = Time.time;

        StartCoroutine(MoveObject(transform, this.gameObject.transform.position, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 3, this.gameObject.transform.position.z), 1.0f));

        while (Time.time - timestampExit < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer * durationInv);
            timer += 0.03f;
            renderer.material.color = new Color(renderer.material.color.a, renderer.material.color.g, renderer.material.color.b, alpha);
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, timer / duration * 50.0f);
            yield return new WaitForSeconds(0.03f);
        }

        mSceneManager.mStageManager.removeDynamic(this.gameObject);
        Destroy(this.gameObject);

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



}
