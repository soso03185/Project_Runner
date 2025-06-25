using TMPro;
using UnityEngine;

/// <summary>
/// 트레일을 따라 다가오는 골드 코인 스크립트
/// </summary>
/// 
public class GoldCoin : LaneObject
{
    public float rotationSpeed = 180f; // 초당 회전 속도 (도 단위)
    public Transform m_CoinObject;

    public override void Init() { }

    public override void Die() 
    {
        // Coin Effect 생성
        GameObject fxCoin = ResourceManager.Instance.InstantiatePrefab("FX/FX_Coin");
        fxCoin.transform.position = transform.position;

        ResourceManager.Instance.Destroy(gameObject);
    }

    void Update()
    {
        // Y축 기준으로 매 프레임 회전
        m_CoinObject.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
