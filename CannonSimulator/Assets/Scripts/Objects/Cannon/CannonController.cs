using System;
using Objects.Cannon;
using Screens.MainScreen;
using UnityEngine;

public class CannonController
{
    private ICannonView _cannonView;
    private IBulletGenerator _bulletGenerator;
    private readonly MainScreenPresenter _mainScreenPresenter;

    private const float MaxBarrelAngle = 40f;
    private const float MaxBaseAngle = 40f;
    private const float MinBaseAngle = -40f;
    private const float BulletSpeedMultiplier = 20f;
    private const float InitialBulletSpeed = 2;

    private float _baseAngle;
    private float _barrelAngleY;
    private Quaternion _barrelInitialRotation;
    private float _bulletSpeed;

    private float _lastBaseAngle;
    private float _lastBarrelAngleY;

    public CannonController(ICannonView cannonView,
        MainScreenPresenter mainScreenPresenter)
    {
        _cannonView = cannonView;
        _mainScreenPresenter = mainScreenPresenter; 

        Initialize();
    }

    private void Initialize()
    {
        _bulletGenerator = _cannonView.BulletGenerator;
        _barrelInitialRotation = _cannonView.BarrelTransform.localRotation;
        _cannonView.OnFireClicked += Fire;
        _cannonView.OnBarrelRotationChanged += UpdateBarrelRotation;
        _cannonView.OnBaseRotationChanged += UpdateBaseRotation;
        _mainScreenPresenter.OnBulletSpeedChanged += SetBulletSpeed;

        _bulletSpeed = InitialBulletSpeed;
        SetTrajectoryLine();
    }

    private void UpdateBarrelRotation(float barrelRotationDelta)
    {
        _barrelAngleY = Mathf.Clamp(_barrelAngleY + barrelRotationDelta, -MaxBarrelAngle, 0f);
        Quaternion barrelAngle = _barrelInitialRotation * Quaternion.Euler(_barrelAngleY, 0f, 0f);
        _cannonView.SetBarrelRotation(barrelAngle);
        CheckAndSetTrajectoryLine();
        _lastBarrelAngleY = _barrelAngleY;
    }

    private void UpdateBaseRotation(float baseRotationDelta)
    {
        _baseAngle = Mathf.Clamp(_baseAngle + baseRotationDelta, MinBaseAngle, MaxBaseAngle);
        _cannonView.SetBaseRotation(Quaternion.Euler(0f, _baseAngle, 0f));
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
        _cannonView.CameraShake.ShakeCamera();
        _cannonView.GunRecoil.Fire();
        _bulletGenerator.GenerateBullet(_cannonView.BarrelTransform.forward, _bulletSpeed);
    }

    private void SetTrajectoryLine()
    {
        Vector3 initialVelocity = _cannonView.BarrelTransform.forward * _bulletSpeed;
        int numberOfPoints = 50;
        Vector3[] points = new Vector3[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float) numberOfPoints;
            points[i] = _cannonView.SpawnPointTransform.position + initialVelocity * t + Physics.gravity * t * t * 0.5f;
        }

        _cannonView.SetTrajectoryLine(points);
    }

    private void SetBulletSpeed(float speed)
    {
        _bulletSpeed = speed * BulletSpeedMultiplier + InitialBulletSpeed;
        SetTrajectoryLine();
    }
}