using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour
{
    SceneManager mSceneManager;


    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (this.gameObject.name == "EventExit") StartCoroutine("ExitStart");
            if (this.gameObject.name == "EventSpeech") StartCoroutine("SpeechStart");

            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }

    }


    IEnumerator ExitStart()
    {
        mSceneManager.mCameraManager.beginFadeOut();
        mSceneManager.state = SceneManager.SceneState.Animating;
        yield return new WaitForSeconds(3.0f);
        Application.LoadLevel("MainMenuScene");

        mSceneManager.mStageManager.removeEvent(this.gameObject);

        Destroy(this.gameObject);

    }

    IEnumerator SpeechStart()
    {

        mSceneManager.state = SceneManager.SceneState.Locked;
        mSceneManager.mHUDManager.addTextToQueue("You have touched Event Speech!");
        mSceneManager.mHUDManager.addTextToQueue("Now releasing Lock!");

        while (mSceneManager.mHUDManager.texts.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        mSceneManager.state = SceneManager.SceneState.Playing;

        mSceneManager.mStageManager.removeEvent(this.gameObject);

        Destroy(this.gameObject);

    }
}
