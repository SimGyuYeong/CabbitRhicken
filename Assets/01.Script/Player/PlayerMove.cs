using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("속성")]

    public float moveSpd = 2f;
    public float runMoveSpd = 3.5f;
    public float walkMoveSpd = 2f;

    public float directRotateSpd = 100f;
    public float bodyRotateSpd = 2f;
    public float VelocityChangSpd = 0.1f;

    private bool isFight = false;

    private Vector3 currentVelocitySpd = Vector3.zero;

    private Vector3 moveDirect = Vector3.zero;

    private Vector3 _jumpVelocity;

    private Vector3 _amount = Vector3.zero;

    //중력값
    private float _gravity = 0.8f;

    //현재 캐릭터 속도
    private float _verticalSpd = 0f;

    public CollisionFlags collisionFlags = CollisionFlags.None;

    public CharacterController characterController = null;
    public Animator animator = null;
    public Player pScript = null;
    public PlayerCollider pCollider = null;

    private bool _isRun = false;

    public enum PlayerState
    {
        None, Idle, Walk, Change, Run, Atk
    }
    public PlayerState playerState = PlayerState.None;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        pScript = GetComponent<Player>();
        pCollider = transform.Find("AttackCollider").GetComponent<PlayerCollider>();
    }

    private void Start()
    {
        playerState = PlayerState.Idle;
    }

    private void Update()
    {
        Move();
        CheckPlayerState();
        BodyDirectChange();
        SetGravity();
        InputAttack();
    }

    void Move()
    {
        if (characterController.isGrounded && _jumpVelocity.y < 0)
        {
            _jumpVelocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpd += 3;
            _isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpd -= 3;
            _isRun = false;
        }

        //백터 내적
        Transform cameraTransform = Camera.main.transform;
        //메인카메라가 바라보는 방향이 월드상에서 어떤 방향인가
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0f;

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 right = new Vector3(forward.z, 0f, -forward.x);
        _amount = (forward * vertical + right * horizontal) * Time.deltaTime;
        _amount *= moveSpd;

        collisionFlags = characterController.Move(_amount);

        //캐릭터 이동 속도
        float spd = moveSpd;

        if (Input.GetButtonDown("Jump"))
        {
            _jumpVelocity.y += Mathf.Sqrt(3f * -3.0f * -9.81f);
        }
        _jumpVelocity.y += _verticalSpd;
        characterController.Move(_jumpVelocity * Time.deltaTime);

        collisionFlags = characterController.Move(moveDirect * Time.deltaTime * spd);
    }

    private void CheckPlayerState()
    {

        switch (playerState)
        {
            case PlayerState.Idle:
                if (moveSpd > 0.0f)
                {
                    playerState = PlayerState.Walk;
                    animator.SetBool("Walk", true);
                }
                break;
            case PlayerState.Walk:
                if (moveSpd < 0.01f)
                {
                    playerState = PlayerState.Idle;
                    animator.SetBool("Walk", false);
                }
                else if (_isRun ==  true)
                {
                    playerState = PlayerState.Run;
                    animator.SetBool("Walk", false);
                    animator.SetBool("Run", true);
                    break;
                }
                break;
            case PlayerState.Run:
                if (_isRun == false)
                {
                    playerState = PlayerState.Walk;
                    animator.SetBool("Walk", true);
                    animator.SetBool("Run", false);
                }
                break;
            default:
                break;
        }
    }

    void InputAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (playerState != PlayerState.Atk && isFight == false)
            {
                isFight = true;
                StartCoroutine(Fighting());
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                playerState = PlayerState.Atk;
                animator.SetBool("Eat", true);
                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (playerState == PlayerState.Atk)
            {
                isFight = false;
                playerState = PlayerState.Idle;
                animator.SetBool("Eat", false);
            }
        }
    }

    public IEnumerator Fighting()
    {
        while(isFight)
        {
            if(pCollider.target != null)
            {
                pCollider.target.Damaged();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void BodyDirectChange()
    {
        Vector3 newForward = characterController.velocity;
        newForward.y = 0;

        transform.forward = Vector3.Lerp(transform.forward, newForward, bodyRotateSpd * Time.deltaTime);

        _amount.y = 0f;
        if (_amount != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_amount), bodyRotateSpd * Time.deltaTime);
    }

    public float GetVelocitySpd()
    {
        if (characterController.velocity == Vector3.zero)
        {
            currentVelocitySpd = Vector3.zero;
        }
        else
        {
            Vector3 retVelocity = characterController.velocity;
            retVelocity.y = 0f;
            currentVelocitySpd = Vector3.Lerp(currentVelocitySpd, retVelocity, VelocityChangSpd * Time.deltaTime);
        }

        return currentVelocitySpd.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Monster"))
        {
            pScript.SendMessage("DamagedMonster");
        }
    }

    void SetGravity()
    {
        if ((collisionFlags & CollisionFlags.CollidedBelow) != 0)
        {
            _verticalSpd = 0f;
        }
        else
        {
            _verticalSpd -= _gravity * Time.deltaTime;
        }
    }
}
