using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    SceneManager mSceneManager;

    public bool showMenu = false;

    private int screenWidth, screenHeight;

    Vector4 _Selector, _Container; // Helper Pos and Sizes
    Vector4 _NewGame, _LoadGame, _Settings, _QuitGame;
    Vector4 _Sound, _Video;
    Vector4 _LoadProfile;
    Vector4 _StartScreen;
    Vector4 _Title;

    Vector4 _Volume, _VolumeSlider, _Resolution, _ResolutionSlider, _Profile1, _Profile2, _Profile3;
    Vector4 _Yes, _No;

    public float volume = 100.0f;
    private float volMin = 0.0f;
    private float volMax = 100.0f;

    public float resolution = 5.0f;
    private float resMin = 3.0f;
    private float resMax = 7.0f;

    public enum MainMenuState { StartScreen, Menu, NewGame, LoadGame, Settings, SettingsSound, SettingsVideo, QuitGame };
    public MainMenuState state;

    public int selectorIndex = 0;
    int menuNum = 4, newGameNum = 1, loadGameNum = 3, settingsNum = 2,  soundNum = 1, videoNum = 1, quitGameNum = 2;

    public Texture2D darkbluebox, lightyellowbox, bluebox;

    // Use this for initialization
    void Start()
    {
        state = MainMenuState.StartScreen;
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        resolution = mSceneManager.mResolution;
        volume = mSceneManager.mVolume;
    }

    public void B()
    {
        if (showMenu)
        {
            if (state != MainMenuState.StartScreen) mSceneManager.mSoundManager.playSound("beep1", this.transform.position);
            selectorIndex++;
        }
    }

    public void A()
    {
        if (showMenu)
        {
            if (state != MainMenuState.StartScreen) mSceneManager.mSoundManager.playSound("beep2", this.transform.position);

            if (state == MainMenuState.Menu) // Change State from Menu
            {
                switch (selectorIndex)
                {
                    case 0:
                        StartCoroutine("loadNewProfile");
                        break;
                    case 1:
                        state = MainMenuState.LoadGame;
                        break;
                    case 2:
                        state = MainMenuState.Settings;
                        break;
                    case 3:
                        state = MainMenuState.QuitGame;
                        break;
                    default:
                        break;
                }
            }
            else
                if (state == MainMenuState.NewGame) // Change State from New Game
                {
                    // Do nothing
                }
                else
                    if (state == MainMenuState.LoadGame) // Change State from Load Game
                    {
                            switch (selectorIndex) // Load Profile #
                            {
                            case 0:
                                StartCoroutine("loadProfile", selectorIndex + 1);
                                break;
                            case 1:
                                StartCoroutine("loadProfile", selectorIndex + 1);
                                break;
                            case 2:
                                StartCoroutine("loadProfile", selectorIndex + 1);
                                break;
                            default:
                                break;
                            }
                    }
                    else
                    if (state == MainMenuState.Settings) // Change State from Settings
                    {
                            switch (selectorIndex) // Sound or Video
                            {
                            case 0:
                                state = MainMenuState.SettingsSound;
                                break;
                            case 1:
                                state = MainMenuState.SettingsVideo;
                                break;
                            default:
                                break;
                            }
                    }
                    else
                    if (state == MainMenuState.SettingsSound) // Change State from Sound Settings
                    {
                            switch (selectorIndex)
                            {
                            case 0:
                                mSceneManager.mProfileManager.setVolume(this.volume);
                                state = MainMenuState.Menu;                                
                                break;
                            default:
                                break;
                            }
                    }
                    else
                    if (state == MainMenuState.SettingsVideo) // Change State from Video Settings
                    {
                            switch (selectorIndex)
                            {
                            case 0:
                                mSceneManager.mProfileManager.setResolution(this.resolution);
                                state = MainMenuState.Menu;                             
                                break;
                            default:
                                break;
                            }
                    }
                    else
                        if (state == MainMenuState.QuitGame) // Change State from QuitGame Settings
                        {
                            switch (selectorIndex)
                            {
                                case 0:
                                    StartCoroutine("quitGame");
                                    break;
                                case 1:
                                    state = MainMenuState.Menu;
                                    break;
                                default:
                                    break;
                            }
                        }
        }
    }

    IEnumerator loadProfile(int profileNumber)
    {
        // Change settings for that profile 
        float tempVol = volume;
        float tempRes = resolution;

        mSceneManager.mProfileManager.LoadProfile(profileNumber);
        mSceneManager.mSceneNumber = mSceneManager.mProfileManager.getProfileData(profileNumber)._userData.sceneNumber;

        if (tempVol != 100.0f)
        {
            mSceneManager.mProfileManager.setVolume(tempVol);
            mSceneManager.mProfileManager.SaveProfile(profileNumber);
        }

        if (tempRes != 5.0f)
        {
            mSceneManager.mProfileManager.setResolution(tempRes);
            mSceneManager.mProfileManager.SaveProfile(profileNumber);
        }

        // Use profile SaveData0 as temp data
        // This will be opened in the New Scene First in order to select which profile data to Load
        mSceneManager.mProfileManager.LoadProfile(0);

        mSceneManager.mSceneNumber = 0;
        mSceneManager.mCheckpointNumber = 1;

        mSceneManager.mProfileNumber = profileNumber;
        mSceneManager.mProfileManager.SaveProfile(0);

        mSceneManager.mCameraManager.beginFadeOut();
        yield return new WaitForSeconds(2.5f);



        // Load Scene given profile Number
        Application.LoadLevel("Scene" + mSceneManager.mProfileManager.getProfileData(profileNumber)._userData.sceneNumber);
    }

    IEnumerator loadNewProfile()
    {
        mSceneManager.mCameraManager.beginFadeOut();
        yield return new WaitForSeconds(2.5f);
        mSceneManager.mSceneNumber = 0;
        mSceneManager.mCheckpointNumber = 1;
        mSceneManager.mProfileNumber = 0;

        mSceneManager.mPlayerManager.gender = "male";
        mSceneManager.mPlayerManager.health = 3;
        mSceneManager.mPlayerManager.lives = 5;
        mSceneManager.mPlayerManager.energy = 100;

        mSceneManager.mPlayerManager.weapon1 = false;
        mSceneManager.mPlayerManager.weapon2 = false;
        mSceneManager.mPlayerManager.weapon3 = false;

        mSceneManager.mPlayerManager.gems = 0;

        mSceneManager.mProfileManager.SaveProfile(0);

        // Load Tutorial scene
        Application.LoadLevel("Scene1");
    }

    IEnumerator quitGame()
    {
        this.showMenu = false;
        mSceneManager.mCameraManager.beginFadeOut();
        yield return new WaitForSeconds(2.5f);
        Application.Quit();
    }

    public void StartButton()
    {
        if (showMenu)
        {
            if (state == MainMenuState.StartScreen || state == MainMenuState.LoadGame || state == MainMenuState.Settings || state == MainMenuState.SettingsSound || state == MainMenuState.SettingsVideo)
            {
                mSceneManager.mSoundManager.playSound("beep2", this.transform.position);

                selectorIndex = 0;
                state = MainMenuState.Menu;
                resolution = mSceneManager.mResolution;
                volume = mSceneManager.mVolume;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (showMenu)
        {
            // Set Graphic Dimensions
            if (screenHeight != Screen.height || screenWidth != Screen.width)
            {
                screenHeight = Screen.height;
                screenWidth = Screen.width;

                _Container = new Vector4(getPositionX(25), getPositionY(40), getPositionX(50), getPositionY(50));

                _NewGame = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(8));
                _LoadGame = new Vector4(getPositionX(30), getPositionY(60), getPositionX(40), getPositionY(8));
                _Settings = new Vector4(getPositionX(30), getPositionY(70), getPositionX(40), getPositionY(8));
                _QuitGame = new Vector4(getPositionX(30), getPositionY(80), getPositionX(40), getPositionY(8));

                _Profile1 = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(10));
                _Profile2 = new Vector4(getPositionX(30), getPositionY(62), getPositionX(40), getPositionY(10));
                _Profile3 = new Vector4(getPositionX(30), getPositionY(74), getPositionX(40), getPositionY(10));

                _Sound = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(18));
                _Video = new Vector4(getPositionX(30), getPositionY(70), getPositionX(40), getPositionY(18));


                _Volume = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(40));
                _VolumeSlider = new Vector4(getPositionX(35), getPositionY(70), getPositionX(30), getPositionY(8));

                _Resolution = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(40));
                _ResolutionSlider = new Vector4(getPositionX(35), getPositionY(70), getPositionX(30), getPositionY(8));

                _Yes = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(18));
                _No = new Vector4(getPositionX(30), getPositionY(70), getPositionX(40), getPositionY(18));

                _StartScreen = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(30));
                _Title = new Vector4(getPositionX(25), getPositionY(10), getPositionX(50), getPositionY(25));
            }

            // Check selector
            switch (state)
            {
                case MainMenuState.Menu:
                    if (selectorIndex >= menuNum) selectorIndex = 0;
                    selectorIndex = selectorIndex % menuNum;
                    break;
                case MainMenuState.NewGame:
                    if (selectorIndex >= newGameNum) selectorIndex = 0;
                    selectorIndex = selectorIndex % newGameNum;
                    break;
                case MainMenuState.LoadGame:
                    if (selectorIndex >= loadGameNum) selectorIndex = 0;
                    selectorIndex = selectorIndex % loadGameNum;
                    break;
                case MainMenuState.Settings:
                    if (selectorIndex >= settingsNum) selectorIndex = 0;
                    selectorIndex = selectorIndex % settingsNum;
                    break;
                case MainMenuState.SettingsSound:
                    if (selectorIndex >= soundNum) selectorIndex = 0;
                    if (volume > volMax) volume = volMax;
                    if (volume < volMin) volume = volMin;
                    selectorIndex = selectorIndex % soundNum;
                    break;
                case MainMenuState.SettingsVideo:
                    if (selectorIndex >= videoNum) selectorIndex = 0;
                    if (resolution > resMax) resolution = resMax;
                    if (resolution < resMin) resolution = resMin;
                    selectorIndex = selectorIndex % videoNum;
                    break;
                case MainMenuState.QuitGame:
                    if (selectorIndex >= quitGameNum) selectorIndex = 0;
                    selectorIndex = selectorIndex % quitGameNum;
                    break;
                default:
                    break;
            }
        }
    }

    void OnGUI()
    {
        if (showMenu)
        {

            // Make a background box
            GUI.backgroundColor = Color.clear;
            GUI.color = Color.white;
            GUI.skin.box.fontSize = 60;

            //GUI.DrawTexture(new Rect(_Title.x, _Title.y, _Title.z, _Title.w), bluebox);
            GUI.Box(new Rect(_Title.x, _Title.y, _Title.z, _Title.w), "Bakemono Blitz");
            GUI.skin.box.fontSize = 25;

            if (state == MainMenuState.StartScreen)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_StartScreen.x, _StartScreen.y, _StartScreen.z, _StartScreen.w), bluebox);
                GUI.Box(new Rect(_NewGame.x, _NewGame.y, _NewGame.z, _NewGame.w), "Press \"Start\" to continue...");
            }


            if (state == MainMenuState.Menu)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_NewGame.x, _NewGame.y, _NewGame.z, _NewGame.w), bluebox);
                GUI.DrawTexture(new Rect(_LoadGame.x, _LoadGame.y, _LoadGame.z, _LoadGame.w), bluebox);
                GUI.DrawTexture(new Rect(_Settings.x, _Settings.y, _Settings.z, _Settings.w), bluebox);
                GUI.DrawTexture(new Rect(_QuitGame.x, _QuitGame.y, _QuitGame.z, _QuitGame.w), bluebox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_NewGame.x, _NewGame.y, _NewGame.z, _NewGame.w), lightyellowbox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_LoadGame.x, _LoadGame.y, _LoadGame.z, _LoadGame.w), lightyellowbox);
                        break;
                    case 2:
                        GUI.DrawTexture(new Rect(_Settings.x, _Settings.y, _Settings.z, _Settings.w), lightyellowbox);
                        break;
                    case 3:
                        GUI.DrawTexture(new Rect(_QuitGame.x, _QuitGame.y, _QuitGame.z, _QuitGame.w), lightyellowbox);
                        break;
                    default:
                        break;
                }


                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Main Menu");

                GUI.color = Color.black;
                // Display Options
                GUI.Box(new Rect(_NewGame.x, _NewGame.y, _NewGame.z, _NewGame.w), "New Game");
                GUI.Box(new Rect(_LoadGame.x, _LoadGame.y, _LoadGame.z, _LoadGame.w), "Load Game");
                GUI.Box(new Rect(_Settings.x, _Settings.y, _Settings.z, _Settings.w), "Settings");
                GUI.Box(new Rect(_QuitGame.x, _QuitGame.y, _QuitGame.z, _QuitGame.w), "Quit Game");
            }

            if (state == MainMenuState.NewGame)
            {
                // Do nothing
            }


            if (state == MainMenuState.LoadGame)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_Profile1.x, _Profile1.y, _Profile1.z, _Profile1.w), bluebox);
                GUI.DrawTexture(new Rect(_Profile2.x, _Profile2.y, _Profile2.z, _Profile2.w), bluebox);
                GUI.DrawTexture(new Rect(_Profile3.x, _Profile3.y, _Profile3.z, _Profile3.w), bluebox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Profile1.x, _Profile1.y, _Profile1.z, _Profile1.w), lightyellowbox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_Profile2.x, _Profile2.y, _Profile2.z, _Profile2.w), lightyellowbox);
                        break;
                    case 2:
                        GUI.DrawTexture(new Rect(_Profile3.x, _Profile3.y, _Profile3.z, _Profile3.w), lightyellowbox);
                        break;
                    default:
                        break;
                }

                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Choose Profile to Load");

                GUI.color = Color.black;

                UserData profile1 = mSceneManager.mProfileManager.getProfileData(1);
                UserData profile2 = mSceneManager.mProfileManager.getProfileData(2);
                UserData profile3 = mSceneManager.mProfileManager.getProfileData(3);

                GUI.skin.box.fontSize = 15;

                // Display Profiles
                GUI.Box(new Rect(_Profile1.x, _Profile1.y, _Profile1.z, _Profile1.w + 100), "Profile 1\nScene: " + profile1._userData.sceneNumber + "\nCheckpoint: " + profile1._userData.checkpointNumber);
                GUI.Box(new Rect(_Profile2.x, _Profile2.y, _Profile2.z, _Profile2.w), "Profile 2\nScene: " + profile2._userData.sceneNumber + "\nCheckpoint: " + profile2._userData.checkpointNumber);
                GUI.Box(new Rect(_Profile3.x, _Profile3.y, _Profile3.z, _Profile3.w), "Profile 3\nScene: " + profile3._userData.sceneNumber + "\nCheckpoint: " + profile3._userData.checkpointNumber);

                GUI.skin.box.fontSize = 25;

            }

            if (state == MainMenuState.Settings)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), bluebox);
                GUI.DrawTexture(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), bluebox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), lightyellowbox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), lightyellowbox);
                        break;
                    default:
                        break;
                }

                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Change Settings");

                GUI.color = Color.black;

                // Display Yes Option 
                GUI.Box(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), "Sound");

                // Display No Option 
                GUI.Box(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), "Video");
            }

            if (state == MainMenuState.SettingsSound)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_Volume.x, _Volume.y, _Volume.z, _Volume.w), bluebox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Volume.x, _Volume.y, _Volume.z, _Volume.w), lightyellowbox);
                        break;
                    default:
                        break;
                }


                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Sound Settings");

                GUI.color = Color.black;

                // Display Volume 
                GUI.Box(new Rect(_Volume.x, _Volume.y, _Volume.z, _Volume.w), "Change Volume");

                volume += mSceneManager.mInputManager.LX * 0.5f;

                GUI.backgroundColor = Color.black;
                // Display Volume Slider
                GUI.HorizontalSlider(new Rect(_VolumeSlider.x, _VolumeSlider.y, _VolumeSlider.z, _VolumeSlider.w), volume, volMin, volMax);
                GUI.backgroundColor = Color.clear;

            }

            if (state == MainMenuState.SettingsVideo)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_Resolution.x, _Resolution.y, _Resolution.z, _Resolution.w), bluebox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Resolution.x, _Resolution.y, _Resolution.z, _Resolution.w), lightyellowbox);
                        break;
                    default:
                        break;
                }

                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Video Settings");

                GUI.color = Color.black;

                // Display Resolution 
                GUI.Box(new Rect(_Resolution.x, _Resolution.y, _Resolution.z, _Resolution.w), "Change Resolution");

                resolution += mSceneManager.mInputManager.LX * 0.05f;


                GUI.backgroundColor = Color.black;
                // Display Resolution Slider
                GUI.HorizontalSlider(new Rect(_ResolutionSlider.x, _ResolutionSlider.y, _ResolutionSlider.z, _ResolutionSlider.w), resolution, resMin, resMax);
                GUI.backgroundColor = Color.clear;

            }

            if (state == MainMenuState.QuitGame)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_Yes.x, _Yes.y, _Yes.z, _Yes.w), bluebox);
                GUI.DrawTexture(new Rect(_No.x, _No.y, _No.z, _No.w), bluebox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Yes.x, _Yes.y, _Yes.z, _Yes.w), lightyellowbox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_No.x, _No.y, _No.z, _No.w), lightyellowbox);
                        break;
                    default:
                        break;
                }

                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Are you sure?");

                GUI.color = Color.black;

                // Display Yes Option 
                GUI.Box(new Rect(_Yes.x, _Yes.y, _Yes.z, _Yes.w), "Yes");

                // Display No Option 
                GUI.Box(new Rect(_No.x, _No.y, _No.z, _No.w), "No");
            }

        }
    }

    public void moveUp()
    {
        if (showMenu)
        {
            Debug.Log("MoveUp");
            mSceneManager.mSoundManager.playSound("beep1", this.transform.position);
            // Check selector
            switch (state)
            {
                case MainMenuState.Menu:
                    if (selectorIndex < menuNum - 1) selectorIndex++;
                    break;
                case MainMenuState.NewGame:
                    if (selectorIndex < newGameNum - 1) selectorIndex++;
                    break;
                case MainMenuState.LoadGame:
                    if (selectorIndex < loadGameNum - 1) selectorIndex++;
                    break;
                case MainMenuState.Settings:
                    if (selectorIndex < settingsNum - 1) selectorIndex++;
                    break;
                case MainMenuState.SettingsSound:
                    if (selectorIndex < soundNum - 1) selectorIndex++;
                    break;
                case MainMenuState.SettingsVideo:
                    if (selectorIndex < videoNum - 1) selectorIndex++;
                    break;
                case MainMenuState.QuitGame:
                    if (selectorIndex < quitGameNum - 1) selectorIndex++;
                    break;
                default:
                    break;
            }
        }
    }

    public void moveDown()
    {
        if (showMenu)
        {
            Debug.Log("MoveDown");

            mSceneManager.mSoundManager.playSound("beep1", this.transform.position);
            if (selectorIndex > 0) selectorIndex--;
        }
    }

    // Return position depending on percentage
    private float getPositionX(float percX)
    {
        return percX / 100 * screenWidth;
    }

    private float getPositionY(float percY)
    {
        return percY / 100 * screenHeight;
    }
}
