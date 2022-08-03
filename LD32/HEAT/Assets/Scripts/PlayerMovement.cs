using UnityEngine;
using System.Collections;

// Handles player's movement
public class PlayerMovement : MonoBehaviour {

    // Set in editor
    public float moveAcceleration;
    public float lookAcceleration;
    public float horizontalBounds;
    public float verticalBounds;

    // For storyManager
    [HideInInspector]
    public bool canMove = true;

    private Rigidbody myRb;
    private Vector3 moveInput;
    private Vector3 mouseHitPosition = Vector3.forward;

    private const int LEFT_CLICK = 0;

	// Use this for initialization
	void Start () {
        myRb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove) {
            GetKeyboardInput();
            GetMouseInput();
            Move();
            Rotate();
        }
    }

    // Keyboard input - WASD + arrows
    private void GetKeyboardInput() {
        moveInput = Vector2.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveInput -= Vector3.right;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveInput += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveInput -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveInput += Vector3.forward;
        }
        if (moveInput.sqrMagnitude > 0) {
            moveInput = moveInput.normalized * moveAcceleration;
        }
    }

    private void GetMouseInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayCastHit;
        if (Physics.Raycast(ray, out rayCastHit, Mathf.Infinity)) {
            mouseHitPosition = rayCastHit.point;
        }
    }

    private void Move() {
        myRb.AddForce(moveInput);

        if (transform.position.x > horizontalBounds) {
            transform.position = new Vector3(horizontalBounds, 0, transform.position.z);
        }
        else if (transform.position.x < -horizontalBounds) {
            transform.position = new Vector3(-horizontalBounds, 0, transform.position.z);
        }

        if (transform.position.z > verticalBounds) {
            transform.position = new Vector3(transform.position.x, 0, verticalBounds);
        }
        else if (transform.position.z < -verticalBounds) {
            transform.position = new Vector3(transform.position.x, 0, -verticalBounds);
        }
    }

    private void Rotate() {
        Vector3 lookTarget = mouseHitPosition - transform.position;
        lookTarget.y = 0;
        myRb.rotation = Quaternion.RotateTowards(myRb.rotation, Quaternion.LookRotation(lookTarget), lookAcceleration);
    }
}
