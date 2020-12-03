using System;
using UnityEngine;


[RequireComponent(typeof (CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    private float moveSpeed = 5;
    private float jumpSpeed = 5;
    private float gravityStrength = 2;

    private const float STICK_TO_GROUND_FORCE = 10;

    private Vector2 mouseSensitivity = new Vector2(2, 2);
    private Vector2 currentEulerAngle = new Vector2(0, 90);

    private bool jumpRequested = false;
    private Vector3 movementVector = Vector3.zero;


    private CharacterController characterController;
    private Camera characterCamera;
    private CollisionFlags lastCollisionFlags;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        movement *= mouseSensitivity;

        Func<float, float> clampDegrees = degrees =>
        {
            if (degrees > 360)
                degrees = degrees - 360;
            if (degrees < 0)
                degrees = 360 + degrees;

            return degrees;
        };

        currentEulerAngle += movement;
        currentEulerAngle.x = clampDegrees(currentEulerAngle.x);
        currentEulerAngle.y = clampDegrees(currentEulerAngle.y);
        currentEulerAngle.y = Math.Min(Math.Max(currentEulerAngle.y, 10), 170);

        characterCamera.transform.localRotation = Quaternion.Euler(currentEulerAngle.y - 90, currentEulerAngle.x, 0);

        if (!jumpRequested)
            jumpRequested = Input.GetKeyDown(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2();
        {
            if (Input.GetKey(KeyCode.A))
                movement.x--;
            if (Input.GetKey(KeyCode.D))
                movement.x++;
            if (Input.GetKey(KeyCode.W))
                movement.y++;
            if (Input.GetKey(KeyCode.S))
                movement.y--;

            movement.Normalize();
        }

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = characterCamera.transform.forward * movement.y + characterCamera.transform.right * movement.x;

        movementVector.x = desiredMove.x * moveSpeed;
        movementVector.z = desiredMove.z * moveSpeed;

        if (characterController.isGrounded)
        {
            movementVector.y = -STICK_TO_GROUND_FORCE;

            if (jumpRequested)
            {
                movementVector.y = jumpSpeed;
                jumpRequested = false;
            }
        }
        else
        {
            movementVector += Physics.gravity * gravityStrength * Time.fixedDeltaTime;
        }

        lastCollisionFlags = characterController.Move(movementVector*Time.fixedDeltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // dont move the rigidbody if the character is on top of it
        if (lastCollisionFlags == CollisionFlags.Below)
            return;

        if (body == null || body.isKinematic)
            return;

        body.AddForceAtPosition(characterController.velocity, hit.point, ForceMode.Impulse);
    }
}
