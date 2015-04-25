using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
    HUD Manager
 
    The HUD Manager controls the display at the screens fore front. Elements being displayed may include the player statistics, items, and moves. The HUD Manager is initialized by the Scene Manager.
 
    Responsibilities include:
 
    Getting all necessary data in real time
    Displaying player health and statuses such as power ups or negative ailments
    Displaying player lives
    Displaying Items being held, or weapon in hand

    Health Bar (HUD)
    Energy Bar* (HUD)
    Life Counter (HUD)
    Gem Counter (HUD)
    Score Counter (HUD)
    Timer (HUD)
 
    http://docs.unity3d.com/Manual/HOWTO-UICreateFromScripting.html 
*/

public class HUDManager : MonoBehaviour
{
    PlayerManager mPlayerManager;
    SceneManager mSceneManager;

    public int health, energy, lives, gems, score, time;

    private int screenWidth, screenHeight;

    // Textbox variables
    private string text = "Random Text";
    private bool showingTextbox = false;
    private float textboxDuration = 3.0f;

    public bool disableHUD = false;
    public bool lockHUDSpeech = false;

    public LinkedList<string> texts = new LinkedList<string>();

    // Displaying Speech pictures
    public Texture2D tex;
    public Texture2D tex2;
    public Texture2D tex3;
    public bool texSelector = false;

    public Texture2D livesTexture;
    public Texture2D hp0Texture;
    public Texture2D hp1Texture;
    public Texture2D hp2Texture;
    public Texture2D hp3Texture;
    public Texture2D gemsTexture;

    public Texture2D healthWord;
    public Texture2D livesWord;
    public Texture2D scoreWord;
    public Texture2D timeWord;

    public Font font;

	// Use this for initialization
	void Start ()
    {
        Debug.Log("Initializing " + this.gameObject.name);
        mPlayerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        StartCoroutine("AlternateSpeechPicture");
	}
	
	// Update is called once per frame
	void Update ()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        health = mPlayerManager.health;
        energy = mPlayerManager.energy;
        lives = mPlayerManager.lives;
        gems = mPlayerManager.gems;
        score = mPlayerManager.score;
        time = mPlayerManager.time;

	}

    IEnumerator AlternateSpeechPicture()
    {
        while (true)
        {
            texSelector = !texSelector;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnGUI()
    {
        // Make a background box
        GUI.backgroundColor = Color.clear;
        GUI.color = Color.white;
        GUI.skin.box.fontSize = 30;
        GUI.skin.box.alignment = TextAnchor.UpperLeft;
        GUI.skin.box.fontStyle = FontStyle.Bold;
        GUI.skin.box.font = font;

        if (!disableHUD && mSceneManager.state != SceneManager.SceneState.MainMenu)
        {
            // Draw Lives
            GUI.DrawTexture(new Rect(getPositionX(1), getPositionY(1), getPositionX(9), 40), livesWord);

            for (int i = 0; i < lives; i++)
            {
                GUI.DrawTexture(new Rect(getPositionX(11.0f + i * 4.0f), getPositionY(1), 40, 40), livesTexture);
            }

            // Draw Health
            GUI.DrawTexture(new Rect(getPositionX(1), getPositionY(10), getPositionX(9), 40), healthWord);

            switch (health)
            {
                case 0:
                    GUI.DrawTexture(new Rect(getPositionX(11.0f), getPositionY(10.0f), 88, 40), hp0Texture);
                    break;
                case 1:
                    GUI.DrawTexture(new Rect(getPositionX(11.0f), getPositionY(10.0f), 88, 40), hp1Texture);
                    break;
                case 2:
                    GUI.DrawTexture(new Rect(getPositionX(11.0f), getPositionY(10.0f), 88, 40), hp2Texture);
                    break;
                case 3:
                    GUI.DrawTexture(new Rect(getPositionX(11.0f), getPositionY(10.0f), 88, 40), hp3Texture);
                    break;
                default:
                    GUI.DrawTexture(new Rect(getPositionX(11.0f), getPositionY(10.0f), 88, 40), hp3Texture);
                    break;
            }

            // Draw Gems
            GUI.Box(new Rect(screenWidth / 2 - getPositionX(4), getPositionY(5), getPositionX(14), getPositionY(8)), "       " + gems);
            GUI.DrawTexture(new Rect(screenWidth / 2 - getPositionX(4), getPositionY(1.0f), 40, 80), gemsTexture);

            // Draw Score
            //GUI.Box(new Rect(screenWidth - getPositionX(25), getPositionY(1), getPositionX(22), getPositionY(8)), "SCORE: " + score);
            GUI.DrawTexture(new Rect(screenWidth - getPositionX(25), getPositionY(1), 110, 40), scoreWord);
            GUI.Box(new Rect(screenWidth - getPositionX(25) + 110, getPositionY(1), 110, 50), " " + score);
            // Draw Time
            GUI.DrawTexture(new Rect(screenWidth - getPositionX(25), getPositionY(10), 110, 40), timeWord);
            GUI.Box(new Rect(screenWidth - getPositionX(25) + 110, getPositionY(10), 110, 50), " " + time);
        }

        if (showingTextbox)
        {
            GUI.DrawTexture(new Rect(getPositionX(10), getPositionY(5), getPositionX(80), getPositionY(30)), tex);

            if (texSelector)
            {
                GUI.DrawTexture(new Rect(getPositionX(12), getPositionY(7), getPositionY(26), getPositionY(26)), tex2);
            }
            else
            {
                GUI.DrawTexture(new Rect(getPositionX(12), getPositionY(7), getPositionY(26), getPositionY(26)), tex3);
            }

            GUI.skin.box.fontSize = 33;
            GUI.skin.box.wordWrap = true;
            GUI.Box(new Rect(getPositionX(14) + getPositionY(26), getPositionY(7), getPositionX(55), getPositionY(26)), text);
            GUI.skin.box.fontSize = 15;
        }

        if (texts.Count > 0 && !showingTextbox)
        {
            showingTextbox = true;
            disableHUD = true;
            StartCoroutine(setText());
        }
    }

    // Return position depending on percentage
    public float getPositionX(float percX)
    {
        return percX / 100 * screenWidth;
    }

    public float getPositionY(float percY)
    {
        return percY / 100 * screenHeight;
    }

    // Initialization called by Scene Manager
    public void InitializeManager()
    {

    }

    public void showTextbox(string someText)
    {
        text = someText;
        showingTextbox = true;
        disableHUD = true;
        Invoke("unsetShowTextbox", textboxDuration);
    }

    private void unsetShowTextbox()
    {
        this.showingTextbox = false;
        this.disableHUD = false;
    }

    public void addTextToQueue(string someText)
    {
        this.texts.AddLast(someText);
    }

    private void removeTextFromQueue()
    {
        this.showingTextbox = false;
        this.disableHUD = false;
        texts.RemoveFirst();
    }

    private IEnumerator setText()
    {
        string tempString = "";
        string completeString = texts.First.Value;

        for (int i = 0; i < completeString.Length; i++)
        {
            if (!lockHUDSpeech)
            {
                text = completeString.Substring(0, i);
                if (pressedA == true)
                {
                    pressedA = false;
                    text = completeString;
                    break;
                }
            }
            yield return new WaitForSeconds(0.05f);

        }

        float timeStart = Time.time;

        while (Time.time - timeStart < 3.0f)
        {
            if (pressedA)
            {
                pressedA = false;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        removeTextFromQueue();
    }

    public bool pressedA = false;

    public void A()
    {
        if (showingTextbox)
        {
            pressedA = true;
        }
    }
}
