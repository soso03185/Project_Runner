using System;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

/// <summary>
/// 도로 위 오브젝트의 부모 스크립트
/// </summary>
public abstract class LaneObject : MonoBehaviour
{
    public int m_CurrentLane;

    public abstract void Init();
    public abstract void Die();
}
