using UnityEngine;
using System.Collections;
using System;

// Select Video to play
// Fade in/out
// Stop Video playback
// Get Video length

/*
    Video Manager
  
    The Video Manager manages all videos within each scene. It implements a list of playable videos and an interface for the user to play them with set parameters such as a duration, in the background, and volume. Addition features include position and size. This manager is initialized by the Scene Manager or Game Manager and all other entities can use this class to play videos.
 
    Responsibilities include:
 
    Generating a list of playable videos
    Holding a video manipulation interface  

*/

// Use Plane that will move based on the location of the main camera.
// Retain a list of movies similar to the Sound Manager
// Set Movie Texture to the plane when playing
// wHEN NOT playing, disable the plane...
// Need to play, stop, and get duration of the movie to be played
// Fade effects?

public class VideoManager : MonoBehaviour
{
    SoundManager mSoundManager;

    private Texture2D[] textures;
    public string[] videoList;

    public bool playingVideo = false;
    public int numberImages = 0;
    public float durationVideo = 0.0f;
    public float startingTime;


    Texture2D mTex;

    public string name;

    int i = 1;
    string currentFile = "";
    string previousFile = "";
    Texture2D videoTexture;

	// Use this for initialization
	void Start ()
    {
	}

    public void playVideo(string nameVideo)
    {
        this.name = nameVideo;

        for (int i = 0; i < videoList.Length; i++)
        {
            if (videoList[i].Equals(name))
            {
                Debug.Log("Playing Video: " + name);

                textures = Resources.LoadAll<Texture2D>("Videos/" + name);
                numberImages = textures.Length;
                playingVideo = true;
                mSoundManager.playSound(name, Vector3.zero);

                durationVideo = mSoundManager.getSoundLength(name);
                startingTime = Time.time;

                InvokeRepeating("change", 0.0f, durationVideo / (float)(numberImages-20));

                // Set Play Video Values Here
                return;
            }
        }

        Debug.Log("Video Not Found: " + name);
    }


	// Update is called once per frame
	void Update ()
    {
        if (playingVideo)
        {
            int numberDigits = 5;
            string digits = "";

            if (i >= numberImages || Time.time - startingTime > durationVideo)
            {
                CancelInvoke();
                playingVideo = false;
                this.i = 1;
            }

            if (i < 10 && i >= 0)
            {
                for (int w = 0; w < numberDigits - 1; w++)
                {
                    digits = digits + "0";
                }
                digits = digits + i;
            }
            if (i < 100 && i >= 10)
            {
                for (int x = 0; x < numberDigits - 2; x++)
                {
                    digits = digits + "0";
                }
                digits = digits + i;
            }
            if (i < 1000 && i >= 100)
            {
                for (int y = 0; y < numberDigits - 3; y++)
                {
                    digits = digits + "0";
                }
                digits = digits + i;
            }
            if (i < 10000 && i >= 1000)
            {
                for (int z = 0; z < numberDigits - 4; z++)
                {
                    digits = digits + "0";
                }
                digits = digits + i;
            }
            previousFile = currentFile;
            currentFile = "Videos" + "/" + name + "/" + name + digits;
        }

	}

    void change()
    {
        try
        {
            i++;
            print(currentFile);
            Resources.UnloadAsset(textures[i-1]); 
            mTex = textures[i];
        } catch(Exception e){}
    }

    void OnGUI()
    {
        if (playingVideo)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mTex);
        }
    }

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);

        mSoundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        videoList = new string[] { "scene" , "starcraft"};
    }
}
