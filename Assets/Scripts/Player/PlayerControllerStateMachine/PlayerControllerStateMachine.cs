using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerStateMachine : MonoBehaviour
{
    // Singleton
    public static PlayerControllerStateMachine Instance;

    // Move settings
    [Header("Movement")]
    private Vector2 _moveInput;
    private Vector3 _velocity;

    // Jump Settings
    [Header("Jump")]
    [SerializeField] private AnimationCurve _jumpArcSpeed;
    private Transform _targetLedge;

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
    private CatAIStateMachine _catAIStateMachine;
    private QuestManager _questManager;
    private Quest _currentTargetableQuest;
    private Animator _playerAnimator;
    private GameObject[] _holoStructure;

    // State variables
    private PlayerControllerBaseState _currentState;
    private PlayerControllerStateFactory _states;

    // Quest interaction stuff
    private GetQuest interactObject = null;

    // Getters and Setters
    public PlayerControllerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Rigidbody RB { get { return rb; } set { rb = value; } }
    public Animator PlayerAnimator { get { return _playerAnimator; } set { _playerAnimator = value;  } }
    public float CurrentSpeed { get { return _currentSpeed; } set { _currentSpeed = value; } }
    public Transform TargetLedge { get { return _targetLedge; } set { _targetLedge = value; } }
    public AnimationCurve JumpArcSpeed => _jumpArcSpeed;
    public float MoveSpeed => _moveSpeed;
    public float SprintSpeed => _sprintSpeed;
    public bool IsJumpPressed => _isJumpPressed;
    public bool IsWalkPressed => _isWalkPressed;
    public bool IsRunPressed => _isRunPressed;

    [Header("For RB Movement (WIP)")]
    public float speed = 60.0f;
    private Rigidbody rb;
    private float targetAngle;

    private void Awake()
    {
        // Create singleton
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        // Setup State
        _states = new PlayerControllerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();

        // Lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        // Setup Components / Variables
        rb = GetComponent<Rigidbody>();
        _questManager = QuestManager.Instance;
        _playerAnimator = GetComponent<Animator>();

        _currentSpeed = _moveSpeed;
    }

    private void Update()
    {
        _currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();
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
        Debug.Log("Echo");
        if (_currentTargetableQuest.isCompleted)
        {
            _questManager.FinishQuest();
            return;
        }

        if (_catAIStateMachine == null || _questManager.isActive) return;

        Instantiate(_echoSensePrefab, transform.position, _echoSensePrefab.transform.rotation);
        AudioManager.PlaySound(SoundType.ECHOSENSE, 0.25f);
        _questManager.StartQuest(_currentTargetableQuest);
        interactObject.Rebuild();
    }

    public void OnMeow(InputAction.CallbackContext context)
    {
        Debug.Log("Meow");
    }
 
    public void GetOrientation()
    {
        targetAngle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void HandleMovement()
    {
        if (_moveInput == Vector2.zero) return;
        Vector3 moveVelocity = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }
 
    private void GetQuestObjects(Collider other)
    {
        interactObject = other.GetComponent<GetQuest>();

        _currentTargetableQuest = interactObject.catQuest;

        _catAIStateMachine = interactObject.cat.GetComponent<CatAIStateMachine>();
        _catAIStateMachine.InQuestLocation = true;

        _holoStructure = interactObject.holoStructure;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<JumpPopup>(out JumpPopup jumpPopup))
        {
            _targetLedge = jumpPopup.ShouldJump ? jumpPopup.TargetPoint : null;
        }
        else
        {
            _targetLedge = null;
        }
    }

    // Change below into its own script...
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestArea"))
        {
            GetQuestObjects(other);
        }

        if (other.CompareTag("QuestItem"))
        {
            Debug.Log(other.GetComponent<ObjectInteract>().name);
            other.GetComponent<ObjectInteract>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("QuestArea")) _catAIStateMachine.InQuestLocation = false;

        _currentTargetableQuest = null;
        _catAIStateMachine = null;
        _holoStructure = null;
    }
}