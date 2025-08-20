using UnityEngine;

/// <summary>
/// 도로위에 선택지로 아이템을 선택했을때 또 선택하게 되는 아이템
/// </summary>
/// 
public class ChoiceItem : MonoBehaviour
{
    public EquipItemData itemToEquip;  // Inspector에서 Sword.asset 같은 걸 연결

    public void PlayEffect()
    {
     //   this.gameObject.SetActive(false);
    }

    public void TestEquipChoice()
    {
        this.gameObject.SetActive(false);
        GameScene.Instance.m_Player.EquipItem(itemToEquip);
    }
}
