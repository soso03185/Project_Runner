using System;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public abstract class LaneObject : MonoBehaviour
{
    public int m_CurrentLane;

    public abstract void Init();
    public abstract void Die();
}
