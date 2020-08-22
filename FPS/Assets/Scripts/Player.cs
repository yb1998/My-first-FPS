using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;


    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Camera")]
    public float lookSensitivity;       // mouse look sensitivity
    public float maxLookX;              // lowest down we can look
    public float minLookX;              // highest up we can look
    private float rotX;                 // current x rotation of the camera

    private Camera cam;
    private Rigidbody rig;
    private Weapon weapon;

    void Awake()
    {
        //get the components
        cam = Camera.main;
        rig = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();

        // disable cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        // initalize the UI
        GameUI.instance.UpdateHealthBar(curHp, maxHp);
        GameUI.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);
        GameUI.instance.UpdateScoreText(0);
    }

    void Update()
    {
        
        //don't do anything if game is paused
        if(GameManager.instance.gamePaused == true)
            return;

        Move();

        if (Input.GetButtonDown("Jump"))
            TryJump();

        if (Input.GetButton("Fire1") && weapon.CanShoot())
            weapon.Shoot();
            
        CamLook();
    }

    void Move()
    {
        //gets the x an z axis inputs
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        Vector3 dir = transform.right * x + transform.forward * z;
        dir.y = rig.velocity.y;

        //adds inputs to body velocity
        rig.velocity = dir;
    }

    // rotate the camera based on mouse movement
    void CamLook ()
    {
        // get mouse inputs
        float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotX += Input.GetAxis("Mouse Y") * lookSensitivity;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        // rotate the camera and player
        cam.transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
        transform.eulerAngles += Vector3.up * y;
    }

    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, 1.1f))
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        GameUI.instance.UpdateHealthBar(curHp, maxHp);

        if(curHp <= 0)
            Die();
    }

    void Die()
    {
        GameManager.instance.LoseGame();
    }

    public void GiveHealth(int amountToGive)
    {
        curHp = Mathf.Clamp(curHp + amountToGive, 0, maxHp);

        GameUI.instance.UpdateHealthBar(curHp, maxHp);
    }

    public void GiveAmmo(int amountToGive)
    {
        weapon.curAmmo = Mathf.Clamp(weapon.curAmmo + amountToGive, 0, weapon.maxAmmo);

        GameUI.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);
    }

}
