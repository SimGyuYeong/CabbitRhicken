using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("카메라 기본속성")]
    //카메라 위치 캐싱 준비
    private Transform cameraTransform = null;

    //플레이어 타겟 게임 오브젝트 캐싱 준비
    public GameObject objTarget = null;

    //플레이어 타겟 위치 캐싱 준비
    private Transform objTargetTransform = null;

    [Header("3인칭 카메라")]
    //현재 카메라 위치까지 타겟으로부터 뒤로 떨어진 거리
    public float distance = 6.0f;
    //현재 카메라 위치까지 타겟의 위치보다 더 추가적인 높이
    public float height = 1.75f;

    //Damp란 카메라가 몇 초뒤에 따라갈지에 대한 값이다.
    //실무에선 Damp값 설정에 대해 오랜 시간을 투자한다.

    //카메라 높이에 대한 Damp 값 
    public float heightDamping = 2.0f;
    //카메라 y축 회전에대한 Damp 값
    public float rotationDamping = 3.0f;

    // Start is called before the first frame update
    void Start()
    {

        //카메라 위치 캐싱
        cameraTransform = GetComponent<Transform>();

        //플레이어 목표 오브젝트가 존재 하다면.
        if (objTarget != null)
        {
            //플레이어 목표 오브젝트 위치 캐싱
            objTargetTransform = objTarget.transform;
        }
    }

    /// <summary>
    /// 3인칭 카메라 함수
    /// </summary>
    void ThirdCamera()
    {
        //현재 타겟의 y축 각도 값
        float objTargetRotationAngle = objTargetTransform.eulerAngles.y;
        //현재 타겟의 높이  + 카메라 위치까지의 추가 높이
        float objHeight = objTargetTransform.position.y + height;
        //현재 카메라의 y축 각도 값을 오일러 각으로 계산
        float nowRotationAngle = cameraTransform.eulerAngles.y;
        //현재 카메라의 높이 값
        float nowHeight = cameraTransform.position.y;

        //현재 각도에서 원하는 각도로 Damp 값 변경
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);

        //현재 높이에서 원하는 높이로 Damp 값 변경
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamping * Time.deltaTime);

        //유니티 각도인 퀀터니언으로 오일러 각 변경
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);

        //이제는 카메라를 회전 시키자.

        //카메라 위치를 플레이어 목적 포지션으로 이동
        cameraTransform.position = objTargetTransform.position;

        //플레이어 목표가 보는 방향으로 뒤쪽으로 쭉 뺀다. 
        // -1 * nowRotation * Vector3.forward(앞쪽 방향) * 거리
        cameraTransform.position -= nowRotation * Vector3.forward * distance;

        //카메라 최종 높이 값은 타겟에 위치 x에서 바라보는 반대 z방향 만큼 이동해서 원하는 
        //높이를 올림.
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z);

        //마지막은 바라봐줘
        cameraTransform.LookAt(objTargetTransform);
    }

    private void LateUpdate()
    {
        //목표 플레이어 게임오브젝트가 존재한가? 없으면 함수 실행 안함.
        if (objTarget == null)
        {
            return;
        }

        //목표 플레이어 위치정보 값이 없다면. 해당 게임오브젝트 위치 정보값 가져온다.
        if (objTargetTransform == null)
        {
            objTargetTransform = objTarget.transform;
        }

        ThirdCamera();
        RotateCamera();
    }

    private void RotateCamera()
    {
        //마우스 x,y 축 값 가져오기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX;
        float rotationY;

        //카메라의 y각도에 마우스(마우스 * 디테일)값만큼 움직인다. 
        //마우스를 움직이지 않았다면 0이다.
        rotationX = objTargetTransform.localEulerAngles.y + mouseX * 0.5f;

        //마이너스 각도를 조절하기 위해 각도를 조절해준다.
        //각도 조절을 안해주면 마이너스로 각도가 바뀌는 순간 튀는 것을 확인 할 수 있다.
        rotationX = (rotationX > 180.0f) ? rotationX - 360.0f : rotationX;

        //현재 y값에 마우스가 움직인 값(마우스 + 디테일)만큼 더해준다.
        rotationY = mouseY * 0.5f;
        //역시 마이너스 각도 조절을 하기 위해 
        rotationY = (rotationY > 180.0f) ? rotationY - 360.0f : rotationY;

        //마우스의 x,y축이 실제 x,y축과 반대여서 반대로 Vector를 만들어 준다.
        objTargetTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
    }
}
