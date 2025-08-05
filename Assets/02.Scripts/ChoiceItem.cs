using UnityEngine;

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
