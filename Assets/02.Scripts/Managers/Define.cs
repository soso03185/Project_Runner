using UnityEngine;

/// <summary>
/// ���������� �����ؼ� ����ϴ� �� ����
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
