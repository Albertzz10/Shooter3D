using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    private CharacterController characterController;
    private Transform cameraTransform;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private float gravity = Physics.gravity.y;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector2 velocity;
    private float verticalVelocity;
    private float verticalRotation = 0;

    private bool isSprinting;

    public float lookSensitivity = 1f;
    private float maxLookAngle = 80f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;


    }

    private void Update()
    {
        MovePlayer();

        LookAround();
    }


    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }


    /// <summary>
    /// receive Sprint input and run
    /// </summary>
    /// <param name="context"></param>
    public void sprint(InputAction.CallbackContext context)
    {



    }

    public void MovePlayer()
    {

        //falling
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            //when is falling increase the speed of fall
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(0, verticalVelocity, 0);
        characterController.Move(move * Time.deltaTime);

        //movement
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        float targetSpeed = isSprinting ? speed *multiplier : speed;
        characterController.Move(moveDirection * targetSpeed * Time.deltaTime);

        //apply gravity constantly
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }

    private void LookAround()
    {
        float horizontalRotation = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * horizontalRotation);

        verticalRotation -= lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = quaternion.Euler(verticalRotation, 0f, 0f);
    }

}
