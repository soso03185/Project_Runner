using UnityEngine;

/// <summary>
/// 전역적으로 공통해서 사용하는 곳 관리
/// </summary>
/// 
public class Define
{
    public static void DEBUG_LOG(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
    public static void DEBUG_ERROR(string msg)
    {
#if UNITY_EDITOR
        Debug.LogError(msg);
#endif
    }

}
