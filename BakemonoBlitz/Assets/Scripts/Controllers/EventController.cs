using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour
{
    SceneManager mSceneManager;

    // Variables used by Level Complete script
    public bool displayLevelComplete = false;
    public bool levelComplete1 = false;
    public bool levelComplete2 = false;
    public bool levelComplete3 = false;
    public int tempTimer = 0;

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
    }

    void OnGUI()
    {
        if (displayLevelComplete)
        {
            // Make a background box
            GUI.backgroundColor = Color.clear;
            GUI.color = Color.white;
            GUI.skin.box.fontSize = 50;
            GUI.skin.box.alignment = TextAnchor.UpperLeft;
            GUI.skin.box.fontStyle = FontStyle.Bold;

            if (levelComplete1)
            {
                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(30),
                    mSceneManager.mHUDManager.getPositionY(25), 
                    mSceneManager.mHUDManager.getPositionX(90),
                    mSceneManager.mHUDManager.getPositionY(40)), 
                    "LEVEL COMPLETED!");
            }
            else if(levelComplete2)
            {
                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(40),
                    mSceneManager.mHUDManager.getPositionY(20),
                    mSceneManager.mHUDManager.getPositionX(90),
                    mSceneManager.mHUDManager.getPositionY(40)),
                    "BONUS");

                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(30),
                    mSceneManager.mHUDManager.getPositionY(35),
                    mSceneManager.mHUDManager.getPositionX(90),
                    mSceneManager.mHUDManager.getPositionY(40)),
                    "TIMER: " + tempTimer);

                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(30),
                    mSceneManager.mHUDManager.getPositionY(50),
                    mSceneManager.mHUDManager.getPositionX(90),
                    mSceneManager.mHUDManager.getPositionY(40)),
                    "GEMS: " + mSceneManager.mPlayerManager.gems);

                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(30),
                    mSceneManager.mHUDManager.getPositionY(65),
                    mSceneManager.mHUDManager.getPositionX(90),
                    mSceneManager.mHUDManager.getPositionY(40)),
                    "SCORE: " + mSceneManager.mPlayerManager.score);

            }
            else if (levelComplete3)
            {
                GUI.skin.box.fontSize = 100;
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;

                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(0),
                    mSceneManager.mHUDManager.getPositionY(0),
                    mSceneManager.mHUDManager.getPositionX(100),
                    mSceneManager.mHUDManager.getPositionY(100)),
                    "SCORE: " + mSceneManager.mPlayerManager.score);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (this.gameObject.name == "EventExit")
            {
                StartCoroutine("ExitStart");
            }

            if (this.gameObject.name == "EventSpeech")
            {
                StartCoroutine("SpeechStart");
            }

            if (this.gameObject.name == "EventNext")
            {
                StartCoroutine("NextStart");
            }

            if (this.gameObject.name == "EventPrevious")
            {
                StartCoroutine("PreviousStart");
            }

            if (this.gameObject.name == "EventBottomlessPit")
            {
                StartCoroutine("BottomlessPitStart");
            }

            if (this.gameObject.name == "EventLevelComplete")
            {
                StartCoroutine("LevelCompleteStart");
            }
        }

    }


    IEnumerator BottomlessPitStart()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        mSceneManager.state = SceneManager.SceneState.Animating;
        mSceneManager.mCameraManager.lockCameraPositions();

        mSceneManager.mPlayerManager.health = mSceneManager.mPlayerManager.health - 1;

        yield return new WaitForSeconds(3.0f);

        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        mSceneManager.mPlayerManager.StartCoroutine("SetInvincible");
        Transform[] ts = this.GetComponentsInChildren<Transform>();
        mSceneManager.mPlayerManager.player.transform.position = ts[1].position;

        mSceneManager.state = SceneManager.SceneState.Playing;
        mSceneManager.mCameraManager.unlockCameraPositions();

    }

    IEnumerator ExitStart()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        mSceneManager.mCameraManager.beginFadeOut();
        mSceneManager.state = SceneManager.SceneState.Animating;
        yield return new WaitForSeconds(3.0f);
        Application.LoadLevel("MainMenuScene");

        mSceneManager.mStageManager.removeEvent(this.gameObject);

        Destroy(this.gameObject);
    }

    IEnumerator NextStart()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        mSceneManager.mCameraManager.beginFadeOut();
        mSceneManager.state = SceneManager.SceneState.Animating;

        mSceneManager.mProfileManager.SaveProfile(0);

        yield return new WaitForSeconds(3.0f);

        string sceneName = Application.loadedLevelName;
        int sceneNumber = int.Parse(sceneName.Substring(sceneName.Length-1));

        Debug.Log("Loading Scene: " + (sceneNumber + 1));

        Application.LoadLevel("Scene" + (sceneNumber + 1));

        mSceneManager.mStageManager.removeEvent(this.gameObject);

        Destroy(this.gameObject);
    }

    IEnumerator PreviousStart()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        mSceneManager.mCameraManager.beginFadeOut();
        mSceneManager.state = SceneManager.SceneState.Animating;

        mSceneManager.mProfileManager.SaveProfile(0);

        yield return new WaitForSeconds(3.0f);

        string sceneName = Application.loadedLevelName;
        int sceneNumber = int.Parse(sceneName.Substring(sceneName.Length - 1));

        Debug.Log("Loading Scene: " + (sceneNumber - 1));

        Application.LoadLevel("Scene" + (sceneNumber - 1));

        mSceneManager.mStageManager.removeEvent(this.gameObject);

        Destroy(this.gameObject);
    }

    IEnumerator SpeechStart()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        mSceneManager.state = SceneManager.SceneState.Locked;
        mSceneManager.mHUDManager.addTextToQueue("You have touched Event Speech! You have touched Event Speech! You have touched Event Speech! You have touched Event Speech! You have touched Event Speech! You have touched Event Speech! You have touched Event Speech! You have touched Event Speech!");
        mSceneManager.mHUDManager.addTextToQueue("Now releasing Lock!");

        while (mSceneManager.mHUDManager.texts.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        mSceneManager.state = SceneManager.SceneState.Playing;

        mSceneManager.mStageManager.removeEvent(this.gameObject);

        Destroy(this.gameObject);
    }

    IEnumerator LevelCompleteStart()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        mSceneManager.state = SceneManager.SceneState.Locked;
        mSceneManager.mHUDManager.enabled = false;
        mSceneManager.mSoundManager.stopSounds();
        mSceneManager.mSoundManager.setBackgroundMusic("sound3");

        displayLevelComplete = true;
        levelComplete1 = true;
        yield return new WaitForSeconds(3.0f);
        tempTimer = mSceneManager.mPlayerManager.time;
        levelComplete1 = false;
        levelComplete2 = true;
        yield return new WaitForSeconds(1.5f);

        while (mSceneManager.mPlayerManager.gems != 0 || tempTimer != 0)
        {
            if (mSceneManager.mPlayerManager.gems != 0)
            {
                mSceneManager.mPlayerManager.gems--;
                mSceneManager.mPlayerManager.score = mSceneManager.mPlayerManager.score + 10;
            }

            if (tempTimer < 200 && tempTimer != 0)
            {
                tempTimer++;
                mSceneManager.mPlayerManager.score = mSceneManager.mPlayerManager.score + 10;
            }
            else
            {
                tempTimer = 0;
            }

            mSceneManager.mSoundManager.playSound("coin", mSceneManager.mCameraManager.getCamera("Main Camera").transform.position);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.0f);
        levelComplete2 = false;
        levelComplete3 = true;

        yield return new WaitForSeconds(3.0f);
        levelComplete3 = false;
        displayLevelComplete = false;

        StartCoroutine("NextStart");
    }
}
