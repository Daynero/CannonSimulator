using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private Transform barrel;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float maxBarrelAngle = 40f;
    [SerializeField] private float maxBaseAngle = 40f;
    [SerializeField] private float minBaseAngle = -40f;

    private float _baseAngle;
    private float _barrelAngleY;
    private Quaternion _barrelInitialRotation;

    private void Start()
    {
        _barrelInitialRotation = barrel.localRotation;
    }

    private void Update()
    {
        UpdateBarrelRotation();
        UpdateBaseRotation();
    }

    private void UpdateBarrelRotation()
    {
        float barrelRotationDelta = -Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        _barrelAngleY = Mathf.Clamp(_barrelAngleY + barrelRotationDelta, -maxBarrelAngle, 0f);
        barrel.localRotation = _barrelInitialRotation * Quaternion.Euler(_barrelAngleY, 0f, 0f);
    }

    private void UpdateBaseRotation()
    {
        float baseRotationDelta = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        _baseAngle = Mathf.Clamp(_baseAngle + baseRotationDelta, minBaseAngle, maxBaseAngle);
        transform.localRotation = Quaternion.Euler(0f, _baseAngle, 0f);
    }
}
