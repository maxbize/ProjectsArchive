using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public string nextScene;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Space)) {
            Application.LoadLevel(nextScene);
        }
	}
}
