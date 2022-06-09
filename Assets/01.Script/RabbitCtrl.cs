using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCtrl : MonoBehaviour
{

    [Header("�⺻ �Ӽ�")]
    
    public float spdMove = 1f;
    public GameObject targetCharactor = null;
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;

    private Animation _animation = null;
    private Transform _trm = null;

    public enum RabbitState { Idle, Move, Wait, GoTarget, Atk, Damage, Die }
    public RabbitState rabbitState = RabbitState.Idle;

    [Header("�����Ӽ�")]
    public int hp = 100;
    public float AtkRange = 1.5f;

    void Start()
    {
        //�ִϸ���, Ʈ������ ������Ʈ ĳ�� : �������� ã�� ������ �ʰ�
        _animation = GetComponent<Animation>();
        _trm = GetComponent<Transform>();

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
            rabbitState = RabbitState.Move;
        }
        else
        {
            rabbitState = RabbitState.GoTarget;
        }
    }

    /// <summary>
    /// �ذ� ���°� �̵� �� �� �� 
    /// </summary>
    void SetMove()
    {
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
                    //��ǥ ��ġ���� �ذ� �ִ� ��ġ ���� ���ϰ�
                    distance = posTarget - _trm.position;

                    //���࿡ �����̴� ���� �ذ��� ��ǥ�� �� ���� ���� ���� 
                    if (distance.magnitude < AtkRange)
                    {
                        //��� ���� �Լ��� ȣ��
                        StartCoroutine(SetWait());
                        //���⼭ ����
                        return;
                    }

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
                    if (distance.magnitude < AtkRange)
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
        _trm.Translate(amount, Space.World);
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

        //���� �Ÿ����� �� ���� �Ÿ��� �־� ���ٸ� 
        if (distance > AtkRange + 0.5f)
        {
            //Ÿ�ٰ��� �Ÿ��� �־����ٸ� Ÿ������ �̵� 
            rabbitState = RabbitState.GoTarget;
        }
    }


    /// <summary>
    /// �ǰ� �浹
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            hp -= 10;
            if (hp <= 0)
            {
                rabbitState = RabbitState.Die;
            }
        }
    }

}
