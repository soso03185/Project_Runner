
using UnityEngine;
using static Define;

[CreateAssetMenu(menuName = "Item/EquipItem", fileName = "NewEquipItem")]
public class EquipItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    public EquipmentSlotType slotType;
    public Sprite icon;

    [Header("외형 프리팹")]
    public GameObject prefab;

    [Header("로컬 위치 조정")]
    public Vector3 localPosition = Vector3.zero;
    public Vector3 localRotation = Vector3.zero;
    public Vector3 localScale = Vector3.one;

    [Header("스탯")]
    public int power = 0;
    public float speedBonus = 0f;
}