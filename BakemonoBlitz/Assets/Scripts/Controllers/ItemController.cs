using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour
{
    SceneManager mSceneManager;

    // Use this for initialization
    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (this.gameObject.name == "Potion") StartCoroutine("PotionStart");
            if (this.gameObject.name == "Sword") StartCoroutine("SwordStart");
            if (this.gameObject.name == "Grapple1") StartCoroutine("GrappleStart");
            if (this.gameObject.name == "Gem") StartCoroutine("GemStart");

            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }
    }

    IEnumerator PotionStart()
    {
        mSceneManager.mPlayerManager.health += 25;

        yield return new WaitForSeconds(0.2f);

        mSceneManager.mItemManager.removeItem(this.gameObject);
        Destroy(this.gameObject);

    }

    IEnumerator SwordStart()
    {
        mSceneManager.mPlayerManager.weapon1 = true;

        yield return new WaitForSeconds(0.2f);

        mSceneManager.mItemManager.removeItem(this.gameObject);
        Destroy(this.gameObject);

    }

    IEnumerator GrappleStart()
    {
        mSceneManager.mPlayerManager.weapon2 = true;

        yield return new WaitForSeconds(0.2f);

        mSceneManager.mItemManager.removeItem(this.gameObject);

        Destroy(this.gameObject);

    }

    IEnumerator GemStart()
    {
        mSceneManager.mPlayerManager.gems += 15;

        yield return new WaitForSeconds(0.2f);

        mSceneManager.mItemManager.removeItem(this.gameObject);

        Destroy(this.gameObject);

    }




}
