using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
    Item Manager
 
    The Item Manager manages all items on the stage. When the player touches these items, the Player Manager will change the player state depending on the item. Items may include weapons that unlock player attacks, coins as currency, and potions that power up the player. The Item Manager is initialized by the Scene Manager.
 
    Responsibilities include:
 
    Loading and creating all item elements
    Handling all entities with item tag
    Changing Player Manager’s state depending on their interactions with the item objects
    Disabling the item object from the entity list when the player touches the item
 
    Get Reference to all items currently in the scene  
    Spawn item at target location given item index
    Remove Item
*/

public class ItemManager : MonoBehaviour
{

    private LinkedList<GameObject> items = new LinkedList<GameObject>();
    private int numItems;

    // Need to be dragged in from Unity Editor
    public GameObject itemPrefab1;
    public GameObject itemPrefab2;
    public GameObject itemPrefab3;

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

        // Get reference to all items
        GameObject[] itemArray = GameObject.FindGameObjectsWithTag("Item");

        if (itemArray.Length != 0)
        {
            foreach (GameObject item in itemArray)
            {
                items.AddLast(item);
            }
        }

        // Set number of items
        numItems = items.Count;
    }


    // Spawn an item at target location given the item index
    // Item Index is determined by the prefab enemy number
    public void spawnItem(int itemIndex, Vector3 location)
    {
        switch (itemIndex)
        {
            case 1: items.AddLast(Instantiate(itemPrefab1, location, Quaternion.identity) as GameObject);
                numItems++;
                break;
            case 2: items.AddLast(Instantiate(itemPrefab2, location, Quaternion.identity) as GameObject);
                numItems++;
                break;
            case 3: items.AddLast(Instantiate(itemPrefab3, location, Quaternion.identity) as GameObject);
                numItems++;
                break;
            default: Debug.Log("Invalid item index");
                break;
        }
    }

    // Called by item controllers when the enemy is destroyed and playing the animation
    public void removeItem(GameObject item)
    {
        items.Remove(item);
        numItems--;
    }

    // Get Number of enemies currently in scene, used to display cleared scene
    public int numberOfItems()
    {
        return numItems;
    }
}
