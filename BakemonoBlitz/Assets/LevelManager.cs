using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private SceneManager mSceneManager;

    public LevelData data, data1, data2, data3, data4, data5, data6, data7, data8, data9, data10, data11;

    public Dictionary<string, LevelData> levels;

	// Use this for initialization
	void Awake ()
    {
        this.mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        levels = new Dictionary<string, LevelData>();

        data1 = new LevelData();
        data1.level = 0;
        data1.stage = 0;
        data1.type = LevelType.Menu;
        levels.Add("MainMenuScene", data1);

        data2 = new LevelData();
        data2.level = 1;
        data2.stage = 1;
        data2.type = LevelType.Game;
        levels.Add("Scene1", data2);

        data3 = new LevelData();
        data3.level = 1;
        data3.stage = 2;
        data3.type = LevelType.Game;
        levels.Add("Scene2", data3);

        data4 = new LevelData();
        data4.level = 1;
        data4.stage = 3;
        data4.type = LevelType.Game;
        levels.Add("Scene3", data4);

        data5 = new LevelData();
        data5.level = 1;
        data5.stage = 4;
        data5.type = LevelType.Game;
        levels.Add("Scene4", data5);

        data6 = new LevelData();
        data6.level = 1;
        data6.stage = 5;
        data6.type = LevelType.Game;
        levels.Add("Scene5", data6);

        data7 = new LevelData();
        data7.level = 2;
        data7.stage = 1;
        data7.type = LevelType.Game;
        levels.Add("Scene6", data7);

        data8 = new LevelData();
        data8.level = 2;
        data8.stage = 2;
        data8.type = LevelType.Game;
        levels.Add("Scene7", data8);

        data9 = new LevelData();
        data9.level = 2;
        data9.stage = 3;
        data9.type = LevelType.Game;
        levels.Add("Scene8", data9);

        data10 = new LevelData();
        data10.level = 2;
        data10.stage = 4;
        data10.type = LevelType.Boss;
        levels.Add("Scene9", data10);

        data11 = new LevelData();
        data11.level = 3;
        data11.stage = 1;
        data11.type = LevelType.Other;
        levels.Add("Scene10", data11);
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public void loadCurrentLevel()
    {
        data = new LevelData();
        levels.TryGetValue(Application.loadedLevelName, out data);

        switch(data.type)
        {
            case LevelType.Boss:
                StartCoroutine("BossRoutine");
                break;
            case LevelType.Game:
                StartCoroutine("GameRoutine");
                break;
            case LevelType.Menu:
                StartCoroutine("MenuRoutine");
                break;
            case LevelType.Other:
                StartCoroutine("OtherRoutine");
                break;
            default:
                Debug.Log("Invalid Level Type");
                break;
        }

        // This needs to be called after profile is loaded
        if (data.level > 0 && data.stage == 1)
        {
            mSceneManager.mPlayerManager.time = 0;
        }
    }

    IEnumerator BossRoutine()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("sound3");

        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator GameRoutine()
    {
        yield return mSceneManager.StartCoroutine("LoadingLevelAnimation");

        mSceneManager.mCameraManager.beginFadeIn();

        mSceneManager.state = SceneManager.SceneState.Playing;
        mSceneManager.mProfileManager.setResolution(mSceneManager.mResolution);
        mSceneManager.mProfileManager.setVolume(mSceneManager.mVolume);

        mSceneManager.mSceneNumber = int.Parse(Application.loadedLevelName.Substring(Application.loadedLevelName.Length - 1, 1));

        mSceneManager.playerStart = GameObject.Find("Start").transform.position;
        mSceneManager.playerEnd = GameObject.Find("End").transform.position;

        if (mSceneManager.mProfileManager.getProfileData(0)._userData.sceneNumber > mSceneManager.mSceneNumber)
        {
            mSceneManager.mPlayerManager.player.transform.position = mSceneManager.playerEnd;
        }
        else
        {
            mSceneManager.mPlayerManager.player.transform.position = mSceneManager.playerStart;
        }

        StartCoroutine("Level" + data.level + "_" + data.stage);

        yield return new WaitForSeconds(3.0f);

        mSceneManager.state = SceneManager.SceneState.Playing;

    }

    IEnumerator MenuRoutine()
    {
        mSceneManager.mCameraManager.setCamera("Camera1");

        // Set Default Scene Properties
        mSceneManager.mProfileManager.LoadProfile(0);
        mSceneManager.mProfileManager.setResolution(4.0f);
        mSceneManager.mProfileManager.setVolume(100.0f);

        // Disable Unecessary Managers
        mSceneManager.mMainMenuController.showMenu = false;
        mSceneManager.mBackgroundManager.enabled = false;

        mSceneManager.mCameraManager.beginFadeIn();
        mSceneManager.displayLogo = true;
        yield return new WaitForSeconds(1.0f);
        mSceneManager.mCameraManager.beginFadeOut();
        yield return new WaitForSeconds(3.0f);
        mSceneManager.displayLogo = false;

        // Play Background Music Here.
        mSceneManager.mSoundManager.playSound("Yoshida Brothers - Rising", Vector3.zero);

        // Set the State to Main Menu, so we don't go to other Scene States
        mSceneManager.state = SceneManager.SceneState.MainMenu;

        // Fade into game Menu when done with Video
        mSceneManager.mCameraManager.beginFadeIn();
        mSceneManager.mMainMenuController.showMenu = true;

    }

    IEnumerator OtherRoutine()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("sound1");

        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Level1_1()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("Saitama Saishuu Heiki - Momentary Life [Remix]");
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Level1_2()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("sound1");
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Level1_3()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("sound2");
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Level1_4()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("sound3");
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Level1_5()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("Saitama Saishuu Heiki - Momentary Life [Remix]");
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Level2_1()
    {
        mSceneManager.mSoundManager.setBackgroundMusic("Saitama Saishuu Heiki - Momentary Life [Remix]");
        yield return new WaitForSeconds(1.0f);
    }


    public enum LevelType { Menu, Game, Boss, Other };

    public struct LevelData
    {
        public int level;
        public int stage;

        public LevelType type;
    }
}