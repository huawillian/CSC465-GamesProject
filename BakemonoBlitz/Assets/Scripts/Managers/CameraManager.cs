using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
    The Camera Manager controls the camera movements. Camera positions depend on the player or event triggers. This controller is initialized by the Scene Manager.
 
    Responsibilities include:
 
    Smoothly following the player position
    Zooming in on key parts
    Focus or Lock on for certain events such as boss battles
    Switching Main Camera's 
*/

// Change Resolution
// Fade In
// Fade Out
// Lock Camera Positions
// Switch Camera

public class CameraManager : MonoBehaviour
{

    private GameObject[] cameras;
    public Vector3[] cameraCoordinates;
    private int numCameras;
    private GameObject currentCamera;

    // Locking Camera position
    public bool locked = false;

    // Fading variables
    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.1f;

    private int drawDepth = -1000;
    float alpha = 1.0f;
    private float fadeDir = -1;

    public bool fadeIn = false;
    public bool fadeOut = false;

    SceneManager mSceneManager;

	// Use this for initialization
	void Start ()
    {
        mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (locked)
        {
            setCameraFromCoordinates();
        }
        else
        {
            setCoordinatesFromCamera();
        }

        cameras[numCameras - 1].transform.position = new Vector3(mSceneManager.mPlayerManager.x, mSceneManager.mPlayerManager.y, cameras[numCameras - 1].transform.position.z);
	}

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);

        // Get reference to all cameras
        GameObject[] cameraArray = GameObject.FindGameObjectsWithTag("Camera");

        cameras = new GameObject[cameraArray.Length+1];
        cameraCoordinates = new Vector3[cameras.Length];

        for (int i = 0; i < cameras.Length-1; i++)
        {
            cameras[i] = cameraArray[i];
        }

        cameras[cameras.Length-1] = GameObject.FindGameObjectWithTag("MainCamera");

        // Set number of cameras
        numCameras = cameras.Length;

        // Set current camera to the MainCamera
        currentCamera = cameras[cameras.Length - 1];

        setAudioListeners();
    }

    // Get Camera from string
    public GameObject getCamera(String cameraName)
    {
        foreach (GameObject camera in cameras)
        {
            if (cameraName.Equals(camera.name))
            {
                return camera;
            }
        }

        return null;
    }

    // Set Main Camera Camera from string
    public void setCamera(String cameraName)
    {
        foreach (GameObject camera in cameras)
        {
            if (cameraName.Equals(camera.name))
            {
                currentCamera = camera;
                setAudioListeners();
                camera.tag = "MainCamera";
                camera.SetActive(true);
                continue;
            }
            else
            {
                camera.SetActive(false);
                camera.tag = "Camera";
            }
        }
    }


    private void setAudioListeners()
    {
        foreach (GameObject camera in cameras)
        {
            if (!camera.Equals(currentCamera))
            {
                camera.GetComponent<AudioListener>().enabled = false;
            }
            else
            {
                camera.GetComponent<AudioListener>().enabled = true;
            }
        }
    }

    private void setCoordinatesFromCamera()
    {
        for (int i = 0; i < numCameras; i++)
        {
            cameraCoordinates[i] = cameras[i].gameObject.transform.position;
        }
    }

    private void setCameraFromCoordinates()
    {
        for (int i = 0; i < numCameras; i++)
        {
            cameras[i].transform.position = cameraCoordinates[i];
        }
    }

    public void setResolutionLow()
    {
        for (int i = 0; i < numCameras; i++)
        {
            cameras[i].GetComponent<Camera>().orthographicSize = 3;
        }
    }

    public void setResolutionMedium()
    {
        for (int i = 0; i < numCameras; i++)
        {
            cameras[i].GetComponent<Camera>().orthographicSize = 5;
        }
    }

    public void setResolutionHigh()
    {
        for (int i = 0; i < numCameras; i++)
        {
            cameras[i].GetComponent<Camera>().orthographicSize = 7;
        }
    }

    public void setResolution(float size)
    {
        for (int i = 0; i < numCameras; i++)
        {
            cameras[i].GetComponent<Camera>().orthographicSize = size;
        }
    }


    public void lockCameraPositions()
    {
        locked = true;
    }

    public void unlockCameraPositions()
    {
        locked = false;
    }

    void OnGUI()
    {
        if (fadeIn)
        {
            alpha += fadeDir * fadeSpeed * Time.deltaTime;

            alpha = Mathf.Clamp01(alpha);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        }

        if (fadeOut)
        {
            alpha -= fadeDir * fadeSpeed * Time.deltaTime;

            alpha = Mathf.Clamp01(alpha);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        }

    }

    public void beginFadeIn()
    {
        fadeIn = true;
        fadeOut = false;
        Invoke("unsetFadeIn", 5);
    }

    public void beginFadeOut()
    {
        fadeOut = true;
        fadeIn = false;
        alpha = 0.0f;
    }

    private void unsetFadeIn()
    {
        fadeIn = false;
    }

    private void unsetFadeOut()
    {
        fadeOut = false;
    }

}


