using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{

    public bool showMenu = false;

    private int screenWidth, screenHeight;

    Vector4 _Selector, _Container;
    Vector4 _Resume, _Sound, _Video, _SaveLoad, _Exit;
    Vector4 _Volume, _VolumeSlider, _Resolution, _ResolutionSlider, _Save, _Load;
    Vector4 _Yes, _No;

    public float volume = 0.0f;
    private float volMin = 0.0f;
    private float volMax = 100.0f;

    public float resolution = 5.0f;
    private float resMin = 3.0f;
    private float resMax = 7.0f;

    public enum Direction { Menu, Sound, Video, SaveLoad, Exit };
    public Direction state;

    public int selectorIndex = 0;
    int menuNum = 5, soundNum = 1, videoNum = 1, saveLoadNum = 2, exitNum = 2;

    public Texture2D blackbox, bluebox, brownbox;

	// Use this for initialization
	void Start ()
    {
        state = Direction.Menu;
	}

    public void A()
    {
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
            case Direction.SaveLoad:
                selectorIndex++;
                selectorIndex = selectorIndex % saveLoadNum;
                break;
            case Direction.Exit:
                selectorIndex++;
                selectorIndex = selectorIndex % exitNum;
                break;
            default:
                break;
        }
    }

    public void B()
    {
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
            _SaveLoad = new Vector4(getPositionX(30), getPositionY(60), getPositionX(40), getPositionY(8));
            _Exit = new Vector4(getPositionX(30), getPositionY(70), getPositionX(40), getPositionY(8));

            _Volume = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(40));
            _VolumeSlider = new Vector4(getPositionX(35), getPositionY(50), getPositionX(30), getPositionY(8));

            _Resolution = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(40));
            _ResolutionSlider = new Vector4(getPositionX(35), getPositionY(50), getPositionX(30), getPositionY(8));

            _Save = new Vector4(getPositionX(30), getPositionY(30), getPositionX(40), getPositionY(18));
            _Load = new Vector4(getPositionX(30), getPositionY(50), getPositionX(40), getPositionY(18));

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
            case Direction.SaveLoad:
                if (selectorIndex >= saveLoadNum) selectorIndex = 0;
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
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), blackbox);
                GUI.DrawTexture(new Rect(_Resume.x, _Resume.y, _Resume.z, _Resume.w), brownbox);
                GUI.DrawTexture(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), brownbox);
                GUI.DrawTexture(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), brownbox);
                GUI.DrawTexture(new Rect(_SaveLoad.x, _SaveLoad.y, _SaveLoad.z, _SaveLoad.w), brownbox);
                GUI.DrawTexture(new Rect(_Exit.x, _Exit.y, _Exit.z, _Exit.w), brownbox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Resume.x, _Resume.y, _Resume.z, _Resume.w), bluebox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), bluebox);
                        break;
                    case 2:
                        GUI.DrawTexture(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), bluebox);
                        break;
                    case 3:
                        GUI.DrawTexture(new Rect(_SaveLoad.x, _SaveLoad.y, _SaveLoad.z, _SaveLoad.w), bluebox);
                        break;
                    case 4:
                        GUI.DrawTexture(new Rect(_Exit.x, _Exit.y, _Exit.z, _Exit.w), bluebox);
                        break;
                    default:
                        break;
                }


                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Game Paused");

                // Display Options
                GUI.Box(new Rect(_Resume.x, _Resume.y, _Resume.z, _Resume.w), "Resume Game");
                GUI.Box(new Rect(_Sound.x, _Sound.y, _Sound.z, _Sound.w), "Sound Settings");
                GUI.Box(new Rect(_Video.x, _Video.y, _Video.z, _Video.w), "Video Settings");
                GUI.Box(new Rect(_SaveLoad.x, _SaveLoad.y, _SaveLoad.z, _SaveLoad.w), "Save/Load Profiles");
                GUI.Box(new Rect(_Exit.x, _Exit.y, _Exit.z, _Exit.w), "Exit to Main Menu");
            }

            if (state == Direction.Sound)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), blackbox);
                GUI.DrawTexture(new Rect(_Volume.x, _Volume.y, _Volume.z, _Volume.w), brownbox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Volume.x, _Volume.y, _Volume.z, _Volume.w), bluebox);
                        break;
                    default:
                        break;
                }


                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Sound Settings");

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
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), blackbox);
                GUI.DrawTexture(new Rect(_Resolution.x, _Resolution.y, _Resolution.z, _Resolution.w), brownbox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Resolution.x, _Resolution.y, _Resolution.z, _Resolution.w), bluebox);
                        break;
                    default:
                        break;
                }

                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Video Settings");

                // Display Resolution 
                GUI.Box(new Rect(_Resolution.x, _Resolution.y, _Resolution.z, _Resolution.w), "Change Resolution");

                GUI.backgroundColor = Color.black;
                // Display Resolution Slider
                resolution = GUI.HorizontalSlider(new Rect(_ResolutionSlider.x, _ResolutionSlider.y, _ResolutionSlider.z, _ResolutionSlider.w), resolution, resMin, resMax);
                GUI.backgroundColor = Color.clear;

            }

            if (state == Direction.SaveLoad)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), blackbox);
                GUI.DrawTexture(new Rect(_Save.x, _Save.y, _Save.z, _Save.w), brownbox);
                GUI.DrawTexture(new Rect(_Load.x, _Load.y, _Load.z, _Load.w), brownbox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Save.x, _Save.y, _Save.z, _Save.w), bluebox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_Load.x, _Load.y, _Load.z, _Load.w), bluebox);
                        break;
                    default:
                        break;
                }

                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Profile Settings");

                // Display Save Option 
                GUI.Box(new Rect(_Save.x, _Save.y, _Save.z, _Save.w), "Save Profile");

                // Display Load Option 
                GUI.Box(new Rect(_Load.x, _Load.y, _Load.z, _Load.w), "Load Profile");

            }

            if (state == Direction.Exit)
            {
                // Display Textures
                GUI.DrawTexture(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), blackbox);
                GUI.DrawTexture(new Rect(_Yes.x, _Yes.y, _Yes.z, _Yes.w), brownbox);
                GUI.DrawTexture(new Rect(_No.x, _No.y, _No.z, _No.w), brownbox);

                // Display Selector
                switch (selectorIndex)
                {
                    case 0:
                        GUI.DrawTexture(new Rect(_Yes.x, _Yes.y, _Yes.z, _Yes.w), bluebox);
                        break;
                    case 1:
                        GUI.DrawTexture(new Rect(_No.x, _No.y, _No.z, _No.w), bluebox);
                        break;
                    default:
                        break;
                }

                // Display Container
                GUI.Box(new Rect(_Container.x, _Container.y, _Container.z, _Container.w), "Are you sure?");

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
