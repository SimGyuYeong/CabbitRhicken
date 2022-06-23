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
                    if (GameManager.Instance.player.PlayerTime >= 100)
                    {
                        GameManager.Instance.player.PlayerTime -= 30;
                        deathCnt++;
                        UIManager.Instance.ShopExplainMessage($"원격사살 아이템을 구매하여 [ 적 {deathCnt}마리 ] 를 사살합니다.");
                    }
                    else
                    {
                        UIManager.Instance.ShopExplainMessage("시간이 부족하여 상품 구매가 불가능합니다.");
                    }
                }
                else
                {
                    UIManager.Instance.ShopExplainMessage("현재 살아있는 적이 없어서 원격사살이 불가능합니다.");
                }
                
                break;
            case 2:
                if(UIManager.Instance.pMove.moveSpd <= 5)
                {
                    if (GameManager.Instance.player.PlayerTime >= 10)
                    {
                        GameManager.Instance.player.PlayerTime -= 10;
                        UIManager.Instance.pMove.moveSpd += 5;
                        UIManager.Instance.ShopExplainMessage($"부스트 아이템을 구매했습니다.");
                    }
                    else
                    {
                        UIManager.Instance.ShopExplainMessage("시간이 부족하여 상품 구매가 불가능합니다.");
                    }
                }
                else
                {
                    UIManager.Instance.ShopExplainMessage("현재 해당 상품을 이용중입니다.");
                }
                break;
            default:
                break;
        }
    }
}
