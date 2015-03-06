using UnityEngine;
using System.Collections;

public class Clouds : MonoBehaviour
{
    float origX = 0.0f;


	// Use this for initialization
	void Start () {
        origX = this.gameObject.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.rigidbody2D.velocity = new Vector2(-1.0f, 0.0f);

        if (origX - this.transform.position.x > 10.0f) Destroy(this.gameObject);

	}
}
