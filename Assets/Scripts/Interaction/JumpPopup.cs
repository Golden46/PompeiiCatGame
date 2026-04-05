using UnityEngine;
using UnityEngine.UI;

public class JumpPopup : MonoBehaviour
{
    [Header("Jump Interaction")]
    [SerializeField] private Image JumpTargetPopup;
    [SerializeField] private Transform _targetPoint;
    private bool _shouldJump;

    public Transform TargetPoint => _targetPoint;
    public bool ShouldJump => _shouldJump;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var direction = _targetPoint.position - other.transform.position;
        var dot = Vector3.Dot(Camera.main.transform.forward, direction.normalized);

        if (dot >= 0.5f)
        {
            JumpTargetPopup.gameObject.SetActive(true);
            _shouldJump = true;
        }
        else
        {
            JumpTargetPopup.gameObject.SetActive(false);
            _shouldJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        JumpTargetPopup.gameObject.SetActive(false);
        _shouldJump = false;
    }
}