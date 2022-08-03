using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    private Text nameText;

	// Use this for initialization
	void Start () {
	    nameText = GameObject.Find("Name Text").GetComponent<Text>();
        nameText.text = PlayerPrefs.GetString(PlayerPrefsKeys.displayName, "Player");
	}

    public void startGame() {
        PlayerPrefs.SetString(PlayerPrefsKeys.displayName, nameText.text);
        
        // Extract and save the weapon choice. As of 10/19 there's no API for this...
        Toggle[] weaponToggles = FindObjectsOfType<Toggle>();
        int weaponId = 1;
        foreach (Toggle toggle in weaponToggles) {
            if (toggle.isOn) {
                string txt = toggle.GetComponentInChildren<Text>().text;
                weaponId = (int)(char.GetNumericValue(txt[txt.Length - 1]));
                Debug.Log(weaponId);
            }
        }

        PlayerPrefs.Save();

        Application.LoadLevel("Game");
    }


}
