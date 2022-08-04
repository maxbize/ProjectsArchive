using UnityEngine;
using System.Collections;

// An item is a container to be picked up containing a weapon or powerup
public class Item : MonoBehaviour {

    public enum type { missile }
    public type myType;

	// Use this for initialization
	void Start () {
	
	}

    public void Init(type t) {
        myType = t;
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<Car>().addItem(myType);
            Destroy(gameObject);
        }

    }
}
