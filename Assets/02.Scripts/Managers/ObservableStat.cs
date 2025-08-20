using System;
using UnityEngine;

/// <summary>
/// 옵저버 패턴으로 만들게 된, 플레이어의 모든 데이터 스탯 관리
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObservableStat<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (!_value.Equals(value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public event Action<T> OnValueChanged;

    public ObservableStat(T initialValue)
    {
        _value = initialValue;
    }
}

public static class UIDataStats
{
    public static ObservableStat<int> Coin = new ObservableStat<int>(0); // 0 부터 시작
    public static ObservableStat<int> Exp = new ObservableStat<int>(0);
    public static ObservableStat<int> Level = new ObservableStat<int>(1); // 레벨은 1 부터 시작

    public static void Reset()
    {
        Coin.Value = 0;
        Exp.Value = 0;
        Level.Value = 1;
        // 필요하면 다른 스탯도 초기화
    }
}