using UnityEngine;

namespace Animations
{
    public class GunRecoil : MonoBehaviour
    {
        [SerializeField] private float recoilForce = 0.4f;
        [SerializeField] private float recoilDuration = 0.2f;

        private Vector3 _originalPosition;
        private float _recoilTimer;

        private void Awake()
        {
            _originalPosition = transform.localPosition;
        }

        private void Update()
        {
            if (_recoilTimer > 0f)
            {
                float recoilPercent = _recoilTimer / recoilDuration;
                float recoilDistance = recoilForce * recoilPercent;
                transform.localPosition = _originalPosition + Vector3.back * recoilDistance;
                _recoilTimer -= Time.deltaTime;
            }
            else
            {
                _recoilTimer = 0f;
                transform.localPosition = _originalPosition;
            }
        }

        public void Fire()
        {
            _recoilTimer = recoilDuration;
        }
    }
}