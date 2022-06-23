using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class RabbitCtrl : Monster
{
    [Header("�⺻ �Ӽ�")]
    
    public float spdMove = 1f;
    public GameObject targetCharactor = null;
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;

    public GameObject ckTargetObj = null;

    private Animator _animator = null;
    private Transform _trm = null;
    private BoxCollider _collider = null;

    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;

    public enum RabbitState { Idle, Move, Wait, GoTarget, Atk, Damage, Die }
    public RabbitState rabbitState = RabbitState.Idle;

    void Start()
    {
        //�ִϸ���, Ʈ������ ������Ʈ ĳ�� : �������� ã�� ������ �ʰ�
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();
        _trm = GetComponent<Transform>();

        ckTargetObj = FindObjectOfType<Player>().gameObject;
        skinnedMeshRenderer = transform.Find("RabbitSkin").GetComponent<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// �ذ� ���¿� ���� ������ �����ϴ� �Լ� 
    /// </summary>
    void CkState()
    {
        switch (rabbitState)
        {
            case RabbitState.Idle:
                //�̵��� ���õ� RayCast��
                SetIdle();
                break;
            case RabbitState.GoTarget:
            case RabbitState.Move:
                SetMove();
                break;
            case RabbitState.Atk:
                SetAtk();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CkState();
    }

    /// <summary>
    /// �ذ� ���°� ��� �� �� ���� 
    /// </summary>
    void SetIdle()
    {
        if (targetCharactor == null)
        {
            SetPosTarget();
            StartCoroutine(RefreshPosTarget());
            rabbitState = RabbitState.Move;
        }
        else
        {
            rabbitState = RabbitState.GoTarget;
        }
    }

    private void SetPosTarget()
    {
        posTarget = new Vector3(_trm.position.x + Random.Range(-10f, 10f),
                                    _trm.position.y + 1000f,
                                    _trm.position.z + Random.Range(-10f, 10f)
                );

        Ray ray = new Ray(posTarget, Vector3.down);
        RaycastHit infoRayCast = new RaycastHit();
        if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
        {
            posTarget.y = infoRayCast.point.y;
        }
    }

    private IEnumerator RefreshPosTarget()
    {
        yield return new WaitForSeconds(10f);
        SetIdle();
    }

    /// <summary>
    /// �ذ� ���°� �̵� �� �� �� 
    /// </summary>
    void SetMove()
    {
        _animator.SetBool("Walk", true);
        //����� ������ �� ������ ���� 
        Vector3 distance = Vector3.zero;
        //��� ������ �ٶ󺸰� ���� �ִ��� 
        Vector3 posLookAt = Vector3.zero;

        //�ذ� ����
        switch (rabbitState)
        {
            //�ذ��� ���ƴٴϴ� ���
            case RabbitState.Move:
                //���� ���� ��ġ ���� ���ΰ� �ƴϸ�
                if (posTarget != Vector3.zero)
                {

                    distance = posTarget - _trm.position;
                    if (Vector3.Distance(_trm.position, ckTargetObj.transform.position) <= attackRange)
                    {
                        OnCkTarget(ckTargetObj);
                        StartCoroutine(SetWait());
                    }

                    if (distance.magnitude < 1) StartCoroutine(SetWait());

                    //��� ������ �ٶ� �� ����. ���� ����
                    posLookAt = new Vector3(posTarget.x,
                                            //Ÿ���� ���� ���� ��찡 ������ y�� üũ
                                            _trm.position.y,
                                            posTarget.z);
                }
                break;

            //ĳ���͸� ���ؼ� ���� ���ƴٴϴ�  ���
            case RabbitState.GoTarget:
                //��ǥ ĳ���Ͱ� ���� ��
                if (targetCharactor != null)
                {
                    //��ǥ ��ġ���� �ذ� �ִ� ��ġ ���� ���ϰ�
                    distance = targetCharactor.transform.position - _trm.position;
                    //���࿡ �����̴� ���� �ذ��� ��ǥ�� �� ���� ���� ���� 
                    if (distance.magnitude < attackRange)
                    {
                        //���ݻ��·� �����մ�.
                        rabbitState = RabbitState.Atk;
                        //���⼭ ����
                        return;
                    }
                    //��� ������ �ٶ� �� ����. ���� ����
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            //Ÿ���� ���� ���� ��찡 ������ y�� üũ
                                            _trm.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }

        //�ذ� �̵��� ���⿡ ũ�⸦ ���ְ� ���⸸ ����(normalized)
        Vector3 direction = distance.normalized;

        //������ x,z ��� y�� ���� �İ� ���Ŷ� ����
        direction = new Vector3(direction.x, 0f, direction.z);

        //�̵��� ���� ���ϱ�
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //ĳ���� ��Ʈ���� �ƴ� Ʈ���������� ���� ��ǥ �̿��Ͽ� �̵�
        _trm.Translate(amount);
        //ĳ���� ���� ���ϱ�
        _trm.LookAt(posLookAt);
    }

    /// <summary>
    /// ��� ���� ���� �� 
    /// </summary>
    /// <returns></returns>
    IEnumerator SetWait()
    {
        rabbitState = RabbitState.Wait;
        float timeWait = Random.Range(1f, 3f);

        yield return new WaitForSeconds(timeWait);
        rabbitState = RabbitState.Idle;
    }

    ///<summary>
    ///�þ� ���� �ȿ� �ٸ� Trigger �Ǵ� ĳ���Ͱ� ������ ȣ�� �ȴ�.
    ///�Լ� ������ ��ǥ���� ������ ��ǥ���� �����ϰ� �ذ��� Ÿ�� ��ġ�� �̵� ��Ų�� 
    ///</summary>

    void OnCkTarget(GameObject target)
    {
        //��ǥ ĳ���Ϳ� �Ķ���ͷ� ����� ������Ʈ�� �ְ� 
        targetCharactor = target;
        //��ǥ ��ġ�� ��ǥ ĳ������ ��ġ ���� �ֽ��ϴ�. 
        targetTransform = targetCharactor.transform;

        //��ǥ���� ���� �ذ��� �̵��ϴ� ���·� ����
        rabbitState = RabbitState.GoTarget;

    }

    /// <summary>
    /// �ذ� ���� ���� ���
    /// </summary>
    void SetAtk()
    {
        //�ذ�� ĳ���Ͱ��� ��ġ �Ÿ� 
        float distance = Vector3.Distance(targetTransform.position, _trm.position); //���̴�

        Attack();

        //���� �Ÿ����� �� ���� �Ÿ��� �־� ���ٸ� 
        if (distance > attackRange + 0.5f)
        {
            //Ÿ�ٰ��� �Ÿ��� �־����ٸ� Ÿ������ �̵� 
            rabbitState = RabbitState.Move;
            targetCharactor = null;
            targetTransform = null;
            _animator.SetBool("Walk", true);
        }
    }

    /// <summary>
    /// ���� ���Ԥ�
    /// </summary>
    public void Damaged()
    {
        if (rabbitState == RabbitState.Die) return;
        effectDamageTween();
        hp -= 10;
        if (hp <= 0)
        {
            _animator.SetTrigger("Dead");
            rabbitState = RabbitState.Die;
        }
    }

    /// <summary>
    /// �ǰݽ� ���� ������ ��½��½ ȿ���� �ش�
    /// </summary>
    void effectDamageTween()
    {
        //Ʈ���� ������ �� Ʈ�� �Լ��� ����Ǹ� ������ ������ �� �� �־ 
        //Ʈ�� �ߺ� üũ�� �̸� ������ ���ش�
        if (effectTweener != null && effectTweener.isComplete == false)
        {
            return;
        }

        //��½�̴� ����Ʈ ������ �������ش�
        Color colorTo = Color.red;

        //Ʈ���� Ÿ���� ��Ų�Ž�������, �ð��� 0.2��, �Ķ���ͷδ� ���� , �ݺ�. �ݹ��Լ�
        effectTweener = HOTween.To(skinnedMeshRenderer, 0.2f, new TweenParms()
                                //������ ��ü
                                .Prop("material.color", colorTo)
                                // �ݺ��� 1���� ��並 �Ⱦ��� ������ 1ȸ ��� 1ȸ �ؾ� �Ѵ�. 
                                .Loops(1, LoopType.Yoyo)
                                //�ǰ� ����Ʈ ����� �̺�Ʈ �Լ� ȣ��
                                .OnStepComplete(OnDamageTweenFinished));
        
    }

    /// <summary>
    /// �ǰ�����Ʈ ����� �̺�Ʈ �Լ� ȣ��
    /// </summary>
    void OnDamageTweenFinished()
    {
        //Ʈ���� ������ �Ͼ������ Ȯ���� ������ �����ش�
        skinnedMeshRenderer.material.color = Color.white;
    }
}
