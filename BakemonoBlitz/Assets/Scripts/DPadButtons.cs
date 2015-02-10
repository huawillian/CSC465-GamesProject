using UnityEngine;
using System.Collections;

// Name: Willian Hua
// Date: 11/24/14
// Project: Attack on Ninja, Team Koga
// Description: To be used by the InputController as an interface to the DPad on the XBox Controller
// Usage: DPadButtons.up, DPadButtons.down, DPadButtons.left, DPadButtons.right

public class DPadButtons : MonoBehaviour
{
	public static bool up;
	public static bool down;
	public static bool left;
	public static bool right;
	
	float lastX;
	float lastY;

	// Must have Input Settings "D Pad X" and "D Pad Y" configured properly in Project Settings
	void Awake()
	{
		up = down = left = right = false;
		lastX = Input.GetAxis("D Pad X");
		lastY = Input.GetAxis("D Pad Y");
	}
	
	void Update()
	{
		// Get button down event only
		// Exclude repeats and holds
		if(Input.GetAxis ("D Pad X") == 1 && lastX != 1) { right = true; } else { right = false; }
		if(Input.GetAxis ("D Pad X") == -1 && lastX != -1) { left = true; } else { left = false; }
		if(Input.GetAxis ("D Pad Y") == 1 && lastY != 1) { up = true; } else { up = false; }
		if(Input.GetAxis ("D Pad Y") == -1 && lastY != -1) { down = true; } else { down = false; }

		lastX = Input.GetAxis("D Pad X");
		lastY = Input.GetAxis("D Pad Y");
	}
}