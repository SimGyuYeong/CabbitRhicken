using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public bool slowlyTimeIng = false; //시간 지연 스킬을 사용중인가?
    public bool powerUpIng = false; //각성 스킬을 사용중인가?
    public bool timeStopIng = false; //시간 정지 스킬을 사용중인가?

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
                        UIManager.Instance.SkillExplainMessage("시간지연 스킬을 사용했습니다.");
                        GameManager.Instance.player.slowlyTimeCnt -= 1;
                        slowlyTimeIng = true;
                        StartCoroutine(DelaySlowly());
                        UIManager.Instance.SlowlyStatusUpdate();
                    }
                    else
                    {
                        UIManager.Instance.SkillExplainMessage("현재 쿨타임 중입니다.");
                    }
                }
                else
                {
                    UIManager.Instance.SkillExplainMessage("해당 스킬을 보유하고 있지 않습니다.");
                }
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (GameManager.Instance.player.powerUpCnt > 0)
                {
                    if (powerUpIng == false)
                    {
                        UIManager.Instance.SkillExplainMessage("각성 스킬을 사용했습니다.");
                        GameManager.Instance.player.powerUpCnt -= 1;
                        powerUpIng = true;
                        StartCoroutine(DelayPowerUp());
                        UIManager.Instance.PowerUpStatusUpdate();
                    }
                    else
                    {
                        UIManager.Instance.SkillExplainMessage("현재 쿨타임 중입니다.");
                    }
                }
                else
                {
                    UIManager.Instance.SkillExplainMessage("해당 스킬을 보유하고 있지 않습니다.");
                }
            }

            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (GameManager.Instance.player.timeStopCnt > 0)
                {
                    if (timeStopIng == false)
                    {
                        UIManager.Instance.SkillExplainMessage("시간정지 스킬을 사용했습니다.");
                        GameManager.Instance.player.timeStopCnt -= 1;
                        timeStopIng = true;
                        StartCoroutine(DelayTimeStop());
                        UIManager.Instance.TimeStopStatusUpdate();
                    }
                    else
                    {
                      UIManager.Instance.SkillExplainMessage("현재 쿨타임 중입니다.");
                     }
                }
                else
                {
                    UIManager.Instance.SkillExplainMessage("해당 스킬을 보유하고 있지 않습니다.");
                }
            }
        }
    }

    /// <summary>
    /// 시간 지연 스킬 쿨타임 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelaySlowly()
    {
        yield return new WaitForSeconds(10f);
        slowlyTimeIng = false;
        UIManager.Instance.SlowlyStatusUpdate();
    }

    /// <summary>
    /// 각성 스킬 쿨타임 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayPowerUp()
    {
        yield return new WaitForSeconds(10f);
        powerUpIng = false;
        UIManager.Instance.PowerUpStatusUpdate();
    }

    /// <summary>
    /// 시간 정지 스킬 쿨타임 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayTimeStop()
    {
        yield return new WaitForSeconds(10f);
        timeStopIng = false;
        UIManager.Instance.TimeStopStatusUpdate();
    }

}
