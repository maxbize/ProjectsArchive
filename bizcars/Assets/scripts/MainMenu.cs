using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool vsAi = false;

    public void StartMap(int mapNum, bool vsAi) {
        MainMenu.vsAi = vsAi;
        SceneManager.LoadScene($"Map{mapNum}");
    }

    public void StartMap1Ai() {
        StartMap(1, true);
    }

    public void StartMap1Ghost() {
        StartMap(1, false);
    }

    public void StartMap2Ai() {
        StartMap(2, true);
    }

    public void StartMap2Ghost() {
        StartMap(2, false);
    }

}
