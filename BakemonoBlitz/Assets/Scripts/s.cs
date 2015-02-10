using UnityEngine;
using System.Collections;

public class s : MonoBehaviour {

    private float timer = 0.0f;
    private float translationTime = 5.0f;
    private Vector3 startPos;
    private Vector3 endPos = new Vector3(5, 0.5f, 0);

    public bool hasCollided;

	// Use this for initialization
	void Start ()
    {
        startPos = this.gameObject.transform.position;
        hasCollided = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!hasCollided)
        {
            Vector3 pos = Vector3.Slerp(startPos, endPos, timer / translationTime);
            transform.position = new Vector3(pos.x, -pos.y + 2 * startPos.y, pos.z);
            timer += Time.deltaTime;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (string.Compare(other.name, "Platform") == 0)
        {
            hasCollided = true;
        }
    }
}
