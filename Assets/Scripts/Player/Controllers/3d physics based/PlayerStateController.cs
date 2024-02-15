using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

// Enum for easing types.
public enum EasingType
{
    Linear,
    // Add more easing types here as needed.
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerStateController : MonoBehaviour
{
    [SerializeField] private PlayerMovementStatsSO playerMovementStatsSO;
    //[SerializeField] private WorldPositionTrackerSO worldPositionTrackerSO;
    [SerializeField] private FootstepManager footstepManager;
    //[SerializeField] private PowerupManager powerupManager;

    private float maxSpeed = 3f;
    private float acceleration = 5f;
    private float deceleration = 5f;
    private float jumpForce = 3f;
    private int jumpCount = 1;
    private float coyoteTime = 0.5f;
    private float maxFallingVelocity = 5f;

    private Vector2 movementVector = Vector2.zero;

    private BaseState currentState;
    private Rigidbody rb; // Reference to the character's Rigidbody component
    private Camera cam;
    [SerializeField] private CinemachineInputProvider cinemachineInputProvider;
    private float coyoteTimeTimer;
    public bool isGrounded = true;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private LayerMask groundMask;
    Coroutine currentMovementCoroutine;
    Coroutine currentLerpCoroutine;

    public IdleState IdleState;
    public RunningState RunningState;
    public JumpingState JumpingState;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        IdleState = new IdleState();
        RunningState = new RunningState();
        JumpingState = new JumpingState();

        if (cinemachineInputProvider == null)
        {
            //yeah i know this line is horrible, I just need it there as a failsafe
            Debug.LogError("Please assign CinemachineInputProvider for PlayerStateController in the inspector");
            //cinemachineInputProvider = cam.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineInputProvider>();
        }

        TransitionToState(IdleState); // Start in the "Idle" state
        LockCursor();
    }

    private void Start()
    {
        IdleState.SetController(this);
        RunningState.SetController(this);
        JumpingState.SetController(this);

        rb.useGravity = true;
    }

    public void InitializeStats()
    {
        this.maxSpeed = playerMovementStatsSO.maxSpeed;
        this.acceleration = playerMovementStatsSO.acceleration;
        this.deceleration = playerMovementStatsSO.deceleration;
        this.jumpForce = playerMovementStatsSO.jumpForce;
        this.jumpCount = playerMovementStatsSO.jumpCount;
        this.maxFallingVelocity = playerMovementStatsSO.maxFallingVelocity;
    }

    private void OnEnable()
    {
        PlaytimeInputManager.inputActions.Player.Move.performed += UpdateMovementVector;
        PlaytimeInputManager.inputActions.Player.Jump.performed += InvokeJump;

        //GameController.GameInitialize += FullInitialize;
        //GameController.GameStart += StartGame;
        //GameController.GameStop += EndGame;

        InitializeStats();
    }

    private void OnDisable()
    {
        PlaytimeInputManager.inputActions.Player.Move.performed -= UpdateMovementVector;
        PlaytimeInputManager.inputActions.Player.Jump.performed -= InvokeJump;

        //GameController.GameInitialize -= FullInitialize;
        //GameController.GameStart -= StartGame;
        //GameController.GameStop -= EndGame;
    }

    private void FullInitialize()
    {
        InitializeStats();
        UnlockCursor();
        DisableCameraInput();
    }

    private void UpdateMovementVector(InputAction.CallbackContext value)
    {
        movementVector = value.ReadValue<Vector2>();
    }

    private void InvokeJump(InputAction.CallbackContext value)
    {
        Jump();
    }

    public void Jump()
    {
        if (jumpCount <= 0 && coyoteTimeTimer <= 0)
        {
            return;
        }
        if (!CheckIfGrounded()) { return; }
        jumpCount--;
        TransitionToState(JumpingState);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //AddForceInDirection(Vector3.forward, 30, 1);
    }

    public void ResetJumps()
    {
        jumpCount = playerMovementStatsSO.jumpCount;
    }

    public void AddJumps(int amount)
    {
        jumpCount += amount;
    }

