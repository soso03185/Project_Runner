using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Tiles : MonoBehaviour
{
    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            // bounds를 크게 설정 → 항상 시야 안에 있다고 간주
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one);
        }
    }
}
