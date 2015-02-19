using UnityEngine;
using System.Collections;

public class something : MonoBehaviour
{
	Texture2D mTex;
    int allImages;
    int i = 1;
    bool done = false;
    string currentFile = "";
	// Use this for initialization
	void Start () {
        allImages = Resources.LoadAll<Texture2D>("Videos").Length - 1;
        		print(allImages.ToString());

                InvokeRepeating("change", 0.5f, 0.0422f);



	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!done)
        {
            int numberDigits = 5;
			string digits = "";

			if(i > 237){
				done = true;
			}
            
			if(i < 10 && i >= 0){
				for(int w=  0; w < numberDigits-1; w++){
					digits = digits + "0";
				}
				digits = digits + i;
			}
			if(i < 100 && i >= 10){
				for(int x = 0; x < numberDigits-2; x++){
					digits = digits + "0";
				}
				digits = digits + i;
			}
			if (i < 1000 && i >= 100){
				for(int y = 0; y < numberDigits-3; y++){
					digits = digits + "0";
				}
				digits = digits + i;
			}
			if (i < 10000 && i >= 1000){
				for(int z = 0; z < numberDigits-4; z++){
					digits = digits + "0";
				}
				digits = digits + i;
			}

			currentFile = "Videos"+"/"+"scene"+ digits;
        }
	}

    void change()
    {
        i += 1;
        			print(currentFile);

                    Texture2D videoTexture = Resources.Load<Texture2D>(currentFile);

			mTex = videoTexture;
    }


	void OnGUI(){
		GUI.DrawTexture(new Rect(0,0,480,360),mTex);//480x360 is size of game in PlayerSettings.
		//check resolution by going to Edit>Project Settings>Player and look under Resolution
	}
	
}
