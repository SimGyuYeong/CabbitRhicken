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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NextStage();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.ShowPlayingShop();
        }
    }

    public void NextStage()
    {
        _spawnMonster.Init(stages[_stageCount].monsterCount);

        player.transform.localPosition = _spawnPos;

        UIManager.Instance.TitleShow($"Stage {_stageCount + 1}");
        player.PlayerTime = stages[_stageCount].second;
        StartCoroutine(_spawnMonster.MonsterSpawnCoroutine());
        _stageCount++;
    }
}
