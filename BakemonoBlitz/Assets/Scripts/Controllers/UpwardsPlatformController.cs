using UnityEngine;
using System.Collections;

public class UpwardsPlatformController : MonoBehaviour
{
    SceneManager mSceneManager;
    BoxCollider2D []boxColliders;
    public float height = 0.64f;

    // Use this for initialization
    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

        boxColliders = this.gameObject.transform.GetComponentsInChildren<BoxCollider2D>();
        boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && this.renderer.bounds.max.y - 0.1f > col.renderer.bounds.min.y)
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
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            boxColliders[boxColliders.Length - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

}
