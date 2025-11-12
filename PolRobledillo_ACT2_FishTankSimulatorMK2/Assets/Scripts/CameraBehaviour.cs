using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CameraBehaviour : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public float distance = 100.0f;

    [Header("Rotation Settings")]
    public float xSensitivity = 2.0f;
    public float ySensitivity = 2.0f;
    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    [Header("Movement Settings")]
    public float smoothTime = 0.2f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (target == null)
        {
            enabled = false;
            return;
        }
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }
    private void LateUpdate()
    {
        if (target == null) return;

        MouseInput();
        UpdateCameraPosition();
    }

    private void MouseInput()
    {
        if (Mouse.current.leftButton.isPressed) // Right mouse button
        {
            Vector2 mouse = Mouse.current.delta.ReadValue();
            currentX += mouse.x * xSensitivity;
            currentY += mouse.y * ySensitivity;
            currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.gameObject.tag == "Surface")
            {
                ObjectsManager.instance.InstantiateFood(hit.point);
            }
        }
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position - rotation * Vector3.back * distance;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}
