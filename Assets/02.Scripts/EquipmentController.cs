using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// Slot���� ������ �� �ִ� ������ �̸� ���������� ���� ������ ���� �Ұ�.
/// Scriptable Object�� ����� ������ ����
/// </summary>
/// 
public class EquipmentController : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public EquipmentSlotType slotType;          // � ��������
        public Transform attachPoint;               // ��� ���� ��ġ (�� ������Ʈ)
        [HideInInspector] public GameObject currentObject; // ���� ������ ���
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