using UnityEngine;
using System.Collections;

// Name: Willian Hua
// Date: 11/24/14
// Project: Attack on Ninja, Team Koga
// Description: To be used by the InputController as an interface to the Trigger buttons on the XBox Controller
// Usage: DTriggerButtons.LT, TriggerButtons.RT
public class TriggerButtons : MonoBehaviour
{
	public static bool LT;
	public static bool RT;
	
	float lastTrigger;

	// Must have an Input Setting named "Trigger Axis" configured properly in Project Settings
	void Awake()
	{
		LT = RT = false;
		lastTrigger = Input.GetAxis ("Trigger Axis");
	}
	
	void Update()
	{
		// Get button down event only
		// Exclude repeats and holds
		if(Input.GetAxis ("Trigger Axis") == 1 && lastTrigger != 1) { LT = true; } else { LT = false; }
		if(Input.GetAxis ("Trigger Axis") == -1 && lastTrigger != -1) { RT = true; } else { RT = false; }
		
		lastTrigger = Input.GetAxis ("Trigger Axis");
	}
}