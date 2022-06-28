using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int second;
    public int monsterCount;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<StageData> stages = new List<StageData>();

    [SerializeField] private SpawnMonster _spawnMonster;
    public Player player;
    public int maxMonsterCnt;
    private int _stageCount = 0;

    [SerializeField] private Vector3 _spawnPos;

    public enum GameType
    {
        Ready,
        Ing,
        DIe
    }
    public GameType gameType = GameType.Ready;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 플레이어가 죽고 다시 시작할때 실행될 함수
    /// </summary>
    public void Retry()
    {
        gameType = GameType.Ready;
        _stageCount = 0;
        player.Gold = 0;

        player.slowlyTimeCnt = 0;
        player.powerUpCnt = 0;
        player.timeStopCnt = 0;

        NextStage();
    }

    /// <summary>
    /// 다음 스테이지로 넘어갈때 
    /// </summary>
    public void NextStage()
    {
        UIManager.Instance.SkillStatusUpdate?.Invoke();
        gameType = GameType.Ing;
        _spawnMonster.Init(stages[_stageCount].monsterCount);

        player.transform.localPosition = _spawnPos;

        UIManager.Instance.TitleShow($"Stage {_stageCount + 1}");
        player.PlayerTime = stages[_stageCount].second;
        StartCoroutine(_spawnMonster.MonsterSpawnCoroutine());
        _stageCount++;
    }

    /// <summary>
    /// 해당 스테이지를 깼을 때
    /// </summary>
    public void StageClear()
    {
        gameType = GameType.Ready;
        UIManager.Instance.TitleShow("Stage Clear!");
        GivenGold();
    }

    /// <summary>
    /// 시간 비례해서 코인 지급
    /// </summary>
    public void GivenGold()
    {
        player.Gold += player.PlayerTime * 5;
    }
}
