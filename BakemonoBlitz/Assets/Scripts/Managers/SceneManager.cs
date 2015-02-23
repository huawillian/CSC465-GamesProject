﻿using UnityEngine;
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
public class SceneManager : MonoBehaviour
{

    // Managers
    public BackgroundManager mBackgroundManager;
    public CameraManager mCameraManager;
    public EnemyManager mEnemyManager;
    public HUDManager mHUDManager;
    public InputManager mInputManager;
    public ItemManager mItemManager;
    public PlayerManager mPlayerManager;
    public SoundManager mSoundManager;
    public StageManager mStageManager;
    public VideoManager mVideoManager;
    public ProfileManager mProfileManager;

    public int mSceneNumber = 1;
    public int mCheckpointNumber = 1;
    public float mVolume = 100.0f;
    public float mResolution = 5.0f;

    public MenuController mMenuController;
    public MainMenuController mMainMenuController;

    public bool menuEnabled = false;
    public bool mainMenuEnabled = false;

    // Profile Number being loaded
    public int mProfileNumber = 0;

    // Debug variables, used to create button
    Rect _Save, _Load, _SaveMSG, _LoadMSG;

    public enum SceneState { MainMenu, Playing, Paused, Locked, Animating, SceneComplete};
    public SceneState state = SceneState.Playing;
    public float tempVol;
    // State:
    // Main Menu - Input to all except the Main Menu Controller is disabled, Main Menu is enabled, Menu is disabled
    // Playing - Everything is normal, Input to the Main Menu is disabled
    // Paused - Entered/Leaved from Playing State only, Input to everything except the Menu Controller is disabled, Menu is enabled, Player and Enemy position locked
    // Locked - No Input received by the Input Manager. Player and Enemy positions are locked, but game is still running. Usually done during speech boxes
    // Animating - No Input received by the Input Manager. Positions are not locked. But animation script is played, moving players.
    // Scene Complete - Called when the level is done. Scene is completed, start script to display points and stuff.

    // State Actions:
    // Main Menu will not be able to change states via input
    // Playing State is the default during level scenes
    // Playing State can be paused with a start Input
    // Paused State can be unpaused with a start Input or via script
    // Locked State is initiated/reset to Playing state via script
    // Animating State is set/reset via script 
    // Scene Complete is set/reset via script





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

