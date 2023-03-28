using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 0.05f;
    [SerializeField] private float shakeDuration = 0.1f;

    private Vector3 _originalPosition;
    private float _shakeTimer;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_shakeTimer > 0f)
        {
            transform.localPosition = _originalPosition + Random.insideUnitSphere * shakeAmount;
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            _shakeTimer = 0f;
            transform.localPosition = _originalPosition;
        }
    }

    public void ShakeCamera()
    {
        _shakeTimer = shakeDuration;
    }
}