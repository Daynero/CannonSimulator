using System;
using UnityEngine;
using XmannaSDK.Core;

namespace Objects.Cannon
{
    public class CannonView : ScreenView, ICannonView
    {
        [SerializeField] private Transform barrel;
        [SerializeField] private Transform spawnPointTransform;
        [SerializeField] private BulletGenerator bulletGenerator;
        [SerializeField] private CameraShake сameraShake;
        [SerializeField] private GunRecoil gunRecoil;
        
        [SerializeField] private float rotationSpeed = 30f;
        
        private LineRenderer _trajectoryLineRenderer;

        public Transform BarrelTransform => barrel;
        public Transform SpawnPointTransform => spawnPointTransform;
        public IBulletGenerator BulletGenerator => bulletGenerator;
        public CameraShake CameraShake => сameraShake;
        public GunRecoil GunRecoil => gunRecoil;

        public event Action OnFireClicked;
        public event Action<float> OnBaseRotationChanged;
        public event Action<float> OnBarrelRotationChanged;

        private void Awake()
        {
            _trajectoryLineRenderer = spawnPointTransform.gameObject.GetComponent<LineRenderer>();
            
            if (_trajectoryLineRenderer != null) return;
            _trajectoryLineRenderer = spawnPointTransform.gameObject.AddComponent<LineRenderer>();
            _trajectoryLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _trajectoryLineRenderer.startColor = Color.yellow;
            _trajectoryLineRenderer.endColor = Color.red;
            _trajectoryLineRenderer.startWidth = 0.1f;
            _trajectoryLineRenderer.endWidth = 0.05f;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnFireClicked?.Invoke();
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                float baseRotationDelta = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
                OnBaseRotationChanged?.Invoke(baseRotationDelta);
            }
            
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                float barrelRotationDelta = -Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
                OnBarrelRotationChanged?.Invoke(barrelRotationDelta);
            }
        }

        public void SetBarrelRotation(Quaternion angle)
        {
            barrel.localRotation = angle;
        }

        public void SetBaseRotation(Quaternion angle)
        {
            transform.localRotation = angle;
        }

        public void SetTrajectoryLine(Vector3[] points)
        {
            _trajectoryLineRenderer.positionCount = points.Length;
            _trajectoryLineRenderer.SetPositions(points);
        }
    }
}