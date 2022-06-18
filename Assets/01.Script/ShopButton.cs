using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public int deathCnt = 0;

    public void ShopBuy(int slot)
    {
        switch(slot)
        {
            case 1:
                if(deathCnt <= UIManager.Instance.spawnMonster.monsters.Count)
                {
                    if (GameManager.Instance.player.PlayerTime >= 30)
                    {
                        GameManager.Instance.player.PlayerTime -= 30;
                        deathCnt++;
                        Debug.Log("구매");
                    }
                }
                
                break;
            case 2:
                if(UIManager.Instance.pMove.moveSpd <= 5)
                {
                    if (GameManager.Instance.player.PlayerTime >= 10)
                    {
                        GameManager.Instance.player.PlayerTime -= 10;
                        UIManager.Instance.pMove.moveSpd += 5;
                        Debug.Log("구매");
                    }
                }
                break;
            default:
                break;
        }
    }
}