        mMenuController = GameObject.Find("Menu Controller").GetComponent<MenuController>();
        mMainMenuController = GameObject.Find("Main Menu Controller").GetComponent<MainMenuController>();

    }


    void OnGUI()
    {
        //*************************************************** 
        // Loading The Player... 
        // **************************************************       
        //if (GUI.Button(_Load, "Load"))
        {
            //mProfileManager.LoadProfile(1);
            //mHUDManager.showTextbox("HELLO");
            //mCameraManager.beginFadeIn();
            //mSoundManager.playSound("sound3", new Vector3(0,0,-10));
        }

        //*************************************************** 
        // Saving The Player... 
        // **************************************************    
        //if (GUI.Button(_Save, "Save"))
        {
            //mProfileManager.SaveProfile(1);
            //mHUDManager.showTextbox("WORLD");
            //mCameraManager.beginFadeOut();
            //mVideoManager.playVideo("scene");
            //mHUDManager.addTextToQueue("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            //mHUDManager.addTextToQueue("Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?");
            //mHUDManager.addTextToQueue("But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?");

        
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

        // Load Profile currently being used, defined in SaveData0
        mProfileManager.LoadProfile(0);
        int tempProfile = this.mProfileNumber;
        mProfileManager.LoadProfile(tempProfile);

        string sceneName = Application.loadedLevelName;

        if(sceneName.Equals("MainMenuScene"))
        {
            StartCoroutine("MainMenuScript", 0.0f);
        }

        if (sceneName.Equals("TestScene"))
        {
            StartCoroutine("TestSceneScript", 0.0f);
        }
	}

    IEnumerator MainMenuScript()
    {
        state = SceneState.MainMenu;

        mProfileManager.LoadProfile(0);
        mProfileManager.setResolution(5.0f);
        mProfileManager.setVolume(100.0f);

        mMainMenuController.showMenu = false;

        mVideoManager.playVideo("starcraft");
        yield return new WaitForSeconds(mSoundManager.getSoundLength("starcraft") - 0.2f);
        mSoundManager.playSound("lol", Vector3.zero);

        mCameraManager.beginFadeIn();
        mMainMenuController.showMenu = true;
    }

    IEnumerator TestSceneScript()
    {
        state = SceneState.Playing;
        
        yield return new WaitForSeconds(1.5f);
        /*
        //state = SceneState.Locked;
        mHUDManager.addTextToQueue("Hello Friends...");
        mHUDManager.addTextToQueue("Welcome to Testing the TestScene");
        mHUDManager.addTextToQueue("LALALALALALALALALALA");

        while(this.mHUDManager.texts.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        //state = SceneState.Playing;


        */


    }





	// Update is called once per frame
	void Update ()
    {
        // Set state values
        switch (state)
        {
            case SceneState.MainMenu:
                this.mHUDManager.enabled = false;
                this.mPlayerManager.enabled = false;
                this.mEnemyManager.enabled = false;
                this.mMenuController.enabled = false;
                this.mMainMenuController.enabled = true;
                break;

            case SceneState.Playing:
                this.mHUDManager.enabled = true;
                this.mHUDManager.disableHUD = false;
                this.mHUDManager.lockHUDSpeech = false;

                this.mPlayerManager.enabled = true;
                this.mPlayerManager.lockPlayerCoordinates = false;
                this.mPlayerManager.lockPlayerInput = false;

                this.mEnemyManager.enabled = true;
                this.mEnemyManager.lockEnemyCoordinates = false;

                this.mMainMenuController.enabled = false;

                this.mMenuController.enabled = true;
                mMenuController.lockMenuInput = true;

                break;

            case SceneState.Paused:
                mMenuController.lockMenuInput = false;
                this.mHUDManager.lockHUDSpeech = true;

                this.mPlayerManager.lockPlayerCoordinates = true;
                this.mPlayerManager.lockPlayerInput = true;

                this.mEnemyManager.lockEnemyCoordinates = true;

                this.mMainMenuController.enabled = false;
                this.mMenuController.enabled = true;

                break;

            case SceneState.Locked:
                this.mHUDManager.enabled = true;
                this.mHUDManager.disableHUD = true;

                this.mPlayerManager.lockPlayerCoordinates = true;
                this.mPlayerManager.lockPlayerInput = true;

                this.mEnemyManager.lockEnemyCoordinates = true;

                this.mMainMenuController.enabled = false;
                mMenuController.lockMenuInput = true;
                this.mMenuController.enabled = true;

                break;

            case SceneState.Animating:
                this.mHUDManager.enabled = true;
                this.mHUDManager.disableHUD = false;

                this.mPlayerManager.lockPlayerCoordinates = false;
                this.mPlayerManager.lockPlayerInput = true;

                this.mEnemyManager.lockEnemyCoordinates = true;

                this.mMainMenuController.enabled = false;

                mMenuController.lockMenuInput = true;
                this.mMenuController.enabled = true;

                break;

            case SceneState.SceneComplete:
                this.mHUDManager.enabled = false;
                this.mHUDManager.disableHUD = false;

                this.mPlayerManager.lockPlayerCoordinates = true;
                this.mPlayerManager.lockPlayerInput = true;

                this.mEnemyManager.lockEnemyCoordinates = true;

                this.mMainMenuController.enabled = false;
                mMenuController.lockMenuInput = true;
                this.mMenuController.enabled = true;

                break;

            default:
                break;
        }

    }

    // Load Properties
    void LoadSceneProperties(int profileNumber)
    {
        Debug.Log("Loading Scene Properties from Profile: " + profileNumber);
        mProfileManager.LoadProfile(profileNumber);

        mProfileManager.setVolume(mVolume);
        mCameraManager.setResolution(mResolution);        
    }

    public void StartButton()
    {
        switch (state)
        {
            case SceneState.Playing:
                tempVol = mVolume;
                this.mProfileManager.setVolume(mVolume * 0.25f);
                state = SceneState.Paused;
                break;
            case SceneState.Paused:
                this.mProfileManager.setVolume(tempVol);
                state = SceneState.Playing;
                break;
            default:
                break;
        }
    }
}
