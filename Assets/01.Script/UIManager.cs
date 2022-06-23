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
    [SerializeField] public PlayerMove pMove;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _monsterCntText;
    [SerializeField] private GameObject _titleObj;
    private Image _titleEffectImage;

    [SerializeField] private ShopButton _shopButton;
    [SerializeField] private GameObject playingShopUI;
    public bool isPlayingShopOpen = false;

    private void Awake()
    {
        Instance = this;
        _titleEffectImage = _titleObj.transform.Find("EffectSprite").GetComponent<Image>();
        spawnMonster = GetComponent<SpawnMonster>();
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

    public void UpdateStatusUI()
    {
        _timeText.text = string.Format($"{_player.PlayerTime}s");
        playingShopUI.transform.Find("Time/Text").GetComponent<TextMeshProUGUI>().text = string.Format($"{_player.PlayerTime}s");


        _monsterCntText.text = string.Format($"{spawnMonster.monsters.Count} / {spawnMonster.maxSpawnCount}");
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.black; 
        
        GUI.Label(new Rect(1000, 10, 200, 50), "[ E ] 키를 눌러 상점열기", labelStyle);
    }

    public void ShowPlayingShop()
    {
        isPlayingShopOpen = !isPlayingShopOpen;
        if(isPlayingShopOpen == true)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(playingShopUI.transform.DOScale(Vector3.one, 1f));
            seq.AppendCallback(() =>
            {
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            });
        }
        else
        {
            Time.timeScale = 1f;
            playingShopUI.transform.DOScale(Vector3.zero, 1f);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if(_shopButton.deathCnt > 0)
            {
                while (_shopButton.deathCnt != 0)
                {
                    if(spawnMonster.monsters.Count > 0)
                    {
                        int cnt = Random.Range(0, spawnMonster.monsters.Count);
                        spawnMonster.monsters[cnt].GetComponent<Monster>().Dead();
                    }
                    _shopButton.deathCnt--;
                }
            }

            if(pMove.moveSpd > 5)
            {
                StartCoroutine(Boost());
            }
        }
    }

    public IEnumerator Boost()
    {
        yield return new WaitForSeconds(3f);
        pMove.moveSpd -= 5;
    }
}
