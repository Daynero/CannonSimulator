using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private int vertexCount = 10;
    [SerializeField] private float maxRadius = 0.5f;

    public void GenerateBullet(Vector3 direction, float firePower)
    {
        Mesh mesh = CreateBulletMesh();
        Bullet bullet = InstantiateBullet(mesh);
        bullet.SetParameters(direction, firePower);
    }

    private Mesh CreateBulletMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = GenerateRandomVertices();
        int[] triangles = GenerateTriangles(vertices);
        SetCenterVertex(vertices);
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private Vector3[] GenerateRandomVertices()
    {
        Vector3[] vertices = new Vector3[vertexCount + 1];
        for (int i = 0; i < vertexCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * maxRadius;
            vertices[i] = new Vector3(randomPoint.x, randomPoint.y, 0f);
        }
        return vertices;
    }

    private int[] GenerateTriangles(Vector3[] vertices)
    {
        int[] triangles = new int[vertexCount * 3];
        for (int i = 0; i < vertexCount; i++)
        {
            int j = (i + 1) % vertexCount;
            triangles[i * 3] = i;
            triangles[i * 3 + 1] = j;
            triangles[i * 3 + 2] = vertexCount;
        }
        return triangles;
    }

    private void SetCenterVertex(Vector3[] vertices)
    {
        Vector3 center = Vector3.zero;
        for (int i = 0; i < vertexCount; i++)
        {
            center += vertices[i];
        }
        center /= vertexCount;
        vertices[vertexCount] = center;
    }

    private Bullet InstantiateBullet(Mesh mesh)
    {
        Bullet bullet = Instantiate(bulletPrefab, spawnPointTransform.position, spawnPointTransform.rotation);
        bullet.MeshFilter.mesh = mesh;
        bullet.MeshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); // Randomize material color
        return bullet;
    }

}