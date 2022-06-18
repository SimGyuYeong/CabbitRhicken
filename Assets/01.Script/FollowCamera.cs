using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("카메라 기본속성")]
    [SerializeField] private Transform _targetObj = null;
    private float _followSpd = 10f;
    private float _sensitivity = 100f;
    private float _clampAngle = 70f;

    private float _rotX;
    private float _rotY;

    [SerializeField] private Transform _realCam;
    private Vector3 _dirNormalized;
    private Vector3 _finalDir;
    private float _minDistance =1;
    private float _maxDistance = 2;
    private float _finalDistance;

    private float _smoothness = 10f;

    private void Awake()
    {
        _rotX = transform.localRotation.eulerAngles.x;
        _rotY = transform.localRotation.eulerAngles.y;

        _dirNormalized = _realCam.localPosition.normalized;
        _finalDistance = _realCam.localPosition.magnitude;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _realCam.LookAt(_targetObj);
    }

    private void LateUpdate()
    {
        _rotX += -(Input.GetAxis("Mouse Y")) * _sensitivity * Time.deltaTime;
        _rotY += Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;

        _rotX = Mathf.Clamp(_rotX, -_clampAngle, _clampAngle);
        Quaternion _rot = Quaternion.Euler(_rotX, _rotY, 0);
        transform.rotation = _rot;

        transform.position = Vector3.MoveTowards(transform.position, _targetObj.position, _followSpd * Time.deltaTime);

        _finalDir = transform.TransformPoint(_dirNormalized * _maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, _finalDir, out hit))
        {
            _finalDistance = Mathf.Clamp(hit.distance, _minDistance, _maxDistance);
        }
        else
        {
            _finalDistance = _maxDistance;
        }

        _realCam.localPosition = Vector3.Lerp(_realCam.localPosition, _dirNormalized * _finalDistance, Time.deltaTime * _smoothness);
        _realCam.LookAt(_targetObj);
    }

}
