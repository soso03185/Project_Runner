using TMPro;
using UnityEngine;

/// <summary>
/// 자동으로 업데이트 할 수 있게, 옵저버 패턴과 연결된 UI의 컨트롤러
/// </summary>
/// 
public class UIController : Singleton<UIController>
{
    public TextMeshProUGUI m_CoinText;
    public TextMeshProUGUI m_LevelText;
    public TextMeshProUGUI m_ExpText;

    void OnEnable()
    {
        UIDataStats.Coin.OnValueChanged += UpdateCoinUI;
        UIDataStats.Exp.OnValueChanged += UpdateExpUI;
        UIDataStats.Level.OnValueChanged += UpdateLevelUI;
    }

    void OnDisable()
    {
        UIDataStats.Coin.OnValueChanged -= UpdateCoinUI;
        UIDataStats.Exp.OnValueChanged -= UpdateExpUI;
        UIDataStats.Level.OnValueChanged -= UpdateLevelUI;
    }

    void UpdateCoinUI(int coin) 
    {
        m_CoinText.text = $"COIN : {coin.ToString()}";
    }

    void UpdateExpUI(int exp)
    {
        m_ExpText.text = $"EXP : {exp.ToString()}";
    }

    void UpdateLevelUI(int level)
    {
        m_LevelText.text = $"LV : {level.ToString()}";
    }
}
