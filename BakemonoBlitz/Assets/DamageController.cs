﻿using UnityEngine;
using System.Collections;

public class DamageController : MonoBehaviour
{
    SceneManager mSceneManager;

    public float damagedDuration = 0.5f;
    public GameObject collidedEnemy;

	// Use this for initialization
	void Start ()
    {
        this.mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && mSceneManager.mPlayerManager.state != PlayerManager.PlayerState.Dashing && !mSceneManager.mPlayerManager.invincible && mSceneManager.mPlayerManager.state != PlayerManager.PlayerState.Damaged)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collidedEnemy = other.gameObject;
            StartCoroutine("PlayerDamagedScript");
        }
    }

    IEnumerator PlayerDamagedScript()
    {
        PlayerManager.PlayerState originalState = mSceneManager.mPlayerManager.state;

        mSceneManager.mPlayerManager.state = PlayerManager.PlayerState.Damaged;
        mSceneManager.mPlayerManager.health--;
        mSceneManager.mPlayerManager.StartCoroutine("SetInvincible");

        Vector3 posDiff = mSceneManager.mPlayerManager.player.gameObject.transform.position - collidedEnemy.transform.position;

        mSceneManager.mPlayerManager.player.rigidbody2D.AddForce(new Vector2(posDiff.x * 500, posDiff.y * 500));

        yield return new WaitForSeconds(damagedDuration);

        mSceneManager.mPlayerManager.state = originalState;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;

    }
}