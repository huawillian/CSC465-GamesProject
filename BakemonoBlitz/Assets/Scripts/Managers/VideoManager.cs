using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
    }
}
