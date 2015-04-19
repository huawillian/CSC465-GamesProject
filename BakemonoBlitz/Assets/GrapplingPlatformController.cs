using UnityEngine;
using System.Collections;
using System;

public class GrapplingPlatformController : MonoBehaviour
{
    SceneManager mSceneManager;
    BoxCollider2D[] boxColliders;
    public float height = 0.64f;

    public bool exiting = false;

    // Use this for initialization
    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

        boxColliders = this.gameObject.transform.GetComponentsInChildren<BoxCollider2D>();
        boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Grapple" && (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.Swinging || mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleExtending || mSceneManager.mPlayerManager.grappleState == PlayerManager.GrappleState.GrappleHooked))
        {
            StartCoroutine("CrumblingExit");
        }

        if (col.gameObject.tag == "Player" && this.renderer.bounds.max.y - 0.1f > col.renderer.bounds.min.y)
        {
            boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (col.gameObject.tag == "Grapple" && this.renderer.bounds.max.y - 0.1f > col.renderer.bounds.min.y)
        {
            boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && this.renderer.bounds.max.y - 0.1f > col.renderer.bounds.min.y)
        {
            boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (col.gameObject.tag == "Grapple" && this.renderer.bounds.max.y - 0.1f > col.renderer.bounds.min.y)
        {
            boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    IEnumerator CrumblingExit()
    {
        for (int i = 0; i < 10; i++)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 10; i++)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        mSceneManager.mStageManager.removeDynamic(this.gameObject);

        Destroy(this.gameObject);

    }
}
