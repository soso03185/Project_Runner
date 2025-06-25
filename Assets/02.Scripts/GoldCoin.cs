using TMPro;
using UnityEngine;

/// <summary>
/// Ʈ������ ���� �ٰ����� ��� ���� ��ũ��Ʈ
/// </summary>
/// 
public class GoldCoin : LaneObject
{
    public float rotationSpeed = 180f; // �ʴ� ȸ�� �ӵ� (�� ����)
    public Transform m_CoinObject;

    public override void Init() { }

    public override void Die() 
    {
        // Coin Effect ����
        GameObject fxCoin = ResourceManager.Instance.InstantiatePrefab("FX/FX_Coin");
        fxCoin.transform.position = transform.position;

        ResourceManager.Instance.Destroy(gameObject);
    }

    void Update()
    {
        // Y�� �������� �� ������ ȸ��
        m_CoinObject.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
