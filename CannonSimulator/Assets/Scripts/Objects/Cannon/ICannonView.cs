using System;
using UnityEngine;

namespace Objects.Cannon
{
    public interface ICannonView
    {
        public Transform BarrelTransform { get; }
        public Transform SpawnPointTransform { get; }
        public IBulletGenerator BulletGenerator { get; }
        public event Action OnFireClicked;
        public event Action<float> OnBaseRotationChanged;
        public event Action<float> OnBarrelRotationChanged;
        public void SetBarrelRotation(Quaternion angle);
        public void SetBaseRotation(Quaternion angle);
        public void SetTrajectoryLine(Vector3[] points);
    }
}