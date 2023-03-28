using UnityEngine;

namespace Objects.Cannon
{
    public class BulletImpactController : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, 10f);
        }
    }
}
