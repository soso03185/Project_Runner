
using UnityEngine;
using static Define;

[CreateAssetMenu(menuName = "Item/EquipItem", fileName = "NewEquipItem")]
public class EquipItemData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string itemName;
    public EquipmentSlotType slotType;
    public Sprite icon;

    [Header("���� ������")]
    public GameObject prefab;

    [Header("���� ��ġ ����")]
    public Vector3 localPosition = Vector3.zero;
    public Vector3 localRotation = Vector3.zero;
    public Vector3 localScale = Vector3.one;

    [Header("����")]
    public int power = 0;
    public float speedBonus = 0f;
}