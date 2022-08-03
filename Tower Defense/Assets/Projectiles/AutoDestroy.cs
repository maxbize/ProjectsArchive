using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        // Automatically destroy the gameObject in one second. Used for particle systems
        Destroy(gameObject, 1.0f);
    }
}
