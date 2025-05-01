using UnityEngine;

/// <summary>
/// Ʈ������ ���� �ٰ����� ��� ���� ��ũ��Ʈ
/// </summary>
/// 
public class GoldCoin : MonoBehaviour
{
    public float rotationSpeed = 180f; // �ʴ� ȸ�� �ӵ� (�� ����)
    public Transform m_CoinObject;

    void Update()
    {
        // Y�� �������� �� ������ ȸ��
        m_CoinObject.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
