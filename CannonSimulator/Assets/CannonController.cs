using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    [SerializeField] private Transform barrel;
    [SerializeField] private BulletGenerator bulletGenerator;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float maxBarrelAngle = 40f;
    [SerializeField] private float maxBaseAngle = 40f;
    [SerializeField] private float minBaseAngle = -40f;

    [SerializeField] private Slider bulletSpeedSlider;  // Remove it

    private const float BulletSpeedMultiplier = 20f;
    
    private float _baseAngle;
    private float _barrelAngleY;
    private Quaternion _barrelInitialRotation;
    private float _bulletSpeed;

    private void Awake()
    {
        bulletSpeedSlider.onValueChanged.AddListener(SetBulletSpeed);
    }

    private void Start()
    {
        _barrelInitialRotation = barrel.localRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        
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
    
    private void Fire()
    {
        bulletGenerator.GenerateBullet(barrel.forward, _bulletSpeed);
    }

    private void SetBulletSpeed(float speed)
    {
        _bulletSpeed = speed * BulletSpeedMultiplier;
    }
}
