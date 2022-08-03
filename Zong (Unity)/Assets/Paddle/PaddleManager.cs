using UnityEngine;

public class PaddleManager : MonoBehaviour
{

    private float deltaX, deltaY;
    private float speed;
    private Vector3 focus;

    // Mouse smoothing vars
    private int smoothFrameDelay = 5;
    private float[] mouseXArray;
    private float[] mouseYArray;

    public float thickness { get; private set; }
    public Vector3 rotationVect { get; private set; }


    void Start()
    {
        GameStart();
        thickness = transform.renderer.bounds.size.z/2f;
        RefreshMouseSmoothingArray();
    }

    // Update is called once per frame
    void Update()
    {
        // Orbit the camera about the centroid
        deltaX = Input.GetAxis("Mouse X");
        deltaY = Input.GetAxis("Mouse Y");

        //distribute mouseinput to the next few frames to smooth it
        for (int i = 0; i < mouseXArray.Length; i++) { mouseXArray[i] += deltaX / mouseXArray.Length; }
        for (int i = 0; i < mouseYArray.Length; i++) { mouseYArray[i] += deltaY / mouseXArray.Length; }
        rotationVect = mouseXArray[0] * -transform.up + mouseYArray[0] * transform.right;
        //transform.Translate(centroid - oldCentroid);
        transform.LookAt(focus, transform.up);
        transform.RotateAround(focus, rotationVect, rotationVect.magnitude * speed);
        focus += (LevelManager.GetCentroid() - focus) * 0.05f; // Smooth the camera transition to the new centroid
        ShiftMouseSmoothArray(mouseXArray);
        ShiftMouseSmoothArray(mouseYArray);
    }

    void GameStart()
    {
        speed = 3f;
        focus = LevelManager.GetCentroid();
    }

    public Vector3 GetBallStuckPosition(float ballRadius)
    {
        return transform.position + GetCenterToCentroid().normalized * (thickness + ballRadius);
    }

    public Vector3 GetCenterToCentroid()
    {
        return focus - transform.position;
    }

    private void ShiftMouseSmoothArray(float[] array)
    {
        //shift left
        for (int i = 0; i < array.Length - 1; i++)
        {
            array[i] = array[i + 1];
        }
        //new array position is set to 0F
        array[array.Length - 1] = 0F;
    }

    //refresh if different smoothFrameDelay is choosen
    public void RefreshMouseSmoothingArray()
    {
        mouseXArray = new float[smoothFrameDelay];
        mouseYArray = new float[smoothFrameDelay];
        for (int i = 0; i < mouseXArray.Length; i++)
        {
            mouseXArray[i] = 0F;
            mouseYArray[i] = 0F;
        }
    }
}
