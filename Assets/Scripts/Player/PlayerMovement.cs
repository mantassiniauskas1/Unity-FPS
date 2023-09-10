using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Stamina")]
    public float staminaMax = 100f;
    public float sprintCostPerSecond = 10f;
    public float sprintRegenRate = 20f;

    private float stamina;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool readyToJump;
    bool grounded;
    bool isSprinting;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        stamina = staminaMax;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (!isSprinting && stamina < staminaMax)
            {
                stamina += sprintRegenRate * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0f, staminaMax);
            }
        
        if (isSprinting)
        {
            stamina -= sprintCostPerSecond * Time.deltaTime;
            if (stamina <= 0f)
            {
                isSprinting = false;
            }
        }


    }

    private void FixedUpdate()
    {
        MovePlayer();

        float speed = rb.velocity.magnitude;

        Debug.Log("Speed: " + speed + " isSprinting? " + isSprinting + " Stamina " + stamina);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input. GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(sprintKey) && stamina > 0f)
        {
            isSprinting = true;
        }

        if (Input.GetKeyUp(sprintKey) || stamina <= 0f)
        {
            isSprinting = false;
        }
}

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isSprinting && grounded)
            rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        float currentMaxSpeed = isSprinting ? sprintSpeed : moveSpeed;

        if(flatVel.magnitude > currentMaxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentMaxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
       rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z) ;

       rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}