using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CatCameraController : MonoBehaviour
{
    public static CatCameraController Instance;

    [SerializeField] private CinemachineFreeLook FreeLookCam;

    [Header("Zoom Settings")]
    [SerializeField] private float MinZoom = 1f;
    [SerializeField] private float MaxZoom = 10f;
    private float _zoomInput;

    [Header("Smoothing")]
    [SerializeField] private float ZoomSmoothTime = 0.5f;
    private float _targetZoom = 6f;
    private float _currentZoom = 6f;
    private float _zoomVelocity = 0f; 

    [Header("Orbit Settings")]
    [SerializeField] private float OrbitSpeed = 100f;
    private bool _isRotating = false;
    private Vector2 _lastMousePosition;
    private Vector2 _currentMousePosition;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Update()
    {
        HandleZoom();
        HandleOrbit();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        _isRotating = context.ReadValueAsButton();
        _lastMousePosition = Mouse.current.position.ReadValue();
    }

    public void OnRightClickCanceled(InputAction.CallbackContext context)
    {
        _isRotating = false;
    }    

    public void OnZoom(InputAction.CallbackContext context)
    {
        _zoomInput = context.ReadValue<float>();
    }
 
    private void HandleZoom()
    {
        if (Mathf.Abs(_zoomInput) > 0.01f)
        {
            _targetZoom = Mathf.Clamp(_targetZoom - _zoomInput, MinZoom, MaxZoom); // clamp to stop going too close
            _zoomInput = 0f;
        }

        _currentZoom = Mathf.SmoothDamp(_currentZoom, _targetZoom, ref _zoomVelocity, ZoomSmoothTime);

        // Orbit settings that preserve a good camera angle
        float centerHeight = Mathf.Clamp(_currentZoom, MinZoom, MaxZoom);
        float topHeight = centerHeight + 2f;
        float bottomHeight = centerHeight - 2f;

        float topRadius = centerHeight + 2f;
        float centerRadius = centerHeight;
        float bottomRadius = centerHeight + 1f;

        // Apply to all 3 orbits
        FreeLookCam.m_Orbits[0].m_Height = topHeight;
        FreeLookCam.m_Orbits[0].m_Radius = topRadius;

        FreeLookCam.m_Orbits[1].m_Height = centerHeight;
        FreeLookCam.m_Orbits[1].m_Radius = centerRadius;

        FreeLookCam.m_Orbits[2].m_Height = bottomHeight;
        FreeLookCam.m_Orbits[2].m_Radius = bottomRadius;
    }

    private void HandleOrbit()
    {
        if (_isRotating)
        {
            _currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 dragDelta = _currentMousePosition - _lastMousePosition;

            FreeLookCam.m_XAxis.Value += dragDelta.x * OrbitSpeed * Time.deltaTime;
            FreeLookCam.m_YAxis.Value += -dragDelta.y * OrbitSpeed * 0.01f * Time.deltaTime;

            _lastMousePosition = _currentMousePosition;
        }
    }
}