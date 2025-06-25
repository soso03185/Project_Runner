using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// Slot으로 장착할 수 있는 부위를 미리 설정해주지 않은 부위는 장착 불가.
/// Scriptable Object를 사용한 아이템 장착
/// </summary>
/// 
public class EquipmentController : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public EquipmentSlotType slotType;          // 어떤 부위인지
        public Transform attachPoint;               // 장비가 붙을 위치 (빈 오브젝트)
        [HideInInspector] public GameObject currentObject; // 현재 장착된 장비
    }

    [SerializeField] private List<Slot> slots;
    public void Equip(EquipItemData item)
    {
        var slot = slots.Find(s => s.slotType == item.slotType);
        if (slot == null || slot.attachPoint == null) return;

        if (slot.currentObject != null)
           ResourceManager.Instance.Destroy(slot.currentObject);
        
        var go = ResourceManager.Instance.InstantiatePrefab(item.itemName, slot.attachPoint);
        go.transform.localPosition = item.localPosition;
        go.transform.localEulerAngles = item.localRotation;
        go.transform.localScale = item.localScale;

        slot.currentObject = go;
    }
}