using TMPro;
using UnityEngine;

public class HitEffectController : MonoBehaviour
{
    [SerializeField] Transform damageFontAnchor;
    
    public void PlayEffectByType(string type, Vector3 attackerPos)
    {

    }

    public void PlayDamageFont(float damage, Vector3 attackerPos)
    {
        // Damage Text UI ����
        GameObject dmgText = ResourceManager.Instance.InstantiatePrefab("UI/T_Damage");
        dmgText.GetComponent<TextMeshPro>().text = damage.ToString();

        // �ش� �������� offset�� ������ ��ġ
        Vector3 fromPlayerDir = (attackerPos - transform.position).normalized;
        Vector3 offsetPos = damageFontAnchor.position + fromPlayerDir * 0.5f;
        dmgText.transform.position = offsetPos;
    }
}
