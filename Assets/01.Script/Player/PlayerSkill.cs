using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public bool slowlyTimeIng = false; //�ð� ���� ��ų�� ������ΰ�?
    public bool powerUpIng = false; //���� ��ų�� ������ΰ�?
    public bool timeStopIng = false; //�ð� ���� ��ų�� ������ΰ�?

    public GameObject powerupParticle;
    public GameObject slowlyParticle;

    public Transform timestopTrm;
    public Image timestopImage;
    public Transform slowlytimeTrm;

    private void Update()
    {
        if(GameManager.Instance.gameType == GameManager.GameType.Ing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (GameManager.Instance.player.slowlyTimeCnt > 0)
                {
                    if(slowlyTimeIng == false)
                    {
                        slowlytimeTrm.localPosition = new Vector3(1100, 0);

                        UIManager.Instance.SkillExplainMessage("�ð����� ��ų�� ����߽��ϴ�.");
                        GameManager.Instance.player.slowlyTimeCnt -= 1;
                        slowlyTimeIng = true;
                        StartCoroutine(DelaySlowly());
                        UIManager.Instance.SlowlyStatusUpdate();

                        Sequence seq = DOTween.Sequence();
                        seq.Append(slowlytimeTrm.DOLocalMoveX(100, 0.5f));
                        seq.AppendCallback(() => Instantiate(slowlyParticle, transform));
                        seq.Append(slowlytimeTrm.DOLocalMoveX(-100, 1f));
                        seq.Append(slowlytimeTrm.DOLocalMoveX(-1100, 0.5f));
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
                        Instantiate(powerupParticle, transform);
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

                        timestopTrm.DOScale(Vector3.one, 0);
                        timestopImage.DOFade(1, 0);
                        timestopImage.DOColor(Color.white, 0);

                        Sequence seq = DOTween.Sequence();
                        seq.Append(timestopTrm.DOScale(Vector3.one * 1.2f, 0.2f));
                        seq.Append(timestopTrm.DOScale(Vector3.one * 0.9f, 0.2f));
                        seq.Append(timestopTrm.DOScale(Vector3.one, 0.2f));
                        seq.Append(timestopImage.DOColor(Color.red, .2f));
                        seq.AppendInterval(0.2f);
                        seq.Append(timestopTrm.DOScale(Vector3.one * 5f, 1.5f));
                        seq.Join(timestopImage.DOFade(0, 1.5f));
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
