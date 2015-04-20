using UnityEngine;
using System.Collections;
using System;

public class CrumblingPlatformController : MonoBehaviour
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

        if (playerTriggerCollider.bounds.center.y - playerTriggerCollider.bounds.extents.y > platformTriggerCollider.bounds.center.y &&
            playerTriggerCollider.bounds.center.y - playerTriggerCollider.bounds.extents.y < platformTriggerCollider.bounds.center.y + platformTriggerCollider.bounds.extents.y &&
            playerCollided &&
            !crumbling &&
            (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Idling || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Falling || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Running || mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Damaged))
        {
            StartCoroutine("CrumblingExit");
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerCollided = true;
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

        if (image.GetComponent<SpriteRenderer>().sprite == greenImage)
        {
            image.GetComponent<SpriteRenderer>().sprite = yellowImage;

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

        mSceneManager.mStageManager.removeDynamic(this.gameObject);

        Destroy(this.gameObject);

    }



}
