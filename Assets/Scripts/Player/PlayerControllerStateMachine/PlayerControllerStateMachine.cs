using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControllerStateMachine : MonoBehaviour
{
    // Singleton
    public static PlayerControllerStateMachine Instance;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.0f;
    public bool IsWalkPressed { get; private set; }
    private float _targetAngle;
    private Vector2 _moveInput;
    private Vector3 _velocity;
    
    [Header("Jump")]
    public Transform TargetLedge { get; set; }
    [FormerlySerializedAs("_jumpArcSpeed")] [SerializeField] private AnimationCurve jumpArcSpeed;
    public AnimationCurve JumpArcSpeed => jumpArcSpeed;
    public bool IsJumpPressed { get; private set; }
    private bool _shouldJump;

    // Particles
    [SerializeField] private GameObject _echoSensePrefab;
    
    [Header("Camera Properties")]
    [SerializeField] private float _rotationSmoothTime = 0.1f;
    [SerializeField] private Transform _cameraTransform;
    private float _rotationVelocity;

    [Header("Components")]
    public Rigidbody Rb { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    private CatAIStateMachine _catAIStateMachine;

    // State variables
    private PlayerControllerStateFactory _states;
    public PlayerControllerBaseState CurrentState { get; set; }

    // Interaction stuff
    private InitiateQuest _questData;
    private ObjectInteract _interactData;

    private void Awake()
    {
        // Create singleton
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        // Setup State
        _states = new PlayerControllerStateFactory(this);
        CurrentState = _states.Idle();
        CurrentState.EnterState();

        // Lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        // Setup Components / Variables
        Rb = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        CurrentState.UpdateState();
    }

    private void FixedUpdate()
    {
        CurrentState.FixedUpdateState();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        IsWalkPressed = _moveInput.x != 0 || _moveInput.y != 0;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        IsJumpPressed = context.ReadValueAsButton();
    }
    
    public void OnEcho(InputAction.CallbackContext context)
    {
        if (_questData == null) return;
        _questData.QuestGate();
    }
    
    public void OnPickup(InputAction.CallbackContext context)
    {
        if (_interactData == null) return;
        _interactData.Pickup();
    }

    public void GetOrientation()
    {
        _targetAngle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _rotationVelocity, _rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void HandleMovement()
    {
        if (_moveInput == Vector2.zero) return;
        var moveVelocity = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward * moveSpeed;
        Rb.linearVelocity = new Vector3(moveVelocity.x, Rb.linearVelocity.y, moveVelocity.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InitiateQuest>(out var initiateQuest)) _questData = initiateQuest;
        else if (other.TryGetComponent<ObjectInteract>(out var objectInteract)) _interactData = objectInteract;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<JumpPopup>(out var jumpPopup)) return;
        _shouldJump = jumpPopup.ShouldJump;
        TargetLedge = _shouldJump ? jumpPopup.TargetPoint : null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InitiateQuest>(out var initiateQuest)) _questData = null;
        else if (other.TryGetComponent<ObjectInteract>(out var objectInteract)) _interactData = null;
    }
}