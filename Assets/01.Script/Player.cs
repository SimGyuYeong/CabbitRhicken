using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _playerTime = 100;
    public int PlayerTime
    {
        get => _playerTime;
        set
        {
            _playerTime = value;
            UIManager.Instance.UpdateTimeUI();
        }
    }

    private void Start()
    {
        StartCoroutine(ReductionTime());
    }

    IEnumerator ReductionTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            PlayerTime -= 1;
        }
    }

    public void DamagedMonster(int damage)
    {
        PlayerTime -= damage;
    }
}
