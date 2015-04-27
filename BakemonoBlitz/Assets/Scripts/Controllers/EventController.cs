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

    //GUI variables
    public Font font;
    public Texture2D tex;
    public Texture2D gemWord;
    public Texture2D scoreWord;
    public Texture2D timeWord;

    //Input used to skip Level Complete part
    public bool skipBonus = false;
    public Texture2D frame1;
    public Texture2D frame2;

    // Set in editor for Speech Event
    public string speechString = "Enter Speech Here...";

    // Variables used for event camera
    public enum CameraPatrolState { Idle, MovingA, MovingB };
    public CameraPatrolState cameraPatrolState;
    public float cameraSize = 2.5f;
    public float cameraTransitionTime = 1.0f;
    public float cameraStayTime = 4.0f;
    public GameObject cameraObj;

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
            GUI.skin.box.font = font;
            GUI.DrawTexture(new Rect(mSceneManager.mHUDManager.getPositionX(15), mSceneManager.mHUDManager.getPositionY(20), mSceneManager.mHUDManager.getPositionX(70), mSceneManager.mHUDManager.getPositionY(60)), tex);


            if (levelComplete1)
            {
                GUI.skin.box.fontSize = 90;
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;

                
                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(10),
                    mSceneManager.mHUDManager.getPositionY(25), 
                    mSceneManager.mHUDManager.getPositionX(80),
                    mSceneManager.mHUDManager.getPositionY(40)), 
                    "LEVEL COMPLETED!");
                GUI.skin.box.alignment = TextAnchor.UpperLeft;

            }
            else if(levelComplete2)
            {
                GUI.skin.box.alignment = TextAnchor.UpperCenter;

                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(0),
                    mSceneManager.mHUDManager.getPositionY(20),
                    mSceneManager.mHUDManager.getPositionX(100),
                    mSceneManager.mHUDManager.getPositionY(40)),
                    "BONUS");

                GUI.skin.box.alignment = TextAnchor.UpperLeft;

                GUI.DrawTexture(new Rect(mSceneManager.mHUDManager.getPositionX(25), mSceneManager.mHUDManager.getPositionY(35), 150, 65), timeWord);
                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(55), mSceneManager.mHUDManager.getPositionY(35), 150, 65),
                    " " + tempTimer);

                GUI.DrawTexture(new Rect(mSceneManager.mHUDManager.getPositionX(25), mSceneManager.mHUDManager.getPositionY(45), 150, 65), gemWord);
                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(55), mSceneManager.mHUDManager.getPositionY(45), 150, 65),
                    " " + mSceneManager.mPlayerManager.gems);

                GUI.DrawTexture(new Rect(mSceneManager.mHUDManager.getPositionX(25), mSceneManager.mHUDManager.getPositionY(60), 180, 65), scoreWord);
                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(55), mSceneManager.mHUDManager.getPositionY(60), 150, 65),
                    " " + mSceneManager.mPlayerManager.score);

            }
            else if (levelComplete3)
            {
                GUI.skin.box.fontSize = 100;
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;

                
                GUI.Box(new Rect(mSceneManager.mHUDManager.getPositionX(0),
                    mSceneManager.mHUDManager.getPositionY(0),
                    mSceneManager.mHUDManager.getPositionX(100),
                    mSceneManager.mHUDManager.getPositionY(100)),
                    "Score: " + mSceneManager.mPlayerManager.score + "");

                GUI.skin.box.alignment = TextAnchor.UpperLeft;

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

            if (this.gameObject.name == "EventCamera")
            {
                StartCoroutine("CameraStart");
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

        if (frame1 != null)
        {
            mSceneManager.mHUDManager.tex2 = frame1;
        }

        if (frame2 != null)
        {
            mSceneManager.mHUDManager.tex3 = frame2;
        }

        mSceneManager.mHUDManager.addTextToQueue(speechString + " ");

        while (mSceneManager.mHUDManager.texts.Count > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }


        mSceneManager.state = SceneManager.SceneState.Animating;

        float timestart = Time.time;

        while (Time.time - timestart < 0.5f)
        {
            mSceneManager.mPlayerManager.player.rigidbody2D.velocity = new Vector2(0, mSceneManager.mPlayerManager.player.rigidbody2D.velocity.y);
            yield return new WaitForSeconds(0.005f);
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
                mSceneManager.mPlayerManager.score = mSceneManager.mPlayerManager.score + 15;
            }

            if (tempTimer < 500 && tempTimer != 0)
            {
                tempTimer++;
                mSceneManager.mPlayerManager.score = mSceneManager.mPlayerManager.score + 10;
            }
            else
            {
                tempTimer = 0;
            }

            if (skipBonus)
            {
                mSceneManager.mPlayerManager.score += mSceneManager.mPlayerManager.gems * 15;
                mSceneManager.mPlayerManager.gems = 0;

                if (tempTimer < 500 && tempTimer != 0)
                {
                    mSceneManager.mPlayerManager.score += (500 - tempTimer) * 10;
                    tempTimer = 0;
                }

            }

            mSceneManager.mSoundManager.playSound("coin", mSceneManager.mCameraManager.getCamera("Main Camera").transform.position);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1.0f);
        levelComplete2 = false;
        levelComplete3 = true;

        yield return new WaitForSeconds(3.0f);
        levelComplete3 = false;
        displayLevelComplete = false;

        StartCoroutine("NextStart");
    }

    public void A()
    {
        if (levelComplete2)
        {
            skipBonus = true;
        }
    }

    IEnumerator CameraStart()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        float timestart = Time.time;
        while (Time.time - timestart < 0.5f)
        {
            mSceneManager.mPlayerManager.player.rigidbody2D.velocity = new Vector2(0, mSceneManager.mPlayerManager.player.rigidbody2D.velocity.y);
            yield return new WaitForSeconds(0.005f);
        }


        cameraPatrolState = CameraPatrolState.Idle;
        // Patrol Coordinates
        GameObject patrolA = null;
        GameObject patrolB = null;
        cameraObj = null;
        Vector3 patrolACoordinates;
        Vector3 patrolBCoordinates;

        foreach (Transform t in this.transform)
        {
            if (t.name == "PatrolA")
                patrolA = t.gameObject;

            if (t.name == "PatrolB")
                patrolB = t.gameObject;

            if (t.name == "Camera")
                cameraObj = t.gameObject;
        }

        patrolACoordinates = patrolA.transform.position;
        patrolBCoordinates = patrolB.transform.position;

        mSceneManager.mCameraManager.setCamera(cameraObj.name);

        cameraPatrolState = CameraPatrolState.MovingA;
        yield return StartCoroutine(MoveObject(transform, patrolACoordinates, patrolBCoordinates, cameraTransitionTime));

        yield return new WaitForSeconds(cameraStayTime);

        cameraPatrolState = CameraPatrolState.MovingB;
        yield return StartCoroutine(MoveObject(transform, patrolBCoordinates, patrolACoordinates, cameraTransitionTime));


        cameraPatrolState = CameraPatrolState.Idle;

        mSceneManager.mCameraManager.setCamera("Main Camera");

        cameraObj.transform.parent = mSceneManager.gameObject.transform.parent;
        mSceneManager.mStageManager.removeEvent(this.gameObject);

        Destroy(this.gameObject);
    }




    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        // Patrol only if player hasn't been found
        double i = 0.0d;
        double rate = 1.0d / time;
        while (i < 1.0d)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, (float)i);

            if (cameraPatrolState == CameraPatrolState.MovingA)
            {
                cameraObj.camera.orthographicSize = mSceneManager.mResolution + (cameraSize - mSceneManager.mResolution) * (float)i;
            }
            else
            {
                cameraObj.camera.orthographicSize = cameraSize + (mSceneManager.mResolution - cameraSize) * (float)i;
            }

            yield return null;
        }
    }

}
