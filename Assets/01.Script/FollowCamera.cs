using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("ī�޶� �⺻�Ӽ�")]
    //ī�޶� ��ġ ĳ�� �غ�
    private Transform cameraTransform = null;

    //�÷��̾� Ÿ�� ���� ������Ʈ ĳ�� �غ�
    public GameObject objTarget = null;

    //�÷��̾� Ÿ�� ��ġ ĳ�� �غ�
    private Transform objTargetTransform = null;

    [Header("3��Ī ī�޶�")]
    //���� ī�޶� ��ġ���� Ÿ�����κ��� �ڷ� ������ �Ÿ�
    public float distance = 6.0f;
    //���� ī�޶� ��ġ���� Ÿ���� ��ġ���� �� �߰����� ����
    public float height = 1.75f;

    //Damp�� ī�޶� �� �ʵڿ� �������� ���� ���̴�.
    //�ǹ����� Damp�� ������ ���� ���� �ð��� �����Ѵ�.

    //ī�޶� ���̿� ���� Damp �� 
    public float heightDamping = 2.0f;
    //ī�޶� y�� ȸ�������� Damp ��
    public float rotationDamping = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //ī�޶� ��ġ ĳ��
        cameraTransform = GetComponent<Transform>();

        //�÷��̾� ��ǥ ������Ʈ�� ���� �ϴٸ�.
        if (objTarget != null)
        {
            //�÷��̾� ��ǥ ������Ʈ ��ġ ĳ��
            objTargetTransform = objTarget.transform;
        }
    }

    /// <summary>
    /// 3��Ī ī�޶� �Լ�
    /// </summary>
    void ThirdCamera()
    {
        //���� Ÿ���� y�� ���� ��
        float objTargetRotationAngle = objTargetTransform.eulerAngles.y;
        //���� Ÿ���� ����  + ī�޶� ��ġ������ �߰� ����
        float objHeight = objTargetTransform.position.y + height;
        //���� ī�޶��� y�� ���� ���� ���Ϸ� ������ ���
        float nowRotationAngle = cameraTransform.eulerAngles.y;
        //���� ī�޶��� ���� ��
        float nowHeight = cameraTransform.position.y;

        //���� �������� ���ϴ� ������ Damp �� ����
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);

        //���� ���̿��� ���ϴ� ���̷� Damp �� ����
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamping * Time.deltaTime);

        //����Ƽ ������ ���ʹϾ����� ���Ϸ� �� ����
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);

        //������ ī�޶� ȸ�� ��Ű��.

        //ī�޶� ��ġ�� �÷��̾� ���� ���������� �̵�
        cameraTransform.position = objTargetTransform.position;

        //�÷��̾� ��ǥ�� ���� �������� �������� �� ����. 
        // -1 * nowRotation * Vector3.forward(���� ����) * �Ÿ�
        cameraTransform.position -= nowRotation * Vector3.forward * distance;

        //ī�޶� ���� ���� ���� Ÿ�ٿ� ��ġ x���� �ٶ󺸴� �ݴ� z���� ��ŭ �̵��ؼ� ���ϴ� 
        //���̸� �ø�.
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z);

        //�������� �ٶ����
        cameraTransform.LookAt(objTargetTransform);
    }

    private void LateUpdate()
    {
        //��ǥ �÷��̾� ���ӿ�����Ʈ�� �����Ѱ�? ������ �Լ� ���� ����.
        if (objTarget == null)
        {
            return;
        }

        //��ǥ �÷��̾� ��ġ���� ���� ���ٸ�. �ش� ���ӿ�����Ʈ ��ġ ������ �����´�.
        if (objTargetTransform == null)
        {
            objTargetTransform = objTarget.transform;
        }

        ThirdCamera();
        RotateCamera();
    }

    private void RotateCamera()
    {
        //���콺 x,y �� �� ��������
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX;
        float rotationY;

        //ī�޶��� y������ ���콺(���콺 * ������)����ŭ �����δ�. 
        //���콺�� �������� �ʾҴٸ� 0�̴�.
        rotationX = objTargetTransform.localEulerAngles.y + mouseX * 0.5f;

        //���̳ʽ� ������ �����ϱ� ���� ������ �������ش�.
        //���� ������ �����ָ� ���̳ʽ��� ������ �ٲ�� ���� Ƣ�� ���� Ȯ�� �� �� �ִ�.
        rotationX = (rotationX > 180.0f) ? rotationX - 360.0f : rotationX;

        //���� y���� ���콺�� ������ ��(���콺 + ������)��ŭ �����ش�.
        rotationY = mouseY * 0.5f;
        //���� ���̳ʽ� ���� ������ �ϱ� ���� 
        rotationY = (rotationY > 180.0f) ? rotationY - 360.0f : rotationY;

        //���콺�� x,y���� ���� x,y��� �ݴ뿩�� �ݴ�� Vector�� ����� �ش�.
        objTargetTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
    }
}
