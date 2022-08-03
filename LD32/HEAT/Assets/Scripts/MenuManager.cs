using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    // Set in editor
    public RectTransform controlsMenu;

    void Start() {
    
    }

    void Update() {

    }

    public void OnClickPlay() {
        Application.LoadLevel("main");
    }

    public void OnClickControls() {
        controlsMenu.gameObject.SetActive(true);
    }

    public void OnDisableControls() {
        controlsMenu.gameObject.SetActive(false);
    }

    public void OnClickTwitter() {
        Application.OpenURL("https://www.twitter.com/intrepid_games");
    }
}
