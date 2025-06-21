using TMPro;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public TextMeshProUGUI m_CoinText;

    public void GetCoin(int coin)
    {
        m_CoinText.text = $"COIN : {coin.ToString()}";
    }
}
