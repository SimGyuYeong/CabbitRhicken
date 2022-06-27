using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public bool slowlyTimeIng = false; //�ð� ���� ��ų�� ������ΰ�?
    public bool powerUpIng = false; //���� ��ų�� ������ΰ�?
    public bool timeStopIng = false; //�ð� ���� ��ų�� ������ΰ�?

    private void Update()
    {
        if(GameManager.Instance.gameType == GameManager.GameType.Ing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(GameManager.Instance.player.slowlyTimeCnt > 0)
                {
                    if(slowlyTimeIng == false)
                    {
                        UIManager.Instance.SkillExplainMessage("�ð����� ��ų�� ����߽��ϴ�.");
                        GameManager.Instance.player.slowlyTimeCnt -= 1;
                        slowlyTimeIng = true;
                        StartCoroutine(DelaySlowly());
                        UIManager.Instance.SlowlyStatusUpdate();
                    }
                    else
                    {
                        UIManager.Instance.SkillExplainMessage("���� ��Ÿ�� ���Դϴ�.");
                    }
                }
                else
                {
                    UIManager.Instance.SkillExplainMessage("�ش� ��ų�� �����ϰ� ���� �ʽ��ϴ�.");
                }
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (GameManager.Instance.player.powerUpCnt > 0)
                {
                    if (powerUpIng == false)
                    {
                        UIManager.Instance.SkillExplainMessage("���� ��ų�� ����߽��ϴ�.");
                        GameManager.Instance.player.powerUpCnt -= 1;
                        powerUpIng = true;
                        StartCoroutine(DelayPowerUp());
                        UIManager.Instance.PowerUpStatusUpdate();
                    }
                    else
                    {
                        UIManager.Instance.SkillExplainMessage("���� ��Ÿ�� ���Դϴ�.");
                    }
                }
                else
                {
                    UIManager.Instance.SkillExplainMessage("�ش� ��ų�� �����ϰ� ���� �ʽ��ϴ�.");
                }
            }

            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (GameManager.Instance.player.timeStopCnt > 0)
                {
                    if (timeStopIng == false)
                    {
                        UIManager.Instance.SkillExplainMessage("�ð����� ��ų�� ����߽��ϴ�.");
                        GameManager.Instance.player.timeStopCnt -= 1;
                        timeStopIng = true;
                        StartCoroutine(DelayTimeStop());
                        UIManager.Instance.TimeStopStatusUpdate();
                    }
                    else
                    {
                      UIManager.Instance.SkillExplainMessage("���� ��Ÿ�� ���Դϴ�.");
                     }
                }
                else
                {
                    UIManager.Instance.SkillExplainMessage("�ش� ��ų�� �����ϰ� ���� �ʽ��ϴ�.");
                }
            }
        }
    }

    /// <summary>
    /// �ð� ���� ��ų ��Ÿ�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelaySlowly()
    {
        yield return new WaitForSeconds(10f);
        slowlyTimeIng = false;
        UIManager.Instance.SlowlyStatusUpdate();
    }

    /// <summary>
    /// ���� ��ų ��Ÿ�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayPowerUp()
    {
        yield return new WaitForSeconds(10f);
        powerUpIng = false;
        UIManager.Instance.PowerUpStatusUpdate();
    }

    /// <summary>
    /// �ð� ���� ��ų ��Ÿ�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayTimeStop()
    {
        yield return new WaitForSeconds(10f);
        timeStopIng = false;
        UIManager.Instance.TimeStopStatusUpdate();
    }

}
