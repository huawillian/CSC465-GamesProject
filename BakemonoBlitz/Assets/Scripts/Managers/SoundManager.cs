using UnityEngine;
using System.Collections;
using System;

//  playSound([name], [Vector3 location]);
//  stopSound([name]);
//  stopSounds();
//  loopSound([name]);
//  setVolumeSound([name], 0-1F);
//  setVolumeSounds([0-1F]);

/*
    Sound Manager
  
    The Sound Manager manages all sounds and music within each scene. 
    It implements a list of playable sounds and an interface for the user to play them with set parameters such as a duration, in the background, and volume. 
    This manager is initialized by the Scene Manager and all other entities can use this class to play sounds.
 
    Responsibilities include:
 
    Generating a list of playable sounds
    Holding a sound manipulation interface
*/

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clip;
    public string[] soundList;
    public GameObject[] soundHolders;

    public GameObject backgroundMusic;
    private SceneManager mSceneManager;

    public float soundCoefficient = 1.0f;
    public float soundVolume = 1.0f;
    public float backgroundMusicCoefficient = 1.0f;
    public float backgroundMusicVolume = 0.5f;

    void Start()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
    }

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);

        soundList = new string[] { "sound1", "sound2", "sound3", "scene", "beep1", "beep2", "starcraft", "lol", "Yoshida Brothers - Rising", "Saitama Saishuu Heiki - Momentary Life [Remix]", "coin", "oneup" };
        clip = new AudioClip[soundList.Length];
        soundHolders = new GameObject[soundList.Length];

        for(int i=0; i<soundList.Length; i++)
        {
            // Load Sound
            Debug.Log(soundList[i]);
            clip[i] = Resources.Load("Sounds/" + soundList[i]) as AudioClip;

            // Create sound holder
            soundHolders[i] = new GameObject();
            soundHolders[i].AddComponent<AudioSource>();
            soundHolders[i].transform.parent = this.transform;
        }

        backgroundMusic = new GameObject();
        backgroundMusic.AddComponent<AudioSource>();

    }

    public void Update()
    {
        Vector3 camPos = mSceneManager.mCameraManager.getCurrentCamera().transform.position;
        backgroundMusic.transform.position = new Vector3(camPos.x, camPos.y, 0);

        // Set Background Music Volume
        if (Math.Abs(backgroundMusicVolume - soundVolume * backgroundMusicCoefficient) > 0.01f)
        {
            backgroundMusicVolume = soundVolume * backgroundMusicCoefficient;

            for (int i = 0; i < soundList.Length; i++)
            {
                if (soundHolders[i] == backgroundMusic)
                {
                    soundHolders[i].GetComponent<AudioSource>().volume = backgroundMusicVolume;
                }
            }

        }

    }

    // Called by other classes to play sound
    public void playSound(string name, Vector3 location)
    {
        for(int i=0; i<soundList.Length; i++)
        {
            if (soundList[i].Equals(name))
            {
                Debug.Log("[" + location.x +  ", " + location.y + ", " + location.z + "] Playing Sound: " + name );
                soundHolders[i].transform.position = location;
                soundHolders[i].GetComponent<AudioSource>().PlayOneShot(clip[i]);
                return;
            }
        }

        Debug.Log("[" + location.x + ", " + location.y + ", " + location.z + "] Sound Not Found: " + name);
    }

    public float getSoundLength(string name)
    {

        for (int i = 0; i < soundList.Length; i++)
        {
            if (soundList[i].Equals(name))
            {
                return clip[i].length;
            }
        }

        Debug.Log("Sound Not Found: " + name);
        return 0.0f;
    }

    public void stopSound(string name)
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            if (soundList[i].Equals(name))
            {
                Debug.Log("Stopping Sound: " + name);
                soundHolders[i].GetComponent<AudioSource>().Stop();
                return;
            }
        }

        Debug.Log("Sound Not Found: " + name);
    }

    public void loopSound(string name)
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            if (soundList[i].Equals(name))
            {
                soundHolders[i].GetComponent<AudioSource>().loop = true;
                return;
            }
        }

        Debug.Log("Sound Not Found: " + name);
    }

    public void stopSounds()
    {
        Debug.Log("Stopping All Sounds");

        for (int i = 0; i < soundList.Length; i++)
        {
            soundHolders[i].GetComponent<AudioSource>().Stop();
        }
    }

    public void setVolumeSound(string name, float volume)
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            if (soundList[i].Equals(name))
            {
                soundHolders[i].GetComponent<AudioSource>().volume = volume / 100.0f * soundCoefficient;
                return;
            }
        }

        Debug.Log("Sound Not Found: " + name);
    }

    public void setVolumeSounds(float volume)
    {
        Debug.Log("Setting volume: " + volume);

        for (int i = 0; i < soundList.Length; i++)
        {
            soundHolders[i].GetComponent<AudioSource>().volume = volume / 100.0f * soundCoefficient;
        }

        soundVolume = soundHolders[0].GetComponent<AudioSource>().volume;
    }

    void OnDestroy()
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            Resources.UnloadAsset(clip[i]);
        }
    }

    public void setBackgroundMusic(string name)
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            if (soundList[i].Equals(name))
            {
                backgroundMusic = soundHolders[i];
                soundHolders[i].GetComponent<AudioSource>().PlayOneShot(clip[i]);
                loopSound(name);
                return;
            }
        }

        Debug.Log("Sound Not Found: " + name);

    }
}
