using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Level level;
    public LevelManager levelManager;


    // Turning variables
    private bool turning = false;
    private float turnThreshold = 0.5F;
    private float turnRate = 350F;
    private float fullTurn = 90;
    private Quaternion turnTarget;
    public bool facingUp = false;
    public bool facingDown = false;

    // Moving variables
    private bool moving = false;
    private float moveThreshold = 0.01F;
    private float moveRate = 6F;
    private Int3 moveTarget;

    // Inventory variables
    public int numDrills = 0;

    // Used at game end
    public bool controlEnabled = true;

	// Use this for initialization
	void Start () {
	    
	}

	// Update is called once per frame
	void Update () {

        if (!controlEnabled) {
            return;
        }

        // Basic rotation
        if (moving) {
            continueMove();
        }
        else if (turning) {
            continueTurn();
        }
        else {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                if (facingUp) {
                    startTurn(transform.up, -1, transform.right, 1);
                    facingUp = false;
                }
                else if (facingDown) {
                    startTurn(transform.up, -1, transform.right, -1);
                    facingDown = false;
                }
                else {
                    startTurn(transform.up, -1);
                }
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                if (facingUp) {
                    startTurn(transform.up, 1, transform.right, 1);
                    facingUp = false;
                }
                else if (facingDown) {
                    startTurn(transform.up, 1, transform.right, -1);
                    facingDown = false;
                }
                else {
                    startTurn(transform.up, 1);
                }
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !facingUp) {
                startTurn(transform.right, -1);
                if (facingDown) {
                    facingDown = false;
                }
                else {
                    facingUp = true;
                }
            }
            else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !facingDown) {
                startTurn(transform.right, 1);
                if (facingUp) {
                    facingUp = false;
                }
                else {
                    facingDown = true;
                }
            }
            // Will we allow the player to move backwards?            
            else if (Input.GetKey(KeyCode.Space)) {
                startMove();
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift)) {
                useDrill();
            }

        }

	}

    private void startMove() {
        Int3 targetPos = transform.position + transform.forward;
        if (level.canMoveHere(targetPos)) {
            SoundManager.playMoveSound(transform.position);
            moveTarget = targetPos;
            moving = true;
        }
    }

    private void continueMove() {
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveRate * Time.deltaTime);
        if ((transform.position - (Vector3)moveTarget).magnitude < moveThreshold) {
            levelManager.checkPosForEvents();
            moving = false;
        }

    }

    // The turn logic is really convoluted because world space turns only exist at the Transform level
    private void startTurn(Vector3 axis, int multiplier) {
        transform.Rotate(Utilities.roundedV3(axis), fullTurn * multiplier, Space.World);
        turnTarget = transform.rotation;
        transform.Rotate(Utilities.roundedV3(axis), fullTurn * -multiplier, Space.World);
        turning = true;

        continueTurn();
    }

    // This is to handle the case where we want to turn left or right while facing up or down
    // The purpose is to keep the player oriented towards the same ceiling/ground
    private void startTurn(Vector3 axis1, int multiplier1, Vector3 axis2, int multiplier2) {
        transform.Rotate(Utilities.roundedV3(axis1), fullTurn * multiplier1, Space.World);
        transform.Rotate(Utilities.roundedV3(axis2), fullTurn * multiplier2, Space.World);
        turnTarget = transform.rotation;
        transform.Rotate(Utilities.roundedV3(axis2), fullTurn * -multiplier2, Space.World);
        transform.Rotate(Utilities.roundedV3(axis1), fullTurn * -multiplier1, Space.World);
        turning = true;

        continueTurn();
    }



    private void continueTurn() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTarget, turnRate * Time.deltaTime);
        if (Quaternion.Angle(transform.rotation, turnTarget) < turnThreshold) {
            turning = false;
        }
    }

    private void useDrill() {
        Int3 target = transform.position + transform.forward;
        if (numDrills > 0 && level.canDestroyWall(target)) {
            numDrills--;
            level.destroyWall(target);
            SoundManager.playDrillSound(target);
        }
    }
}
