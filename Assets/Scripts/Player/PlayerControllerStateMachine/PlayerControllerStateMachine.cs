using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerControllerStateMachine : MonoBehaviour
{
    // Singleton
    public static PlayerControllerStateMachine Instance;

    // Move settings
    [Header("Movement")]
    private Vector2 _moveInput;
    private Vector3 _velocity;
    [SerializeField] private float _gravity = -9.81f;

    // Jump Settings
    [Header("Jump")]
    [SerializeField] float _jumpHeight = 2f;

    // Speed settings
    [Header("Speed")]
    private float _currentSpeed;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _sprintSpeed = 7.5f;

    // Button pressed
    private bool _isJumpPressed;

    // Camera settings
    [Header("Camera")]
    [SerializeField] private float _rotationSmoothTime = 0.1f;
    [SerializeField] private Transform _cameraTransform;
    private float _rotationVelocity;

    // Components
    private CharacterController _characterController;

    // State variables
    private PlayerControllerBaseState _currentState;
    private PlayerControllerStateFactory _states;

    // Getters and Setters
    public PlayerControllerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController {  get { return _characterController; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public float VelocityY { get { return _velocity.y; } set { _velocity.y = value; } }
    public float JumpHeight { get { return _jumpHeight; } }
    public float Gravity { get { return _gravity; }  }

    private void Awake()
    {
        // Create singleton
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        // Setup Components / Variables
        _characterController = GetComponent<CharacterController>();
        _currentSpeed = _moveSpeed;

        // Setup State
        _states = new PlayerControllerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.UpdateState();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _currentSpeed = context.performed ? _sprintSpeed : _moveSpeed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    private void LateUpdate()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _characterController.Move(moveDir.normalized * _currentSpeed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
