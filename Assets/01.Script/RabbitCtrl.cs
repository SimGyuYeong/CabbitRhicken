using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCtrl : MonoBehaviour
{

    [Header("기본 속성")]
    
    public float spdMove = 1f;
    public GameObject targetCharactor = null;
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;

    private Animation _animation = null;
    private Transform _trm = null;

    public enum RabbitState { Idle, Move, Wait, GoTarget, Atk, Damage, Die }
    public RabbitState rabbitState = RabbitState.Idle;

    [Header("전투속성")]
    public int hp = 100;
    public float AtkRange = 1.5f;

    void Start()
    {
        //애니메이, 트랜스폼 컴포넌트 캐싱 : 쓸때마다 찾아 만들지 않게
        _animation = GetComponent<Animation>();
        _trm = GetComponent<Transform>();

    }

    /// <summary>
    /// 해골 상태에 따라 동작을 제어하는 함수 
    /// </summary>
    void CkState()
    {
        switch (rabbitState)
        {
            case RabbitState.Idle:
                //이동에 관련된 RayCast값
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
    /// 해골 상태가 대기 일 때 동작 
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
    /// 해골 상태가 이동 일 때 동 
    /// </summary>
    void SetMove()
    {
        //출발점 도착점 두 벡터의 차이 
        Vector3 distance = Vector3.zero;
        //어느 방향을 바라보고 가고 있느냐 
        Vector3 posLookAt = Vector3.zero;

        //해골 상태
        switch (rabbitState)
        {
            //해골이 돌아다니는 경우
            case RabbitState.Move:
                //만약 랜덤 위치 값이 제로가 아니면
                if (posTarget != Vector3.zero)
                {
                    //목표 위치에서 해골 있는 위치 차를 구하고
                    distance = posTarget - _trm.position;

                    //만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
                    if (distance.magnitude < AtkRange)
                    {
                        //대기 동작 함수를 호출
                        StartCoroutine(SetWait());
                        //여기서 끝냄
                        return;
                    }

                    //어느 방향을 바라 볼 것인. 랜덤 지역
                    posLookAt = new Vector3(posTarget.x,
                                            //타겟이 높이 있을 경우가 있으니 y값 체크
                                            _trm.position.y,
                                            posTarget.z);
                }
                break;
            //캐릭터를 향해서 가는 돌아다니는  경우
            case RabbitState.GoTarget:
                //목표 캐릭터가 있을 땟
                if (targetCharactor != null)
                {
                    //목표 위치에서 해골 있는 위치 차를 구하고
                    distance = targetCharactor.transform.position - _trm.position;
                    //만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
                    if (distance.magnitude < AtkRange)
                    {
                        //공격상태로 변경합니.
                        rabbitState = RabbitState.Atk;
                        //여기서 끝냄
                        return;
                    }
                    //어느 방향을 바라 볼 것인. 랜덤 지역
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            //타겟이 높이 있을 경우가 있으니 y값 체크
                                            _trm.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }

        //해골 이동할 방향에 크기를 없애고 방향만 가진(normalized)
        Vector3 direction = distance.normalized;

        //방향은 x,z 사용 y는 땅을 파고 들어갈거라 안함
        direction = new Vector3(direction.x, 0f, direction.z);

        //이동량 방향 구하기
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //캐릭터 컨트롤이 아닌 트랜스폼으로 월드 좌표 이용하여 이동
        _trm.Translate(amount, Space.World);
        //캐릭터 방향 정하기
        _trm.LookAt(posLookAt);
    }

    /// <summary>
    /// 대기 상태 동작 함 
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
    ///시야 범위 안에 다른 Trigger 또는 캐릭터가 들어오면 호출 된다.
    ///함수 동작은 목표물이 들어오면 목표물을 설정하고 해골을 타겟 위치로 이동 시킨다 
    ///</summary>

    void OnCkTarget(GameObject target)
    {
        //목표 캐릭터에 파라메터로 검출된 오브젝트를 넣고 
        targetCharactor = target;
        //목표 위치에 목표 캐릭터의 위치 값을 넣습니다. 
        targetTransform = targetCharactor.transform;

        //목표물을 향해 해골이 이동하는 상태로 변경
        rabbitState = RabbitState.GoTarget;

    }

    /// <summary>
    /// 해골 상태 공격 모드
    /// </summary>
    void SetAtk()
    {
        //해골과 캐릭터간의 위치 거리 
        float distance = Vector3.Distance(targetTransform.position, _trm.position); //무겁다

        //공격 거리보다 둘 간의 거리가 멀어 졌다면 
        if (distance > AtkRange + 0.5f)
        {
            //타겟과의 거리가 멀어졌다면 타겟으로 이동 
            rabbitState = RabbitState.GoTarget;
        }
    }


    /// <summary>
    /// 피격 충돌
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
