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
            if (parentObject.name == "UpwardsPlatform")
            {
                parentObject.GetComponent<UpwardsPlatformController>().vicinity = true;
            }

            if (parentObject.name == "PlatformTemplate")
            {
                parentObject.GetComponent<PlatformTemplate>().vicinity = true;
            }

            if (parentObject.name == "MovingPlatform")
            {
                parentObject.GetComponent<MovingPlatformController>().vicinity = true;
            }

            if (parentObject.name == "CrumblingPlatform")
            {
                parentObject.GetComponent<CrumblingPlatformController>().vicinity = true;
            }

            if (parentObject.name == "GrapplingPlatform")
            {
                parentObject.GetComponent<GrapplingPlatformController>().vicinity = true;
            }

            if (parentObject.name == "Kappa")
            {
                parentObject.GetComponent<KappaController>().playerFound = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (parentObject.name == "UpwardsPlatform")
            {
                parentObject.GetComponent<UpwardsPlatformController>().vicinity = true;
            }

            if (parentObject.name == "PlatformTemplate")
            {
                parentObject.GetComponent<PlatformTemplate>().vicinity = true;
            }

            if (parentObject.name == "MovingPlatform")
            {
                parentObject.GetComponent<MovingPlatformController>().vicinity = true;
            }

            if (parentObject.name == "CrumblingPlatform")
            {
                parentObject.GetComponent<CrumblingPlatformController>().vicinity = true;
            }

            if (parentObject.name == "GrapplingPlatform")
            {
                parentObject.GetComponent<GrapplingPlatformController>().vicinity = true;
            }

            if (parentObject.name == "Kappa")
            {
                parentObject.GetComponent<KappaController>().playerFound = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (parentObject.name == "UpwardsPlatform")
            {
                parentObject.GetComponent<UpwardsPlatformController>().vicinity = false;
            }

            if (parentObject.name == "PlatformTemplate")
            {
                parentObject.GetComponent<PlatformTemplate>().vicinity = false;
            }

            if (parentObject.name == "MovingPlatform")
            {
                parentObject.GetComponent<MovingPlatformController>().vicinity = false;
            }

            if (parentObject.name == "CrumblingPlatform")
            {
                parentObject.GetComponent<CrumblingPlatformController>().vicinity = false;
            }

            if (parentObject.name == "GrapplingPlatform")
            {
                parentObject.GetComponent<GrapplingPlatformController>().vicinity = false;
            }
        }
    }
}