    public void AddForceInDirection(Vector3 direction, float forceAmount, float duration, float controlAmount = 1, bool cancelVelocity = false, bool stopOnCollision = false)
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }
        currentMovementCoroutine = StartCoroutine(MovePlayer(direction, forceAmount, duration, controlAmount, controlAmount, cancelVelocity, stopOnCollision));
    }

    public void AddForceInDirection(Vector3 direction, float forceAmount, float duration, float accelerationAmount, float decelerationAmount, bool cancelVelocity = false, bool stopOnCollision = false)
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }
        currentMovementCoroutine = StartCoroutine(MovePlayer(direction, forceAmount, duration, accelerationAmount, decelerationAmount, cancelVelocity, stopOnCollision));
    }

    private IEnumerator MovePlayer(Vector3 direction, float forceAmount, float duration, float accelerationAmount, float decelerationAmount, bool cancelVelocity, bool stopOnCollision)
    {

        float originalDeceleration = deceleration;
        float originalAcceleration = acceleration;
        deceleration = decelerationAmount;
        acceleration = accelerationAmount;
        Vector3 worldDirection = transform.TransformDirection(direction);
        if (cancelVelocity) { rb.velocity = Vector3.zero; }
        rb.AddForce(worldDirection.normalized * forceAmount, ForceMode.Impulse);
        yield return new WaitForSeconds(duration);
        deceleration = originalDeceleration;
        acceleration = originalAcceleration;
    }

    public void LerpPositionTo(Transform transformToMove, Transform targetTransform, float duration, EasingType easeType = EasingType.Linear)
    {
        if (currentLerpCoroutine != null)
        {
            StopCoroutine(currentLerpCoroutine);
        }
        currentLerpCoroutine = StartCoroutine(LerpToTransformCoroutine(transformToMove, targetTransform, duration, easeType));
    }

    public void LerpPositionTo(Transform targetTransform, float duration, EasingType easeType = EasingType.Linear)
    {
        if (currentLerpCoroutine != null)
        {
            StopCoroutine(currentLerpCoroutine);
        }
        currentLerpCoroutine = StartCoroutine(LerpToTransformCoroutine(this.transform, targetTransform, duration, easeType));
    }

    private IEnumerator LerpToTransformCoroutine(Transform transformToMove, Transform targetTransform, float duration, EasingType easeType)
    {
        float startTime = Time.time;
        Vector3 initialPosition = transformToMove.position;
        //Quaternion initialRotation = transformToMove.rotation;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            t = EasingFunction(easeType, t);

            transformToMove.position = Vector3.Lerp(initialPosition, targetTransform.position, t);
            //transformToMove.rotation = Quaternion.Slerp(initialRotation, targetTransform.rotation, t);

            yield return null;
        }

        // Ensure it reaches the exact target position and rotation.
        transformToMove.position = targetTransform.position;
        //transformToMove.rotation = targetTransform.rotation;
    }

    // Easing function for interpolation.
    private float EasingFunction(EasingType type, float t)
    {
        switch (type)
        {
            case EasingType.Linear:
                return t;
            // You can add more easing functions here.
            // For example: ease in/out, ease out, etc.
            default:
                return t;
        }
    }

    public void StartGame()
    {
        LockCursor();
        EnableCameraInput();
        rb.isKinematic = false;
    }
    public void EndGame()
    {
        UnlockCursor();
        DisableCameraInput();
        rb.isKinematic = true;
    }
    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableCameraInput()
    {
        cinemachineInputProvider.enabled = false;
    }

    public void EnableCameraInput()
    {
        cinemachineInputProvider.enabled = true;
    }

    public void UpdateMovement()
    {
        // Calculate the movement direction based on player input
        Vector3 inputDirection = new Vector3(movementVector.x, 0, movementVector.y);
        Vector3 worldSpaceMovement = this.transform.TransformDirection(inputDirection).normalized;

        // Calculate the desired velocity
        Vector3 desiredVelocity = worldSpaceMovement * maxSpeed;

        // Calculate the change in velocity based on acceleration
        Vector3 velocityChange = (desiredVelocity - rb.velocity) * acceleration * Time.deltaTime;

        // Apply the change in velocity using AddForce
        rb.AddForce(new Vector3(velocityChange.x, 0, velocityChange.z), ForceMode.VelocityChange);

        // Limit the speed to the maximum speed without affecting Y velocity
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }

        // Check if there is no input and apply deceleration
        if (movementVector.magnitude == 0)
        {
            Vector3 decelerationAmount = -rb.velocity * deceleration * Time.deltaTime;

            // Clamp the velocity if it's close to zero
            if (rb.velocity.magnitude <= 0.1f)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
                rb.velocity += new Vector3(decelerationAmount.x, 0, decelerationAmount.z);
            }
        }

        if (isGrounded)
        {
            coyoteTimeTimer = coyoteTime;
            footstepManager?.PlayFootstepSound();
        }
        else
        {
            coyoteTimeTimer -= Time.deltaTime;
        }

        if (GetVerticalVelocity() < -maxFallingVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxFallingVelocity, rb.velocity.z);
        }

        //worldPositionTrackerSO.value = this.transform.position;
    }

    public bool CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundPosition.position, 0.3f, groundMask);
        return isGrounded;
    }

/*    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundPosition.position, 0.3f);
    }*/

    // Update is called once per frame
    private void Update()
    {
        currentState.UpdateState();
        transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    // Method for transitioning to a new state
    public void TransitionToState(BaseState newState)
    {
        //Debug.Log($"Transition to {newState}");
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState();
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    public Vector2 GetInputVector()
    {
        return movementVector;
    }

    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }

    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }

    public Camera GetCamera()
    {
        return cam;
    }

}
