using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public SpawnMonster spawnMonster;

    [SerializeField] private Player _player;
    [SerializeField] private PlayerMove _pMove;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private GameObject _titleObj;
    private Image _titleEffectImage;

    private void Awake()
    {
        Instance = this;
        _titleEffectImage = _titleObj.transform.Find("EffectSprite").GetComponent<Image>();
        spawnMonster = GetComponent<SpawnMonster>();
    }

    private void Start()
    {
        TitleShow("Stage 1");
        StartCoroutine(transform.GetComponent<SpawnMonster>().MonsterSpawnCoroutine());
    }

    public void TitleShow(string text)
    {
        _titleObj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;

        Sequence seq = DOTween.Sequence();
        seq.Append(_titleEffectImage.transform.DOScale(new Vector3(420, 120, 1), .5f));
        seq.Join(_titleEffectImage.DOFade(0, .5f));
        seq.SetLoops(2);

        Sequence seq1 = DOTween.Sequence();
        seq1.AppendInterval(1f);
        seq1.Append(_titleObj.transform.DOLocalMove(new Vector3(0, 0, 0), .5f, true));
        seq1.Append(seq.Play());
        seq1.Append(_titleObj.transform.DOLocalMove(new Vector3(0, 440, 0), .5f, true));
    }

    public void UpdateTimeUI()
    {
        _timeText.text = string.Format($"{_player.PlayerTime}s");
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 30;
        labelStyle.normal.textColor = Color.white; 
        
        GUI.Label(new Rect(500, 10, 100, 50), "남은 몬스터 : " + spawnMonster.monsters.Count + "마리", labelStyle);
    }

}
