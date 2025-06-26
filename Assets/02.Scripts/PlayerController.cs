using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static Define;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// Player�� ��ü���� ������ ���ִ� ��
/// </summary>
/// 
public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float m_FallMultiplier = 3f;
    [SerializeField] private float m_LowJumpMultiplier = 5f;

    float m_ForwardSpeed = 5f;          // ���� �ӵ�
    float m_LaneDistance = 1.0f;        // ���� �� �Ÿ�
    float m_LaneSwitchSpeed = 10f;      // �¿� ��ȯ �ӵ�
    float m_JumpForce = 7f;             // ���� ��
    int m_CurrentLane = 1;              // 0 = ����, 1 = �߰�, 2 = ������
    float m_AttackDamage = 99;

    bool m_IsGrounded = true;
    bool m_IsKnockBack = false;
    Vector3 m_TargetPos;

    Define.State m_State = Define.State.Idle;
    EquipmentController equipmentController;
    Rigidbody m_Rigidbody;

    void Awake()
    {
        equipmentController = GetComponent<EquipmentController>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        m_TargetPos = transform.position;
    }

    private void FixedUpdate()
    {
        // ���� ��
        if (m_Rigidbody.linearVelocity.y < 0)
        {
            // ���� ���� ��ȭ
            Vector3 extraGravity = (m_FallMultiplier - 1) * Physics.gravity;
            m_Rigidbody.AddForce(extraGravity, ForceMode.Acceleration);
        }
        else if (m_Rigidbody.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // ���� ��ư ���� �� ������ ��� ����
            Vector3 extraGravity = (m_LowJumpMultiplier - 1) * Physics.gravity;
            m_Rigidbody.AddForce(extraGravity, ForceMode.Acceleration);
        }

        // �¿� ��ġ ����
        float newX = Mathf.Lerp(transform.position.x, m_TargetPos.x, m_LaneSwitchSpeed * Time.fixedDeltaTime);

        // ���� �Ÿ� ���
        float newZ = transform.position.z + m_ForwardSpeed * Time.fixedDeltaTime;

        Vector3 newPosition = new Vector3(newX, transform.position.y, newZ);
        m_Rigidbody.MovePosition(newPosition);
    }

    void Update()
    {
        HandleInput();
        UpdateTargetPosition();
        UpdateState();
    }

    private void UpdateState()
    {
        switch (m_State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Jump:
                UpdateJump();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && m_CurrentLane > 0)
        {
            m_CurrentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && m_CurrentLane < 2)
        {
            m_CurrentLane++;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
            m_State = Define.State.Jump;
        }
    }

    private void UpdateTargetPosition()
    {
        // ��ǥ ��ġ ��� (X ��ġ�� ����)
        float targetX = m_CurrentLane * m_LaneDistance;
        m_TargetPos = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    public void UpdateDie()
    {

    }
    public void UpdateIdle()
    {

    }
    public void UpdateMoving()
    {

    }
    public void UpdateJump()
    {

    }
    public void UpdateAttack()
    {

    }

    public void EquipItem(EquipItemData newItem)
    {
        equipmentController.Equip(newItem);
    }

    public void TakeDamage(float damage, Vector3 attackPos)
    {
    }

    void Attack(LaneObject laneObj)
    {
        if (laneObj.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(m_AttackDamage, transform.position);
        }
        GameObject fxSlash = ResourceManager.Instance.InstantiatePrefab("FX/FX_Slash_Blue");
        fxSlash.transform.position = laneObj.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            UIController.Instance.GetCoin(++DataManager.m_GameCoin);
            other.gameObject.GetComponent<GoldCoin>().Die();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            LaneObject laneObj = other.gameObject.GetComponent<LaneObject>();
            if (laneObj.m_CurrentLane == m_CurrentLane)
            {
                if (m_IsKnockBack == false)
                {
                    StartCoroutine(CoKnockBack());
                    Attack(laneObj);
                }
            }
        }
    }

    IEnumerator CoKnockBack()
    {
        m_IsKnockBack = true;
        m_Rigidbody.AddForce(Vector3.back * m_JumpForce * 1.5f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);
        m_IsKnockBack = false;
        yield break;
    }
}
