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
            // bounds�� ũ�� ���� �� �׻� �þ� �ȿ� �ִٰ� ����
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one);
        }
    }
}
