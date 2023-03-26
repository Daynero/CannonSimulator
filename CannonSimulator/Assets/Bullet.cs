using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _power;
    private const float Gravity = 9.81f;
    private float _deltaTime;  
    
    private Vector3 _velocity;
    private Vector3 _direction;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private bool _isDestroyed;
    private int _ricochetsCount;
    private TrailRenderer _trailRenderer;

    public MeshFilter MeshFilter => _meshFilter;
    public MeshRenderer MeshRenderer => _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        _velocity = _direction.normalized * _power;
        _deltaTime = Time.deltaTime;
    }

    private void Update()
    {
        if (!_isDestroyed)
        {
            transform.position += _velocity * _deltaTime;
            transform.rotation = Quaternion.LookRotation(_velocity, Vector3.up);

            if (Physics.Raycast(transform.position, _velocity, out var hit, _velocity.magnitude * _deltaTime))
            {
                Vector3 normal = hit.normal;
                _velocity = Vector3.Reflect(_velocity, normal);
                _ricochetsCount++;

                if (_ricochetsCount >= 3)
                {
                    _isDestroyed = true;
                    _trailRenderer.emitting = false;
                    LeaveTrailOnSurface(hit.point);
                    Destroy(gameObject, 2f);
                }
            }
            else
            {
                _velocity += Vector3.down * Gravity * _deltaTime;
            }
        }
    }

    private void LeaveTrailOnSurface(Vector3 hitPoint)
    {
        GameObject impactEffect = Instantiate(Resources.Load<GameObject>("BulletImpact"));
        impactEffect.transform.position = hitPoint;
        impactEffect.transform.up = hitPoint.normalized;
    }

    public void SetParameters(Vector3 direction, float power)
    {
        _direction = direction;
        _power = power;
    }
}
