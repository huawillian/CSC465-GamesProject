using UnityEngine;
using System.Collections;

public class SmokeController : MonoBehaviour
{
    SceneManager mSceneManager;
    GameObject player;

    public GameObject smallSmoke;
    public GameObject twoSmokes;

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        player = mSceneManager.mPlayerManager.player;

        StartCoroutine("SmokeGeneratorStart");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    IEnumerator SmokeGeneratorStart()
    {
        yield return new WaitForSeconds(5.0f);

        while (true)
        {
            if (mSceneManager.mPlayerManager.state == PlayerManager.PlayerState.WallSliding)
            {
                if (player.GetComponent<PlayerController>().WallCollideRight)
                {
                    Instantiate(smallSmoke, new Vector3(player.transform.position.x + player.collider2D.bounds.extents.x, player.transform.position.y + player.collider2D.bounds.extents.y, 0), Quaternion.identity);
                }
                else
                {
                    Instantiate(smallSmoke, new Vector3(player.transform.position.x - player.collider2D.bounds.extents.x, player.transform.position.y + player.collider2D.bounds.extents.y, 0), Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator TwoSmokeGeneratorStart()
    {
        Instantiate(twoSmokes, new Vector3(player.transform.position.x, player.transform.position.y - player.collider2D.bounds.extents.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
    }


}
