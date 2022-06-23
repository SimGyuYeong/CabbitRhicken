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
                        UIManager.Instance.ShopExplainMessage($"���ݻ�� �������� �����Ͽ� [ �� {deathCnt}���� ] �� ����մϴ�.");
                    }
                    else
                    {
                        UIManager.Instance.ShopExplainMessage("�ð��� �����Ͽ� ��ǰ ���Ű� �Ұ����մϴ�.");
                    }
                }
                else
                {
                    UIManager.Instance.ShopExplainMessage("���� ����ִ� ���� ��� ���ݻ���� �Ұ����մϴ�.");
                }
                
                break;
            case 2:
                if(UIManager.Instance.pMove.moveSpd <= 5)
                {
                    if (GameManager.Instance.player.PlayerTime >= 10)
                    {
                        GameManager.Instance.player.PlayerTime -= 10;
                        UIManager.Instance.pMove.moveSpd += 5;
                        UIManager.Instance.ShopExplainMessage($"�ν�Ʈ �������� �����߽��ϴ�.");
                    }
                    else
                    {
                        UIManager.Instance.ShopExplainMessage("�ð��� �����Ͽ� ��ǰ ���Ű� �Ұ����մϴ�.");
                    }
                }
                else
                {
                    UIManager.Instance.ShopExplainMessage("���� �ش� ��ǰ�� �̿����Դϴ�.");
                }
                break;
            default:
                break;
        }
    }
}
