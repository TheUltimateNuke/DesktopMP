using UnityEngine;
using System;
using Desktop_MP;

public class FPSController : MonoBehaviour
{
    public FPSController(IntPtr ptr) : base(ptr) { }

    public GameObject Volumerenderer;

    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 8f;
    public GameObject feet;
    public GameObject arm;
    public Animator playerAnimator;

    public float mouseSensitivity = 2f;
    public float minLookAngle = -90f;
    public float maxLookAngle = 90f;
    public float armXRotOffset = 0f;
    public float armYRotOffset = 2f;
    public float armZRotOffset = 0f;
    public float armYRotDiv = 2f;

    private Camera playerCamera;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalRotation = 0f;
    private Rigidbody rb;
    
    public bool IsGrounded()
    {
        if (Physics.Raycast(feet.transform.position, Vector3.down, 0.1f))
        {
            return true;
        }

        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Volumerenderer = gameObject.transform.Find("Camera").GetChild(1).gameObject;
    }

    public int GetVertical() 
    { 
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) { return 0; }
        else if (Input.GetKey(KeyCode.W)) { return 1; }
        else if (Input.GetKey(KeyCode.S)) { return -1; }
        else { return 0; }
    }

    public int GetHorizontal()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) { return 0; }
        else if (Input.GetKey(KeyCode.D)) { return 1; }
        else if (Input.GetKey(KeyCode.A)) { return -1; }
        else { return 0; }
    }

    private void FixedUpdate()
    {
        // Movement
        float moveSpeedMultiplier = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        float horizontalInput = GetHorizontal() * moveSpeedMultiplier;
        float verticalInput = GetVertical() * moveSpeedMultiplier;

        moveDirection = new Vector3(horizontalInput, moveDirection.y, verticalInput);
        moveDirection = transform.TransformDirection(moveDirection);
        if (IsGrounded())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce);
            }
        }
        rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime);

        if (Input.GetKeyDown(KeyCode.H)) { 
            gameObject.transform.position = BoneLib.Player.playerHead.transform.position + new Vector3(0.3f,0,0);
            
        }
    }

    private void Update()
    {
        playerAnimator.SetFloat("InputStrength", Mathf.Abs(GetHorizontal() + GetVertical()));
        // Mouse Look
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0f, horizontalRotation, 0f);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        if (playerAnimator.GetCurrentAnimatorClipInfo(1)[0].clip.name != "HandIdle")
        {
            arm.transform.localRotation = Quaternion.Euler(armXRotOffset * verticalRotation, (verticalRotation / armYRotDiv) + armYRotOffset, armZRotOffset * verticalRotation);
        }

        else
        {
            arm.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Mod.doVolumetrics.Value == true)
        {
            Volumerenderer.SetActive(true);
        }
        else
        {
            Volumerenderer.SetActive(false);
        }
    }
}
