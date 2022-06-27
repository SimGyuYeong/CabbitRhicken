using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public int deathCnt = 0;

    public void RemoteKill()
    {
         if(deathCnt <= UIManager.Instance.spawnMonster.monsters.Count)
         {
                if (GameManager.Instance.player.PlayerTime >= 60)
                {
                     GameManager.Instance.player.PlayerTime -= 60;
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
    }

    public void Boost()
    {
        if (UIManager.Instance.pMove.moveSpd <= 5)
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
    }

    public void SlowlyTime()
    {
        if(GameManager.Instance.player.Gold >= 250)
        {
            GameManager.Instance.player.Gold -= 250;
            GameManager.Instance.player.slowlyTimeCnt += 1;
            UIManager.Instance.ShopExplainMessage("�ð� ���� �������� �����߽��ϴ�.");
        }
        else
        {
            UIManager.Instance.ShopExplainMessage("���� �����Ͽ� ��ǰ ���Ű� �Ұ����մϴ�.");
        }
    }

    public void PowerUp()
    {
        if (GameManager.Instance.player.Gold >= 100)
        {
            GameManager.Instance.player.Gold -= 100;
            GameManager.Instance.player.powerUpCnt += 1;
            UIManager.Instance.ShopExplainMessage("���� �������� �����߽��ϴ�.");
        }
        else
        {
            UIManager.Instance.ShopExplainMessage("���� �����Ͽ� ��ǰ ���Ű� �Ұ����մϴ�.");
        }
    }

    public void TimeStop()
    {
        if (GameManager.Instance.player.Gold >= 500)
        {
            GameManager.Instance.player.Gold -= 500;
            GameManager.Instance.player.timeStopCnt += 1;
            UIManager.Instance.ShopExplainMessage("�ð����� �������� �����߽��ϴ�.");
        }
        else
        {
            UIManager.Instance.ShopExplainMessage("���� �����Ͽ� ��ǰ ���Ű� �Ұ����մϴ�.");
        }
    }
}
