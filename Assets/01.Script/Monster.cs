using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Tooltip("�÷��̾� ĳ��")]
    private Player _player;

    [Header("���� �Ӽ�")]
    public float damage; //������
    public float hp; //ü��
    public float attackRange; //���� ���� ��Ÿ�
    public float attackDelay; //���� ������
    public bool isAttacking = false; //�������ΰ�?
    public int addTime; // �ش� ���͸� �׿��� �� �߰��� �ð�

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// ���� �Լ�
    /// </summary>
    public void Attack()
    {
        //���� �������� �ƴ϶��
        if(isAttacking == false)
        {
            _player.SendMessage("DamagedMonster", damage);
            isAttacking = true;
            StartCoroutine(AttackDelay());
        }
    }

    /// <summary>
    /// ���� ������ �� �ٽ� ���� �����ϰ� ���ִ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    /// <summary>
    /// �׾����� �Լ�
    /// </summary>
    public void Dead()
    {
        _player.PlayerTime += addTime;
        UIManager.Instance.spawnMonster.monsters.Remove(gameObject);
        Destroy(gameObject);
    }
}
