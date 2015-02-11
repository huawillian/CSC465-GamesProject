using UnityEngine;
using System.Collections;

/*
    To implement the features described in the previous section, we create the following classes:
 
    InputManager.cs
    DPadButtons.cs
    TriggerButtons.cs
    MenuController.cs
 
    The Input Manager serves as the interface between the input received from the Input and the various classes in the game that use the input. The Input Manager will have several flags that other classes can use to accommodate their functions. The flags may include:
 
    Button Down
    Button Up
    Button Hold
 
    For Buttons, {A, B, X, Y, LB, RB, Start, Left Trigger, Right Trigger, D-Pad Left, D-Pad Right, D-Pad Up, D-Pad Down}.
 
    For Axis of values between -1 and 1 or 0 and 1, {Left Analog Horizontal, Left Analog Vertical, Left Trigger, Right Trigger, Right Analog Horizontal, and Right Analog Vertical}.
 
    The Input Manager class uses the DPadButtons helper class to translate D-Pad Horizontal and D-Pad Vertical Axis inputs into Left, Right, Up, and Down button presses.
    The Input Manager class uses the TriggerButtons helper class to translate Left Trigger Axis and Right Trigger Axis into Left and Right Trigger button presses.
 
    The Input Manager initializes the MenuController class. This class will only be initialized if the Scene Manager is present. The MenuController class controls the in-game pause menu. It displays and handles any setting or game changes.
*/
 
public class InputManager : MonoBehaviour {

    // FLAGS used by other classes
    public bool A, B, X, Y, START, LT, RT;
    public float LX, LY;
    public float LTA, RTA;
    public bool LTH, RTH;

    private float holdThreshold = 0.25f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Check button presses
        checkA();
        checkB();
        checkX();
        checkY();
        checkStart();
        checkLT();
        checkRT();

        // Update Axis for LX,LY, LTA, and RTA
        setLX();
        setLY();
        setLTA();
        setRTA();

        // Check holding down for LT and RT
        checkLTH();
        checkRTH();
	}

    // Initialization called by Scene Manager
    public void InitializeManager()
    {
        Debug.Log("Initializing " + this.gameObject.name);
    }

    // Helper Methods
    private void checkA()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetButtonDown("A"))
        {
            Debug.Log("A has been pressed");
            A = true;
        }

        if (Input.GetKeyUp(KeyCode.Keypad2) || Input.GetButtonUp("A"))
        {
            A = false;
        }
    }

    private void checkB()
    {
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetButtonDown("B"))
        {
            Debug.Log("B has been pressed");
            B = true;
        }

        if (Input.GetKeyUp(KeyCode.Keypad6) || Input.GetButtonUp("B"))
        {
            B = false;
        }
    }

    private void checkX()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetButtonDown("X"))
        {
            Debug.Log("X has been pressed");
            X = true;
        }

        if (Input.GetKeyUp(KeyCode.Keypad4) || Input.GetButtonUp("X"))
        {
            X = false;
        }
    }

    private void checkY()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetButtonDown("Y"))
        {
            Debug.Log("Y has been pressed");
            Y = true;
        }

        if (Input.GetKeyUp(KeyCode.Keypad8) || Input.GetButtonUp("Y"))
        {
            Y = false;
        }
    }

    private void checkStart()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetButtonDown("Start"))
        {
            Debug.Log("Start has been pressed");
            START = true;
        }

        if (Input.GetKeyUp(KeyCode.Keypad5) || Input.GetButtonUp("Start"))
        {
            START = false;
        }
    }

    private void checkLT()
    {
        if (Input.GetKeyDown(KeyCode.Q) || TriggerButtons.LT)
        {
            Debug.Log("LT has been pressed");
            LT = true;
        }

        if (Input.GetKeyUp(KeyCode.Q) || TriggerButtons.LT)
        {
            LT = false;
        }
    }

    private void checkRT()
    {
        if (Input.GetKeyDown(KeyCode.E) || TriggerButtons.RT)
        {
            Debug.Log("RT has been pressed");
            RT = true;
        }

        if (Input.GetKeyUp(KeyCode.E) || TriggerButtons.RT)
        {
            RT = false;
        }
    }

    private void checkLTH()
    {
        if (LTA > holdThreshold) LTH = true;
        else LTH = false;
    }

    private void checkRTH()
    {
        if (RTA > holdThreshold) RTH = true;
        else RTH = false;
    }

    private void setLX()
    {
        if (Input.GetKey(KeyCode.A)) LX = -1.0f;
        else 
            if (Input.GetKey(KeyCode.D)) LX = 1.0f;
        else
                LX = LX = Input.GetAxis("Left Analog Stick X");
    }

    private void setLY()
    {
        if (Input.GetKey(KeyCode.S)) LY = -1.0f;
        else
            if (Input.GetKey(KeyCode.W)) LY = 1.0f;
            else
                LY = LY = Input.GetAxis("Left Analog Stick Y");
    }

    private void setLTA()
    {
        if (Input.GetKey(KeyCode.Q)) LTA = 1.0f;
        else
            LTA = Input.GetAxis("Left Trigger Axis");
    }

    private void setRTA()
    {
        if (Input.GetKey(KeyCode.E)) RTA = 1.0f;
        else
            RTA = Input.GetAxis("Right Trigger Axis");
    }
}
