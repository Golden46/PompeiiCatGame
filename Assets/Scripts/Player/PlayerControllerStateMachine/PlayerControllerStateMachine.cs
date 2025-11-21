using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerStateMachine : MonoBehaviour
{
    // Singleton
    public static PlayerControllerStateMachine Instance;

    // Move settings
    [Header("Movement")]
    private Vector2 _moveInput;
    private Vector3 _appliedMovement;
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
    private bool _isWalkPressed;
    private bool _isRunPressed;

    // Particles
    [SerializeField] private GameObject _echoSensePrefab;

    // Camera settings
    [Header("Camera Properties")]
    [SerializeField] private float _rotationSmoothTime = 0.1f;
    [SerializeField] private Transform _cameraTransform;
    private float _rotationVelocity;

    // Components
    private CharacterController _characterController;
    private CatAIStateMachine _catAIStateMachine;
    private QuestManager _questManager;
    private Quest _currentTargetableQuest;
    private GameObject[] _holoStructure;

    // State variables
    private PlayerControllerBaseState _currentState;
    private PlayerControllerStateFactory _states;

    // Quest interaction stuff
    private GetQuest interactObject = null;

    // Getters and Setters
    public PlayerControllerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public float CurrentSpeed { get { return _currentSpeed; } set { _currentSpeed = value; } }
    public float MoveSpeed { get { return _moveSpeed; } }
    public float SprintSpeed { get { return _sprintSpeed; } } 
    public float MoveInputX { get { return _moveInput.x; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float MoveInputY { get { return _moveInput.y; } }
    public float AppliedMovementZ { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsWalkPressed { get { return _isWalkPressed; } }
    public bool isRunPressed { get { return _isRunPressed; } }
    public Vector3 Velocity { get { return _velocity; } }
    public float VelocityY { get { return Velocity.y; } set { _velocity.y = value; } }
    public float JumpHeight { get { return _jumpHeight; } }
    public float Gravity { get { return _gravity; }  }

    [Header("For RB Movement (WIP)")]
    public float speed = 4.0f;
    public CinemachineFreeLook cmFreeLook;
    public Rigidbody rb;
    public float rotateSpeed = 10.0f;

    private void Awake()
    {
        // Create singleton
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        // Setup Components / Variables
        _characterController = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        _questManager = QuestManager.Instance;
        _currentSpeed = _moveSpeed;

        // Setup State
        _states = new PlayerControllerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();

        // Lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _currentState.UpdateState();
        HandleMovement();
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        //HandleMovement(); // For RB movement.
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _isWalkPressed = _moveInput.x != 0 || _moveInput.y != 0;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    public void OnEcho(InputAction.CallbackContext context)
    {
        if (_catAIStateMachine == null || _questManager.isActive || _currentTargetableQuest.isCompleted) return;

        Instantiate(_echoSensePrefab, transform.position, _echoSensePrefab.transform.rotation);
        AudioManager.PlaySound(SoundType.ECHOSENSE, 0.25f);
        _questManager.StartQuest(_currentTargetableQuest);
        interactObject.Rebuild();
    }

    public void OnMeow(InputAction.CallbackContext context)
    {
        Debug.Log("Meow");
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = new Vector3(_appliedMovement.x, 0f, _appliedMovement.y).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _characterController.Move(moveDir.normalized * _currentSpeed * Time.deltaTime);
        }
    }

    // Making a start to RB movement :/
    /*
    private void HandleMovement()
    { 
        Vector3 inputVector = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
        inputVector = transform.TransformDirection(inputVector);
        rb.linearVelocity = inputVector * speed; 
    }
    */
    
    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void GetQuestObjects(Collider other)
    {
        interactObject = other.GetComponent<GetQuest>();

        _currentTargetableQuest = interactObject.catQuest;

        _catAIStateMachine = interactObject.cat.GetComponent<CatAIStateMachine>();
        _catAIStateMachine.InQuestLocation = true;

        _holoStructure = interactObject.holoStructure;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestArea"))
        {
            GetQuestObjects(other);
        }

        if (other.CompareTag("QuestItem"))
        {
            other.GetComponent<ObjectInteract>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "QuestArea") _catAIStateMachine.InQuestLocation = false;

        _currentTargetableQuest = null;
        _catAIStateMachine = null;
        _holoStructure = null;
    }
}
