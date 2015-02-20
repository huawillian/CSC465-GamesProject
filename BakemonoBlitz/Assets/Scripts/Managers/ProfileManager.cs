using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class ProfileManager : MonoBehaviour
{
    private PlayerManager mPlayerManager;
    private SceneManager mSceneManager;
    private CameraManager mCameraManager;

    // This is our local private members 
    string _FileLocation, _FileName;
    string _data;

    public int _ProfileNumber;

    UserData myData;

    // When the EGO is instansiated the Start will trigger 
    // so we setup our initial values for our local members 
    void Start()
    {
    }

    /* The following metods came from the referenced URL */
    string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Here we serialize our UserData object of myData 
    string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form 
    object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }

    // Finally our save and load methods for the file itself 
    void CreateXML()
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(_FileLocation + "\\" + _FileName + _ProfileNumber + ".xml");
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(_data);
        writer.Close();
        Debug.Log("File written.");
    }

    void LoadXML()
    {
        StreamReader r = File.OpenText(_FileLocation + "\\" + _FileName + _ProfileNumber + ".xml");
        string _info = r.ReadToEnd();
        r.Close();
        _data = _info;
        Debug.Log("File Read");
    }

    // Method called by Scene Manager to save the current state to XML fie
    // Must pass in an integer between 1 and 3 (inclusive)
    // Save to xml file SaveFile1.xml
    // or SaveFile2.xml or SaveFile3.xml
    public void SaveProfile(int profileNumber)
    {
        if (profileNumber >= 0 && profileNumber <= 3)
        {
            this._ProfileNumber = profileNumber;

            myData._userData.gender = mPlayerManager.gender;
            myData._userData.health = mPlayerManager.health;
            myData._userData.lives = mPlayerManager.lives;
            myData._userData.energy = mPlayerManager.energy;

            if (mPlayerManager.weapon1)
            {
                myData._userData.weapon1 = "yes";
            }
            else
            {
                myData._userData.weapon1 = "no";
            }
            if (mPlayerManager.weapon2)
            {
                myData._userData.weapon2 = "yes";
            }
            else
            {
                myData._userData.weapon2 = "no";
            }
            if (mPlayerManager.weapon3)
            {
                myData._userData.weapon3 = "yes";
            }
            else
            {
                myData._userData.weapon3 = "no";
            }


            myData._userData.x = mPlayerManager.x;
            myData._userData.y = mPlayerManager.y;
            myData._userData.z = mPlayerManager.z;
            myData._userData.gems = mPlayerManager.gems;

            myData._userData.volume = mSceneManager.mVolume;
            myData._userData.resolution = mSceneManager.mResolution;

            myData._userData.sceneNumber = mSceneManager.mSceneNumber;
            myData._userData.checkpointNumber = mSceneManager.mCheckpointNumber;

            // Time to creat our XML! 
            _data = SerializeObject(myData);
            // This is the final resulting XML from the serialization process 
            CreateXML();
            Debug.Log(_data);

        }
        else
        {
            Debug.Log("Attempting to Save... Invalid Profile Number");
        }
    }

    public UserData getProfileData(int profileNumber)
    {
        if (profileNumber >= 0 && profileNumber <= 3)
        {
            StreamReader r = File.OpenText(_FileLocation + "\\" + _FileName + profileNumber + ".xml");
            string _info = r.ReadToEnd();
            r.Close();

            if (_info.ToString() != "")
            {
                // notice how I use a reference to type (UserData) here, you need this 
                // so that the returned object is converted into the correct type 
                return (UserData)DeserializeObject(_info);
            }
        }

        return null;
    }

    // Load Profile given profile number from 1 to 3 inclusive
    public void LoadProfile(int profileNumber)
    {
        if (profileNumber >= 0 && profileNumber <= 3)
        {
            this._ProfileNumber = profileNumber;

            // Load our UserData into myData 
            LoadXML();

            // Set variables in game
            if (_data.ToString() != "")
            {
                // notice how I use a reference to type (UserData) here, you need this 
                // so that the returned object is converted into the correct type 
                myData = (UserData)DeserializeObject(_data);

                mPlayerManager.health = myData._userData.health;
                mPlayerManager.lives = myData._userData.lives;
                mPlayerManager.energy = myData._userData.energy;

                if (myData._userData.weapon1.Equals("yes"))
                {
                    mPlayerManager.weapon1 = true;
                }
                else
                {
                    mPlayerManager.weapon1 = false;
                }
                if (myData._userData.weapon2.Equals("yes"))
                {
                    mPlayerManager.weapon2 = true;
                }
                else
                {
                    mPlayerManager.weapon2 = false;
                }
                if (myData._userData.weapon3.Equals("yes"))
                {
                    mPlayerManager.weapon3 = true;
                }
                else
                {
                    mPlayerManager.weapon3 = false;
                }

                mPlayerManager.x = myData._userData.x;
                mPlayerManager.y = myData._userData.y;
                mPlayerManager.z = myData._userData.z;

                mPlayerManager.gems = myData._userData.gems;
                mPlayerManager.gender = myData._userData.gender;

                mSceneManager.mVolume = myData._userData.volume;
                mSceneManager.mResolution = myData._userData.resolution;
                mSceneManager.mSceneNumber = myData._userData.sceneNumber;
                mSceneManager.mCheckpointNumber = myData._userData.checkpointNumber;

            }
        }
    }

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
        this.mPlayerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        this.mSceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        this.mCameraManager = GameObject.Find("Camera Manager").GetComponent<CameraManager>();


        // Where we want to save and load to and from 
        _FileLocation = Application.dataPath + "/Profiles/";
        _FileName = "SaveData";

        // we need soemthing to store the information into 
        myData = new UserData();
    }

    public void setVolume(float vol)
    {
        Debug.Log("Setting volume to: " + vol);
        AudioListener.volume = vol;
        mSceneManager.mVolume = vol;
        this.myData._userData.volume = vol;
    }

    public void setResolution(float res)
    {
        Debug.Log("Setting resolution to: " + res);
        mCameraManager.setResolution(res);
        mSceneManager.mResolution = res;
        this.myData._userData.resolution = res;
    }
}

// UserData is our custom class that holds our defined objects we want to store in XML format 
public class UserData
{
    // We have to define a default instance of the structure 
    public Data _userData;
    // Default constructor doesn't really do anything at the moment 
    public UserData() { }

    // Anything we want to store in the XML file, we define it here 
    public struct Data
    {
        public string gender;   // male or female

        public int health; // 0 to 100
        public int lives; // 0 to 3
        public int energy; // 0 to 100

        public string weapon1; // yes or no
        public string weapon2; // yes or no
        public string weapon3; // yes or no

        public float x; // #.##...
        public float y; // #.##...
        public float z; // #.##...

        public int gems; // #

        public float volume; // #.##...
        public float resolution; // #.##...

        public int sceneNumber; // #
        public int checkpointNumber; // #
    }

}
