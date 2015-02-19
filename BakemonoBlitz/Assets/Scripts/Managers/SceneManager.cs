using UnityEngine;
using System.Collections;

/*
    Scene Manager
 
    The Scene Manager controls each individual unity scene. Each animation/cut-scene, character selection, and level can be categorized as a unity scene. 
    The Scene Manager is unique to each unity scene to either display an animation, character selector, or the level being played.
    This Manager will have a reference to all of the entities on the stage, including the player, enemies, platforms, objects, and events/triggers.
    This manager will also load the next corresponding scene depending on the trigger.
 
    Responsibilities include:
 
    Initializing all of the other Managers within the scene
    Initializing the Camera Controller and Menu Controller
    Display animations, stages, or options uniquely to each unity scene
    Loading player statistics or state upon scene creation
    Loading next scene while passing on the player state
    Invoking the events according to the triggers defined
 */

// Scene Manager needs to set the Scene and Checkpoint Number in Profile Manager in order to save different number into file
public class SceneManager : MonoBehaviour {

    // Managers
    BackgroundManager mBackgroundManager;
    CameraManager mCameraManager;
    EnemyManager mEnemyManager;
    HUDManager mHUDManager;
    InputManager mInputManager;
    ItemManager mItemManager;
    PlayerManager mPlayerManager;
    SoundManager mSoundManager;
    StageManager mStageManager;
    VideoManager mVideoManager;
    ProfileManager mProfileManager;

    public int mSceneNumber = 1;
    public int mCheckpointNumber = 1;

    // Profile Number being loaded
    int mProfileNumber = 1;

    Rect _Save, _Load, _SaveMSG, _LoadMSG;

    // Initialize Scene Manager and get reference to all other Managers
    void Awake()
    {
        Debug.Log("Initalizing Scene Manager");
        mBackgroundManager = GameObject.Find("Background Manager").GetComponent<BackgroundManager>();
        mCameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();
        mEnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        mHUDManager = GameObject.Find("HUD Manager").GetComponent<HUDManager>();
        mInputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        mItemManager = GameObject.Find("Item Manager").GetComponent<ItemManager>();
        mPlayerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        mSoundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        mStageManager = GameObject.Find("Stage Manager").GetComponent<StageManager>();
        mVideoManager = GameObject.Find("Video Manager").GetComponent<VideoManager>();
        mProfileManager = GameObject.Find("Profile Manager").GetComponent<ProfileManager>();


        Debug.Log("Initializing other Managers...");
        mBackgroundManager.InitializeManager();
        mCameraManager.InitializeManager();
        mEnemyManager.InitializeManager();
        mHUDManager.InitializeManager();
        mInputManager.InitializeManager();
        mItemManager.InitializeManager();
        mPlayerManager.InitializeManager();
        mSoundManager.InitializeManager();
        mStageManager.InitializeManager();
        mProfileManager.InitializeManager();
        mVideoManager.InitializeManager();

        // Load Profile Properties for Player and Scene
        this.LoadSceneProperties(mProfileNumber);
        mPlayerManager.LoadPlayerProperties(mProfileNumber);

    }


    void OnGUI()
    {
        //*************************************************** 
        // Loading The Player... 
        // **************************************************       
        if (GUI.Button(_Load, "Load"))
        {
            mProfileManager.LoadProfile(1);
            //mHUDManager.showTextbox("HELLO");
            //mCameraManager.beginFadeIn();
            mSoundManager.playSound("sound3", Vector3.zero);
        }

        //*************************************************** 
        // Saving The Player... 
        // **************************************************    
        if (GUI.Button(_Save, "Save"))
        {
            mProfileManager.SaveProfile(2);
            //mHUDManager.showTextbox("WORLD");
            mCameraManager.beginFadeOut();
        }


    }
	// Use this for initialization
	void Start ()
    {
        // We setup our rectangles for our messages 
        _Save = new Rect(10, 80, 100, 20);
        _Load = new Rect(10, 100, 100, 20);
        _SaveMSG = new Rect(10, 120, 400, 40);
        _LoadMSG = new Rect(10, 140, 400, 40);

        mSoundManager.playSound("video", Vector3.zero);
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject camera =  mCameraManager.getCamera("Main Camera");

        //camera.transform.position = new Vector3(camera.transform.position.x + 0.001f, camera.transform.position.y, camera.transform.position.z);
    }

    // Load Properties
    void LoadSceneProperties(int profileNumber)
    {
        Debug.Log("Loading Scene Properties from Profile: " + profileNumber);

        // Change/Set Volume, Resolution, Controls... (Scene Number and Checkpoint Number?)
        
    }
}
