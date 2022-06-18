using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Tooltip("플레이어 캐싱")]
    private Player _player;

    [Header("전투 속성")]
    public float damage; //데미지
    public float hp; //체력
    public float attackRange; //공격 가능 사거리
    public float attackDelay; //공격 딜레이
    public bool isAttacking = false; //공격중인가?
    public int addTime; // 해당 몬스터를 죽였을 때 추가할 시간

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// 공격 함수
    /// </summary>
    public void Attack()
    {
        //만약 공격중이 아니라면
        if(isAttacking == false)
        {
            _player.SendMessage("DamagedMonster", damage);
            isAttacking = true;
            StartCoroutine(AttackDelay());
        }
    }

    /// <summary>
    /// 공격 딜레이 후 다시 공격 가능하게 해주는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    /// <summary>
    /// 죽었을때 함수
    /// </summary>
    public void Dead()
    {
        _player.PlayerTime += addTime;
        UIManager.Instance.spawnMonster.monsters.Remove(gameObject);
        Destroy(gameObject);
    }
}
