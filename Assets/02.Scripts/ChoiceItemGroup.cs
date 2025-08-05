using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceItemGroup : MonoBehaviour
{
    List<ChoiceItem> m_choiceItems = new List<ChoiceItem>();
    public float m_laneDistOffset = 0;
    float m_laneDistance;

    public string m_ItemName = "ChoiceItem";

    public void Start()
    {
        m_laneDistance = GameScene.Instance.m_Player.m_LaneDistance;

        // [-1, 0, 1] -> [ÁÂ Áß ¿ì] 
        for (int i = -1; i < 2; i++) 
        {
            GameObject go = ResourceManager.Instance.InstantiatePrefab(m_ItemName);
            go.transform.position = new Vector3((m_laneDistance + m_laneDistOffset) * i, transform.position.y, transform.position.z);
            go.transform.parent = transform;

            ChoiceItem item = go.GetOrAddComponent<ChoiceItem>();
            m_choiceItems.Add(item);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int laneIndex = GameScene.Instance.m_Player.m_CurLaneIndex;

        switch(laneIndex)
        {
            case -1: // ÁÂ
                m_choiceItems[0].PlayEffect();
                m_choiceItems[0].TestEquipChoice();
                break;
            case 0:  // Áß
                m_choiceItems[1].PlayEffect();
                m_choiceItems[1].TestEquipChoice();
                break;
            case 1:  // ¿ì
                m_choiceItems[2].PlayEffect();
                m_choiceItems[2].TestEquipChoice();
                break;
        }
    }
}
