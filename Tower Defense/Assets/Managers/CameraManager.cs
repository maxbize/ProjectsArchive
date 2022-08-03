using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    Vector3 mousePos;
    float threshold = 50f; // Num pixels from the edge required for camera movement
    float minSpeed = 50f;   // Min speed camera will move
    float maxSpeed = 600f;  // Max speed camera will move
    Vector4 bounds = new Vector4(300, -300, -600, -150); // L/R/B/T bounds for camera position

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        if (mousePos.x < threshold && transform.position.z < bounds[0]) // LEFT
        {
            moveCam(Vector3.forward, threshold - mousePos.x);
        }
        else if (mousePos.x > Screen.width - threshold && transform.position.z > bounds[1]) // RIGHT
        {
            moveCam(Vector3.back, mousePos.x - (Screen.width - threshold));
        }
        if (mousePos.y < threshold && transform.position.x > bounds[2]) // DOWN
        {
            moveCam(Vector3.left, threshold - mousePos.y);
        }
        else if (mousePos.y > Screen.height - threshold && transform.position.x < bounds[3]) // UP
        {
            moveCam(Vector3.right, mousePos.y - (Screen.height - threshold));
        }
    }

    // magInThresh == how far past the threshold is the cursor?
    void moveCam(Vector3 dir, float magInThresh)
    {
        transform.position += dir * Time.deltaTime * Mathf.Lerp(minSpeed, maxSpeed, magInThresh/threshold);
    }
}
