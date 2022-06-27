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
        Ing
    }
    public GameType gameType = GameType.Ready;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NextStage();
    }

    public void GameOver()
    {
        gameType = GameType.Ready;
        _stageCount = 0;
        player.Gold = 0;

        player.slowlyTimeCnt = 0;
        player.powerUpCnt = 0;
        player.timeStopCnt = 0;

        NextStage();
    }

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

    public void StageClear()
    {
        gameType = GameType.Ready;
        UIManager.Instance.TitleShow("Stage Clear!");
        GivenGold();
    }

    public void GivenGold()
    {
        player.Gold += player.PlayerTime * 5;
    }
}
