using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    SceneManager mSceneManager;


	// Use this for initialization
	void Start ()
    {
        this.mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        mSceneManager.mEnemyManager.removeEnemy(this.gameObject);
    }
}
