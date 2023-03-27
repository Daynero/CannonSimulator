using System;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    [SerializeField] private Transform barrel;
    [SerializeField] private BulletGenerator bulletGenerator;
    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float maxBarrelAngle = 40f;
    [SerializeField] private float maxBaseAngle = 40f;
    [SerializeField] private float minBaseAngle = -40f;

    [SerializeField] private Slider bulletSpeedSlider; // Remove it

    private const float BulletSpeedMultiplier = 20f;

    private float _baseAngle;
    private float _barrelAngleY;
    private Quaternion _barrelInitialRotation;
    private float _bulletSpeed;
    private LineRenderer _trajectoryLineRenderer;

    private float _lastBaseAngle;
    private float _lastBarrelAngleY;

    private void Awake()
    {
        bulletSpeedSlider.onValueChanged.AddListener(SetBulletSpeed);
        _trajectoryLineRenderer = spawnPointTransform.gameObject.GetComponent<LineRenderer>();
        if (_trajectoryLineRenderer == null)
        {
            _trajectoryLineRenderer = spawnPointTransform.gameObject.AddComponent<LineRenderer>();
            _trajectoryLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _trajectoryLineRenderer.startColor = Color.yellow;
            _trajectoryLineRenderer.endColor = Color.red;
            _trajectoryLineRenderer.startWidth = 0.1f;
            _trajectoryLineRenderer.endWidth = 0.05f;
        }
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
        CheckAndSetTrajectoryLine();
    }

    private void UpdateBarrelRotation()
    {
        float barrelRotationDelta = -Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        _barrelAngleY = Mathf.Clamp(_barrelAngleY + barrelRotationDelta, -maxBarrelAngle, 0f);
        barrel.localRotation = _barrelInitialRotation * Quaternion.Euler(_barrelAngleY, 0f, 0f);
        CheckAndSetTrajectoryLine();
        _lastBarrelAngleY = _barrelAngleY;
    }

    private void UpdateBaseRotation()
    {
        float baseRotationDelta = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        _baseAngle = Mathf.Clamp(_baseAngle + baseRotationDelta, minBaseAngle, maxBaseAngle);
        transform.localRotation = Quaternion.Euler(0f, _baseAngle, 0f);
        CheckAndSetTrajectoryLine();
        _lastBaseAngle = _baseAngle;
    }

    private void CheckAndSetTrajectoryLine()
    {
        if (Math.Abs(_lastBaseAngle - _baseAngle) > 0.01f || Math.Abs(_lastBarrelAngleY - _barrelAngleY) > 0.01f)
        {
            SetTrajectoryLine();
        }
    }


    private void Fire()
    {
        bulletGenerator.GenerateBullet(barrel.forward, _bulletSpeed);
    }


    private void SetTrajectoryLine()
    {
        Vector3 initialVelocity = barrel.forward * _bulletSpeed;
        int numberOfPoints = 50;
        Vector3[] points = new Vector3[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float) numberOfPoints;
            points[i] = spawnPointTransform.position + initialVelocity * t + Physics.gravity * t * t * 0.5f;
        }

        _trajectoryLineRenderer.positionCount = numberOfPoints;
        _trajectoryLineRenderer.SetPositions(points);
    }

    private void SetBulletSpeed(float speed)
    {
        _bulletSpeed = speed * BulletSpeedMultiplier;
        SetTrajectoryLine();
    }
}