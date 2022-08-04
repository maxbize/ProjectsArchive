using UnityEngine;
using System.Collections.Generic;

// This class manages dropping items for players to pick up.
//  In this class, an 'item' is something like a weapon or powerup that is placed
//  on the map for the user to get and use
public class ItemsManager : MonoBehaviour
{

    public Transform itemPrefab;

    public float minTimeBetweenItems = 10;
    public float maxTimeBetweenItems = 20;
    public int maxItemsOnScreen = 4;

    float timeToNextDrop = 3;

    List<AI_Waypoint> waypoints;


    // Use this for initialization
    void Start() {
        waypoints = AI_CarController.getAIWaypoints();
    }

    // Update is called once per frame
    void Update() {
        timeToNextDrop -= Time.deltaTime;

        if (timeToNextDrop < 0) {
            timeToNextDrop = Random.Range(minTimeBetweenItems, maxTimeBetweenItems);

            Item[] itemsOnScreen = GameObject.FindObjectsOfType<Item>();
            if (itemsOnScreen.Length < maxItemsOnScreen) {
                Vector3 dropLoc = Vector3.zero;
                bool validLoc = false;

                while (!validLoc) {
                    // Get the drop location by picking a random point along the AI waypoints.
                    //  Note that this method will have variable density across the maps, with 
                    //  more items appearing in areas that have lots of turns
                    Vector3 left, right;
                    int baseWpInd = Random.Range(0, waypoints.Count);
                    left = waypoints[baseWpInd].transform.position;
                    right = waypoints[(baseWpInd == waypoints.Count - 1 ? 0 : baseWpInd + 1)].transform.position;
                    dropLoc = left + ((right - left) * Random.Range(0f, 1f));

                    validLoc = true;
                    foreach (Item i in itemsOnScreen) {
                        if ((i.transform.position - dropLoc).magnitude < i.GetComponent<BoxCollider2D>().size.x) {
                            validLoc = false;
                            break;
                        }
                    }
                }

                Transform item = (Transform)Instantiate(itemPrefab, dropLoc, Quaternion.identity);
                item.GetComponent<Item>().myType = Item.type.missile;
                
            }
        }


    }
}
