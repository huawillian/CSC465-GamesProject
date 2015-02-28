using UnityEngine;
using System.Collections;

public class UpwardsPlatformController : MonoBehaviour {
    SceneManager mSceneManager;


    // Use this for initialization
    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.transform.position.y < this.gameObject.transform.position.y)
        {
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

}
