using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpact;
    private float _power;
    private const float Gravity = 9.81f;
    private float _deltaTime;

    private Vector3 _velocity;
    private Vector3 _direction;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

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
        transform.position += _velocity * _deltaTime;
        transform.rotation = Quaternion.LookRotation(_velocity, Vector3.up);

        if (Physics.Raycast(transform.position, _velocity, out var hit, _velocity.magnitude * _deltaTime))
        {
            Vector3 normal = hit.normal;
            _velocity = Vector3.Reflect(_velocity, normal);
            _ricochetsCount++;

            if (_ricochetsCount >= 3)
            {
                DestroyBullet(hit);
            }
        }
        else
        {
            _velocity += Vector3.down * Gravity * _deltaTime;
        }
    }

    private void DestroyBullet(RaycastHit hit)
    {
        LeaveTrailOnSurface(hit.point);
        Destroy(gameObject);
    }

    private void LeaveTrailOnSurface(Vector3 hitPoint)
    {
        GameObject impactEffect = Instantiate(bulletImpact);
        impactEffect.transform.position = hitPoint;
        impactEffect.transform.up = hitPoint.normalized;
    }

    public void SetParameters(Vector3 direction, float power)
    {
        _direction = direction;
        _power = power;
    }
}