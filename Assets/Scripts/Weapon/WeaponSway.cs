using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float walkSwayAmount;
    [SerializeField] private float walkSwaySpeed;
    [SerializeField] private float sprintSwayAmount;
    [SerializeField] private float sprintSwaySpeed;

    private float swayTime = 0f;

    private Rigidbody rb;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if(playerObject != null)
        {
            rb = playerObject.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Player GameObject not found with the 'Player' tag.");
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float playerSpeed = rb.velocity.magnitude;

        if (playerSpeed < 8f && playerSpeed > 1f)
        {
            swayTime += Time.deltaTime * walkSwaySpeed;
            float swayOffset = Mathf.Sin(swayTime) * walkSwayAmount;

            Quaternion swayRotation = Quaternion.Euler(0f, 0f, swayOffset);

            transform.localRotation = transform.localRotation * swayRotation;
        }
        Debug.Log(playerSpeed);
        if (playerSpeed > 8f)
        {
            swayTime += Time.deltaTime * sprintSwaySpeed;
            float swayOffset = Mathf.Sin(swayTime) * sprintSwayAmount;

            Quaternion swayRotation = Quaternion.Euler(0f, 0f, swayOffset);

            transform.localRotation = transform.localRotation * swayRotation;
        }
    }
}
