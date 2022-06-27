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
            UIManager.Instance.UpdateStatusUI();
            if (_playerTime <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private int _gold = 0;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            UIManager.Instance.UpdateStatusUI();
            if (_gold < 0) _gold = 0;
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
            if (UIManager.Instance.isShopOpen == false && GameManager.Instance.gameType == GameManager.GameType.Ing)
                PlayerTime -= 1;
        }
    }

    public void DamagedMonster(int damage)
    {
        PlayerTime -= damage;
    }
}
