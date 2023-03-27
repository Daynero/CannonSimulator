using UnityEngine;

public interface IBulletGenerator
{
    public void GenerateBullet(Vector3 direction, float firePower);
}