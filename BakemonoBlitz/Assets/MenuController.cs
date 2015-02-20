using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    SceneManager mSceneManager;

    public bool showMenu = false;

    private int screenWidth, screenHeight;

    Vector4 _Selector, _Container;
    Vector4 _Resume, _Sound, _Video, _SaveProfile, _Exit;
    Vector4 _Volume, _VolumeSlider, _Resolution, _ResolutionSlider, _Profile1, _Profile2, _Profile3;
    Vector4 _Yes, _No;

    public float volume = 0.0f;
    private float volMin = 0.0f;
    private float volMax = 100.0f;

    public float resolution = 5.0f;
    private float resMin = 3.0f;
    private float resMax = 7.0f;

    public enum Direction { Menu, Sound, Video, SaveProfile, Exit };
    public Direction state;

    public int selectorIndex = 0;
    int menuNum = 5, soundNum = 1, videoNum = 1, saveNum = 3, exitNum = 2;

    public Texture2D darkbluebox, lightyellowbox, bluebox;

	// Use this for initialization
	void Start ()
    {
        state = Direction.Menu;
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
    }

    private void reset()
    {
        selectorIndex = 0;
        state = Direction.Menu;
    }

    IEnumerator returnToMainMenu()
    {
        mSceneManager.mCameraManager.beginFadeOut();
        showMenu = false;
        yield return new WaitForSeconds(3.0f);
        Application.LoadLevel("TestScene");
    }

    public void A()
    {
        if (showMenu)
        {

            mSceneManager.mSoundManager.playSound("beep1", this.transform.position);

            switch (state)
            {
                case Direction.Menu:
                    selectorIndex++;
                    selectorIndex = selectorIndex % menuNum;
                    break;
                case Direction.Sound:
                    selectorIndex++;
                    selectorIndex = selectorIndex % soundNum;
                    break;
                case Direction.Video:
                    selectorIndex++;
                    selectorIndex = selectorIndex % videoNum;
                    break;
                case Direction.SaveProfile:
                    selectorIndex++;
                    selectorIndex = selectorIndex % saveNum;
                    break;
                case Direction.Exit:
                    selectorIndex++;
                    selectorIndex = selectorIndex % exitNum;
                    break;
                default:
                    break;
            }
        }
    }

    public void B()
    {
        if (showMenu)
        {
            mSceneManager.mSoundManager.playSound("beep2", this.transform.position);

            if (state == Direction.Menu)
            {
                switch (selectorIndex)
                {
                    case 0:
                        showMenu = false;
                        break;
                    case 1:
                        state = Direction.Sound;
                        break;
                    case 2:
                        state = Direction.Video;
                        break;
                    case 3:
                        state = Direction.SaveProfile;
                        break;
                    case 4:
                        state = Direction.Exit;
                        break;
                    default:
                        break;
                }
            }
            else
                if (state == Direction.Sound)
                {
                    mSceneManager.mProfileManager.setVolume(this.volume);
                    state = Direction.Menu;
                }
                else
                    if (state == Direction.Video)
                    {
                        mSceneManager.mProfileManager.setResolution(this.resolution);
                        state = Direction.Menu;
                    }
                    else
                        if (state == Direction.SaveProfile)
                        {
                            switch (selectorIndex)
                            {
                                case 0:
                                    mSceneManager.mProfileManager.SaveProfile(selectorIndex + 1);
                                    state = Direction.Menu;
                                    break;
                                case 1:
                                    mSceneManager.mProfileManager.SaveProfile(selectorIndex + 1);
                                    state = Direction.Menu;
                                    break;
                                case 2:
                                    mSceneManager.mProfileManager.SaveProfile(selectorIndex + 1);
                                    state = Direction.Menu;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            if (state == Direction.Exit)
                            {
                                switch (selectorIndex)
                                {
                                    case 0:
                                        state = Direction.Menu;
                                        StartCoroutine(returnToMainMenu());
                                        break;
                                    case 1:
                                        state = Direction.Menu;
                                        break;
                                    default:
                                        break;
                                }
                            }
        }
    }

    public void StartButton()
    {
        if (showMenu)
        {
            mSceneManager.mSoundManager.playSound("beep2", this.transform.position);
            reset();
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            screenHeight = Screen.height;
            screenWidth = Screen.width;

            _Container = new Vector4(getPositionX(25), getPositionY(20), getPositionX(50), getPositionY(65));

            _Resume = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(8));
            _Sound = new Vector4(getPositionX(30), getPositionY(40), getPositionX(40), getPositionY(8));
            _Video = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(8));
            _SaveProfile = new Vector4(getPositionX(30), getPositionY(60), getPositionX(40), getPositionY(8));
            _Exit = new Vector4(getPositionX(30), getPositionY(70), getPositionX(40), getPositionY(8));

            _Volume = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(40));
            _VolumeSlider = new Vector4(getPositionX(35), getPositionY(50), getPositionX(30), getPositionY(8));

            _Resolution = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(40));
            _ResolutionSlider = new Vector4(getPositionX(35), getPositionY(50), getPositionX(30), getPositionY(8));

            _Profile1 = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(15));
            _Profile2 = new Vector4(getPositionX(30), getPositionY(47), getPositionX(40), getPositionY(15));
            _Profile3 = new Vector4(getPositionX(30), getPositionY(64), getPositionX(40), getPositionY(15)); ;

            _Yes = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(18));
            _No = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(18));
        }

        switch (state)
        {
            case Direction.Menu:
                if (selectorIndex >= menuNum) selectorIndex = 0;
                break;
            case Direction.Sound:
                if (selectorIndex >= soundNum) selectorIndex = 0;
                break;
            case Direction.Video:
                if (selectorIndex >= videoNum) selectorIndex = 0;
                break;
            case Direction.SaveProfile:
                if (selectorIndex >= saveNum) selectorIndex = 0;
                break;
            case Direction.Exit:
                if (selectorIndex >= exitNum) selectorIndex = 0;
                break;
            default:
                break;
        }

	}

    void OnGUI()
    {
        if (showMenu)
        {

            // Make a background box
            GUI.backgroundColor = Color.clear;
            GUI.color = Color.white;
            GUI.skin.box.fontSize = 25;

            if (state == Direction.Menu)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), darkbluebox);
                GUI.DrawTexture(new Rect(_Resume.x, _Resume.y, _Resume.z, _Resume.w), bluebox);
                GUI.DrawTexture(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), bluebox);
                GUI.DrawTexture(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), bluebox);
                GUI.DrawTexture(new Rect(_SaveProfile.x, _SaveProfile.y, _SaveProfile.z, _SaveProfile.w), bluebox);
                GUI.DrawTexture(new Rect(_Exit.x, _Exit.y, _Exit.z, _Exit.w), bluebox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Resume.x, _Resume.y, _Resume.z, _Resume.w), lightyellowbox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), lightyellowbox);
                        break;
                    case 2:
                        GUI.DrawTexture(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), lightyellowbox);
                        break;
                    case 3:
                        GUI.DrawTexture(new Rect(_SaveProfile.x, _SaveProfile.y, _SaveProfile.z, _SaveProfile.w), lightyellowbox);
                        break;
                    case 4:
                        GUI.DrawTexture(new Rect(_Exit.x, _Exit.y, _Exit.z, _Exit.w), lightyellowbox);
                        break;
                    default:
                        break;
                }


                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Game Paused");

                GUI.color = Color.black;
                // Display Options
                GUI.Box(new Rect(_Resume.x, _Resume.y, _Resume.z, _Resume.w), "Resume Game");
                GUI.Box(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), "Sound Settings");
                GUI.Box(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), "Video Settings");
                GUI.Box(new Rect(_SaveProfile.x, _SaveProfile.y, _SaveProfile.z, _SaveProfile.w), "Save Profile");
                GUI.Box(new Rect(_Exit.x, _Exit.y, _Exit.z, _Exit.w), "Exit to Main Menu");
            }

            if (state == Direction.Sound)
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

                GUI.backgroundColor = Color.black;
                // Display Volume Slider
                volume = GUI.HorizontalSlider(new Rect(_VolumeSlider.x, _VolumeSlider.y, _VolumeSlider.z, _VolumeSlider.w), volume, volMin, volMax);
                GUI.backgroundColor = Color.clear;

            }

            if (state == Direction.Video)
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

                GUI.backgroundColor = Color.black;
                // Display Resolution Slider
                resolution = GUI.HorizontalSlider(new Rect(_ResolutionSlider.x, _ResolutionSlider.y, _ResolutionSlider.z, _ResolutionSlider.w), resolution, resMin, resMax);
                GUI.backgroundColor = Color.clear;

            }

            if (state == Direction.SaveProfile)
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
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Saving Profile");

                GUI.color = Color.black;

                UserData profile1 = mSceneManager.mProfileManager.getProfileData(1);
                UserData profile2 = mSceneManager.mProfileManager.getProfileData(2);
                UserData profile3 = mSceneManager.mProfileManager.getProfileData(3);

                // Display Profiles
                GUI.Box(new Rect(_Profile1.x, _Profile1.y, _Profile1.z, _Profile1.w+100), "Profile 1\nScene: " + profile1._userData.sceneNumber + "\nCheckpoint: " + profile1._userData.checkpointNumber);
                GUI.Box(new Rect(_Profile2.x, _Profile2.y, _Profile2.z, _Profile2.w), "Profile 2\nScene: " + profile2._userData.sceneNumber + "\nCheckpoint: " + profile2._userData.checkpointNumber);
                GUI.Box(new Rect(_Profile3.x, _Profile3.y, _Profile3.z, _Profile3.w), "Profile 3\nScene: " + profile3._userData.sceneNumber + "\nCheckpoint: " + profile3._userData.checkpointNumber);

            }

            if (state == Direction.Exit)
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
