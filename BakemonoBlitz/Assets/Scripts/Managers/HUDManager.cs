using UnityEngine;
using System.Collections;

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
    public int health, energy, lives, gems, score, time;

    private int screenWidth, screenHeight;
    //public Texture2D image;

    private string text = "Random Text";
    private bool showingTextbox = false;
    private float textboxDuration = 3.0f;

    private bool disableHUD = false;

	// Use this for initialization
	void Start ()
    {

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

    void OnGUI()
    {
        // Make a background box
        GUI.backgroundColor = Color.black;
        GUI.color = Color.white;
        //GUI.Box(new Rect(10, 10, 100, 100), image);

        if (!disableHUD)
        {
            // Draw Health
            GUI.Box(new Rect(getPositionX(1), getPositionY(1), getPositionX(14), getPositionY(4)), "HEALTH: " + health);
            // Draw Energy
            GUI.Box(new Rect(getPositionX(1), getPositionY(6), getPositionX(14), getPositionY(4)), "ENERGY: " + energy);
            // Draw Lives
            GUI.Box(new Rect(getPositionX(1), getPositionY(11), getPositionX(14), getPositionY(4)), "LIVES: " + lives);
            // Draw Gems
            GUI.Box(new Rect(screenWidth / 2 - getPositionX(7), getPositionY(1), getPositionX(14), getPositionY(4)), "GEMS: " + gems);
            // Draw Score
            GUI.Box(new Rect(screenWidth - getPositionX(15), getPositionY(1), getPositionX(14), getPositionY(4)), "SCORE: " + score);
            // Draw Time
            GUI.Box(new Rect(screenWidth - getPositionX(15), getPositionY(6), getPositionX(14), getPositionY(4)), "TIME: " + time);
        }
        if (showingTextbox)
        {
            // Draw Text Box
            GUI.Box(new Rect(getPositionX(5), getPositionY(3), getPositionX(90), getPositionY(30)), text);
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

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
        mPlayerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
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

}
