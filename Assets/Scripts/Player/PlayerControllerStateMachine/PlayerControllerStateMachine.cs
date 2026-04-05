using System;
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
    public bool ShouldJump { get; private set; }

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

    // Quest interaction stuff
    private bool _canEcho;
    private InitiateQuest _questData;

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
        
        /*
        Debug.Log("Echo");
        if (CurrentTargetableQuest.isCompleted)
        {
            _questManager.FinishQuest();
            return;
        }

        if (_catAIStateMachine == null || _questManager.isActive) return;

        Instantiate(_echoSensePrefab, transform.position, _echoSensePrefab.transform.rotation);
        AudioManager.PlaySound(SoundType.ECHOSENSE, 0.25f);
        _questManager.StartQuest(CurrentTargetableQuest); // DONE
        _interactObject.HoloRestoration(); // DONE 
        */
    }

    public void OnMeow(InputAction.CallbackContext context)
    {
        Debug.Log("Meow");
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
        if (!other.TryGetComponent<InitiateQuest>(out var initiateQuest)) return;
        _canEcho = true;
        _questData = initiateQuest;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<JumpPopup>(out var jumpPopup)) return;
        ShouldJump = jumpPopup.ShouldJump;
        TargetLedge = ShouldJump ? jumpPopup.TargetPoint : null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InitiateQuest>(out var initiateQuest)) _questData = null;
    }
    
    /*
    // Change below into its own script...
    private void GetQuestObjects(Collider other)
    {
        _catAIStateMachine = _interactObject.cat.GetComponent<CatAIStateMachine>();
        _catAIStateMachine.InQuestLocation = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QuestItem"))
        {
            Debug.Log(other.GetComponent<ObjectInteract>().name);
            other.GetComponent<ObjectInteract>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("QuestArea")) _catAIStateMachine.InQuestLocation = false;

        CurrentTargetableQuest = null;
        _catAIStateMachine = null;
        _holoStructure = null;
    }
    */
}