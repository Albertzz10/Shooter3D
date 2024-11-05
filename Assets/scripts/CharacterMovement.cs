using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    private float maxStamina = 1f;
    private float staminaValue = 1f;
    private bool staminaCharged = true;

    //[SerializeField] private float maxStamina;
      [SerializeField] private Slider staminaSlider;
    //[SerializeField] private float staminaDrainRate = 10f;
    //[SerializeField] private float staminaRegenerate = 5;
    //private float stamina;


    private WeaponController weaponController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        weaponController = GetComponent<WeaponController>();

        //stamina = maxStamina;
    }

    private void Update()
    {
        MovePlayer();

        LookAround();
    }


    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// receive Sprint input and run
    /// </summary>
    /// <param name="context"></param>
    public void Sprint(InputAction.CallbackContext context)
    {
        var SprintAction = context.ReadValue<float>();
        isSprinting = SprintAction > 0 ? true : false;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (weaponController.CanShoot()) weaponController.Shoot();
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
        float targetSpeed = isSprinting && staminaCharged ? speed *multiplier : speed;
        characterController.Move(moveDirection * targetSpeed * Time.deltaTime);

        //apply gravity constantly
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }

    /// <summary>
    /// Handles camera rotation based on Look Input
    /// </summary>
    private void LookAround()
    {
        // Horizontal rotation (Y-axis) based on sensitivity on input
        float horizontalRotation = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * horizontalRotation);

        // Vertical rotation (X-axis) with clamping to prevent over-rotation
        verticalRotation -= lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    //private void HandleStamina()
    //{
    //    if (isSprinting && stamina > 0)
    //    {
    //        stamina -= staminaDrainRate * Time.deltaTime;
    //        if totally used
    //        if (stamina <= 0)
    //        {
    //            stamina = 0;
    //            isSprinting = false;
    //        }
    //        regenerate stamina
    //        else if (!isSprinting && stamina < maxStamina)
    //        {
    //            stamina += staminaRegenerate * Time.deltaTime;
    //        }
    //    }
    //}


    private void Stamina()
    {

        if (isSprinting  && staminaValue >= 0)
        {
            staminaValue -= Time.deltaTime;

            if (staminaValue <= 0)
            {
                staminaValue = 0;
                isSprinting = false;
            }
        }
        else if (!isSprinting && staminaValue <= maxStamina)
        {
            staminaValue += Time.deltaTime;
        }
                                                                                                                                  
        staminaSlider.value = staminaValue;
    }
}
