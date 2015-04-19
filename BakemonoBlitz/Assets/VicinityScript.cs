using UnityEngine;
using System.Collections;

public class VicinityScript : MonoBehaviour
{
    GameObject parentObject;

	// Use this for initialization
	void Start ()
    {
        parentObject = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            parentObject.GetComponent<PlatformTemplate>().vicinity = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            parentObject.GetComponent<PlatformTemplate>().vicinity = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            parentObject.GetComponent<PlatformTemplate>().vicinity = false;
        }
    }
}
