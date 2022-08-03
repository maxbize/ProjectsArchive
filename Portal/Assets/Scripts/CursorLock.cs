using UnityEngine;
using System.Collections;

public class CursorLock : MonoBehaviour
{

    private bool wasLocked = false;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Screen.lockCursor = true;
        }

        if (Input.GetKeyDown("escape"))
            Screen.lockCursor = false;

        if (!Screen.lockCursor && wasLocked) {
            wasLocked = false;
        }
        else
            if (Screen.lockCursor && !wasLocked) {
                wasLocked = true;
            }
    }
}