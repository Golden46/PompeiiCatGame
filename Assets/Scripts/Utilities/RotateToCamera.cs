using System;
using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main; 
    }

    private void Update()
    {
        if (_camera)
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up);
    }
}
