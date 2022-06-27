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

    [SerializeField] private Transform _canvas;

    [SerializeField] private Player _player;
    [SerializeField] public PlayerMove pMove;

    private Text _explainText;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _shopTimeText;
    [SerializeField] private TextMeshProUGUI _monsterCntText;
    [SerializeField] private GameObject _titleObj;
    private Image _titleEffectImage;

    [SerializeField] private ShopButton _shopButton;
    [SerializeField] private GameObject playingShopUI;

    private Transform _coinShopUI;

    public bool isShopOpen = false;

    private void Awake()
    {
        Instance = this;
        _titleEffectImage = _titleObj.transform.Find("EffectSprite").GetComponent<Image>();
        spawnMonster = GetComponent<SpawnMonster>();

        _explainText = _canvas.Find("TimeShop/ExplainText").GetComponent<Text>();
        _timeText = _canvas.Find("Time/Text").GetComponent<TextMeshProUGUI>();
        _shopTimeText = _canvas.Find("TimeShop/Time/Text").GetComponent<TextMeshProUGUI>();

        _coinShopUI = _canvas.Find("CoinShop");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.gameType == GameManager.GameType.Ing) ShowShop(playingShopUI.transform);
            else ShowShop(_coinShopUI);
        }
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
        _shopTimeText.text = string.Format($"{_player.PlayerTime}s");

        _monsterCntText.text = string.Format($"{spawnMonster.monsters.Count} / {spawnMonster.maxSpawnCount}");
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.black; 
        
        GUI.Label(new Rect(1000, 10, 200, 50), "[ E ] Ű�� ���� ��������", labelStyle);
    }

    public void ShowShop(Transform shopTrm)
    {
        isShopOpen = !isShopOpen;
        if (isShopOpen == true)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(shopTrm.DOScale(Vector3.one, .5f));
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
            shopTrm.DOScale(Vector3.zero, .5f);

            _explainText.text = "";

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (GameManager.Instance.gameType == GameManager.GameType.Ing) CloseTimeShop();
            else CloseCoinShop();
        }
    }

    public void CloseTimeShop()
    {
        if (_shopButton.deathCnt > 0)
        {
            while (_shopButton.deathCnt != 0)
            {
                if (spawnMonster.monsters.Count > 0)
                {
                    int cnt = Random.Range(0, spawnMonster.monsters.Count);
                    spawnMonster.monsters[cnt].GetComponent<Monster>().Dead();
                }
                _shopButton.deathCnt--;
            }
        }

        if (pMove.moveSpd > 5)
        {
            StartCoroutine(Boost());
        }
    }

    public void CloseCoinShop()
    {

    }

    public IEnumerator Boost()
    {
        yield return new WaitForSeconds(3f);
        pMove.moveSpd -= 5;
    }

    public void ShopExplainMessage(string message)
    {
        _explainText.text = message;
    }
}