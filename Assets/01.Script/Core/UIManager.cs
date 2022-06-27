using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public SpawnMonster spawnMonster;

    [SerializeField] private Transform _canvas;

    [SerializeField] private Player _player;
    [SerializeField] public PlayerMove pMove;

    private Text _timeExplainText; //�ð� ���� �ȳ� �޼���
    private Text _coinExplainText; //���� ���� �ȳ� �޼���
    private TextMeshProUGUI _timeText; //UI �ð�ǥ�� �ؽ�Ʈ
    private TextMeshProUGUI _shopTimeText; //�ð� ���� �ð�ǥ�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _monsterCntText; //UI ���� �� ǥ�� �ؽ�Ʈ

    private Image _slowly; //�ð� ���� ��ų �̹���
    private Image _powerUP; //���� ��ų �̹���
    private Image _timeStop; //�ð� ���� ��ų �̹���
    public Action SkillStatusUpdate;
    private Image _skillExplainText; //��ų ��� ���� �ؽ�Ʈ

    private GameObject _gameStopUI; //���� ���� UI

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

        _timeExplainText = _canvas.Find("TimeShop/ExplainText").GetComponent<Text>();
        _timeText = _canvas.Find("Time/Text").GetComponent<TextMeshProUGUI>();
        _shopTimeText = _canvas.Find("TimeShop/Time/Text").GetComponent<TextMeshProUGUI>();

        _coinShopUI = _canvas.Find("CoinShop");
        _coinExplainText = _coinShopUI.Find("ExplainText").GetComponent<Text>();

        _slowly = _canvas.Find("SkillInv/SlowlyTime/Icon").GetComponent<Image>();
        _powerUP = _canvas.Find("SkillInv/PowerUp/Icon").GetComponent<Image>();
        _timeStop = _canvas.Find("SkillInv/TimeStop/Icon").GetComponent<Image>();
        _skillExplainText = _canvas.Find("SkillInv/Explain").GetComponent<Image>();

        _gameStopUI = _canvas.Find("GameStop").gameObject;
    }

    private void Start()
    {
        SkillStatusUpdate += SlowlyStatusUpdate;
        SkillStatusUpdate += PowerUpStatusUpdate;
        SkillStatusUpdate += TimeStopStatusUpdate;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.gameType == GameManager.GameType.Ing) ShowShop(playingShopUI.transform);
            else ShowShop(_coinShopUI);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_gameStopUI.activeSelf == false)
            {
                _gameStopUI.SetActive(true);
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                _gameStopUI.SetActive(false);
                Time.timeScale = 1f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            
        }
    }

    /// <summary>
    /// �ȳ� �޼��� ����ִ� �Լ�
    /// </summary>
    /// <param name="text">�޼���</param>
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

    /// <summary>
    /// UI ������Ʈ
    /// </summary>
    public void UpdateStatusUI()
    {
        _timeText.text = string.Format($"{_player.PlayerTime}s");
        _shopTimeText.text = string.Format($"{_player.PlayerTime}s");

        _coinShopUI.Find("Coin/Text").GetComponent<TextMeshProUGUI>().text = _player.Gold.ToString();

        _monsterCntText.text = string.Format($"{spawnMonster.monsters.Count} / {spawnMonster.maxSpawnCount}");
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.black; 
        
        GUI.Label(new Rect(1400, 0, 200, 10), "[ E ] Ű�� ���� ��������", labelStyle);
        GUI.Label(new Rect(1400, 50, 200, 10), "1: �ð����� ��ų���", labelStyle);
        GUI.Label(new Rect(1400, 100, 200, 10), "2: ���� ��ų���", labelStyle);
        GUI.Label(new Rect(1400, 150, 200, 10), "3: �ð����� ��ų���", labelStyle);
    }

    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    /// <param name="shopTrm">�� ���� ĵ������ Transform</param>
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

            _timeExplainText.text = "";

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (GameManager.Instance.gameType == GameManager.GameType.Ing) CloseTimeShop();
            else
            {
                SkillStatusUpdate?.Invoke();
                GameManager.Instance.NextStage();
            }
        }
    }

    /// <summary>
    /// �ð� ������ �ݾ��� ��
    /// </summary>
    public void CloseTimeShop()
    {
        if (_shopButton.deathCnt > 0)
        {
            while (_shopButton.deathCnt != 0)
            {
                if (spawnMonster.monsters.Count > 0)
                {
                    int cnt = UnityEngine.Random.Range(0, spawnMonster.monsters.Count);
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

    public IEnumerator Boost()
    {
        yield return new WaitForSeconds(3f);
        pMove.moveSpd -= 5;
    }

    /// <summary>
    /// ���� ���� ���� �ȳ� �޼��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="message"></param>
    public void ShopExplainMessage(string message)
    {
        if (GameManager.Instance.gameType == GameManager.GameType.Ing)
            _timeExplainText.text = message;
        else
            _coinExplainText.text = message;
    }

    /// <summary>
    /// ��ų ��� ���� �ȳ� �޼��� ����ִ� �Լ�
    /// </summary>
    /// <param name="message"></param>
    public void SkillExplainMessage(string message)
    {
        _skillExplainText.transform.GetComponentInChildren<Text>().text = message;
        Sequence seq = DOTween.Sequence();
        seq.Append(_skillExplainText.transform.DOScale(Vector3.one, .2f));
        seq.AppendInterval(0.5f);
        seq.Append(_skillExplainText.transform.DOScale(Vector3.zero, .2f));
    }

    /// <summary>
    /// �ð� ���� ��ų �̹��� ���� ������Ʈ
    /// </summary>
    public void SlowlyStatusUpdate()
    {
        //���� ��ų�� ��Ÿ�����̶��
        if (GameManager.Instance.player.skill.slowlyTimeIng == true)
        {
            //��������� ����
            _slowly.DOColor(Color.yellow, .5f);
        }
        //���� ��ų�� �����ϰ� ���� ������
        else if(GameManager.Instance.player.slowlyTimeCnt == 0)
        {
            //���������� ����
            _slowly.DOColor(Color.red, .5f);
        }
        //��ų ����� �����ϴٸ�
        else
        {
            //�Ͼ������ ����
            _slowly.DOColor(Color.white, .5f);
        }
    }

    /// <summary>
    /// ���� ��ų �̹��� ���� ������Ʈ
    /// </summary>
    public void PowerUpStatusUpdate()
    {
        //���� ��ų�� ��Ÿ�����̶��
        if (GameManager.Instance.player.skill.powerUpIng == true)
        {
            //��������� ����
            _powerUP.DOColor(Color.yellow, .5f);
        }
        //���� ��ų�� �����ϰ� ���� ������
        else if (GameManager.Instance.player.powerUpCnt == 0)
        {
            //���������� ����
            _powerUP.DOColor(Color.red, .5f);
        }
        //��ų ����� �����ϴٸ�
        else
        {
            //�Ͼ������ ����
            _powerUP.DOColor(Color.white, .5f);
        }
    }

    /// <summary>
    /// �ð� ���� ��ų �̹��� ���� ������Ʈ
    /// </summary>
    public void TimeStopStatusUpdate()
    {
        //���� ��ų�� ��Ÿ�����̶��
        if (GameManager.Instance.player.skill.timeStopIng == true)
        {
            //��������� ����
            _timeStop.DOColor(Color.yellow, .5f);
        }
        //���� ��ų�� �����ϰ� ���� ������
        else if (GameManager.Instance.player.timeStopCnt == 0)
        {
            //���������� ����
            _timeStop.DOColor(Color.red, .5f);
        }
        //��ų ����� �����ϴٸ�
        else
        {
            //�Ͼ������ ����
            _timeStop.DOColor(Color.white, .5f);
        }
    }
    
    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
