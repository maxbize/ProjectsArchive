using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{

    public Transform player;
    public Transform cam;

    private CharacterController playerController;
    private float maxPlayerSpeed = 2f;

    private Quaternion lastRotation = Quaternion.identity;
    private float rotateScale;
    private float moveScale;
    private float minScale = 0.02f;
    private float maxScale = 1;
    private float scaleForAim = 0;
    private float crosshairDiameter = 0.28f;
    

    private bool isZoomed = false;
    private float zoomInFOV = 30;
    private float zoomOutFOV = 60;

    private float reloadTime = 1;
    private const int MAX_AMMO = 5;
    private int ammo = MAX_AMMO;
    private bool reloading = false;
    private float reloadStartTime;
    private float fireRate = 0.55f;
    private float lastShotTime = 0;

    private UIManager uiManager;
    private BulletManager bulletManager;

    enum mouseButtons { left = 0, right = 1 };

    // Use this for initialization
    void Start() {
        playerController = player.GetComponent<CharacterController>();
        bulletManager = GameObject.FindObjectOfType<BulletManager>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        uiManager.updateAmmoText(ammo, MAX_AMMO);
    }

    // Update is called once per frame
    void Update() {
        if (!networkView.isMine) {
            return;
        }

        float playerSpeed = playerController.velocity.magnitude;

        moveScale = playerSpeed / maxPlayerSpeed * 0.3f;
        
        rotateScale += Quaternion.Angle(lastRotation, cam.rotation) * 6 * Time.deltaTime;
        rotateScale -= 2.3f * Time.deltaTime;
        if (rotateScale < 0) { rotateScale = 0; }
        if (rotateScale > 1.5f) { rotateScale = 1.5f; }

        float scale = new Vector2(moveScale, rotateScale).magnitude;
        if (scale > maxScale) { scale = maxScale; }
        scaleForAim = (scale > 0 ? scale : 0);
        if (scale < minScale) { scale = minScale; }

        transform.localScale = scale * new Vector3(1, 1, 1);

        lastRotation = cam.rotation;

        if (Input.GetMouseButtonDown((int)mouseButtons.left)) {
            if (Time.timeSinceLevelLoad - lastShotTime > fireRate && ammo > 0 && !reloading) {
                localShoot();
                lastShotTime = Time.timeSinceLevelLoad;
            }
        }

        if (Input.GetMouseButtonDown((int)mouseButtons.right)) {
            if (isZoomed) {
                transform.parent.camera.fieldOfView = zoomOutFOV;
            }
            else {
                transform.parent.camera.fieldOfView = zoomInFOV;
            }
            isZoomed = !isZoomed;
        }

        if (Input.GetKeyDown(KeyCode.R) && ammo != MAX_AMMO) {
            reloading = true;
            reloadStartTime = Time.timeSinceLevelLoad;
        }

        if (reloading && Time.timeSinceLevelLoad - reloadStartTime > reloadTime) {
            reloading = false;
            ammo = MAX_AMMO;
            uiManager.updateAmmoText(ammo, MAX_AMMO);
        }
    }


    private void localShoot() {
        ammo--;
        uiManager.updateAmmoText(ammo, MAX_AMMO);
        Vector3 offset = Random.insideUnitSphere * crosshairDiameter * scaleForAim;
        offset.y = 0;
        offset = transform.TransformDirection(offset);
        Vector3 dir = transform.position - cam.position + offset;
        rotateScale = 1.5f;
        bulletManager.networkView.RPC("shoot", RPCMode.All, cam.position, dir, player.GetComponent<Player>().id);
    }

}
