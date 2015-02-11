using UnityEngine;
using System.Collections;

/*
    Item Manager
 
    The Item Manager manages all items on the stage. When the player touches these items, the Player Manager will change the player state depending on the item. Items may include weapons that unlock player attacks, coins as currency, and potions that power up the player. The Item Manager is initialized by the Scene Manager.
 
    Responsibilities include:
 
    Loading and creating all item elements
    Handling all entities with item tag
    Changing Player Manager’s state depending on their interactions with the item objects
    Disabling the item object from the entity list when the player touches the item
*/

public class ItemManager : MonoBehaviour {

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
