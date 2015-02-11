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

    // Profile Number being loaded
    int mProfileNumber = 0;

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
        mVideoManager.InitializeManager();

        // Load Profile Properties for Player and Scene
        this.LoadSceneProperties(mProfileNumber);
        mPlayerManager.LoadPlayerProperties(mProfileNumber);

    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // Load Properties
    void LoadSceneProperties(int profileNumber)
    {
        Debug.Log("Loading Scene Properties from Profile: " + profileNumber);

        // Change/Set Volume, Resolution, Controls... (Scene Number and Checkpoint Number?)
        
    }
}
