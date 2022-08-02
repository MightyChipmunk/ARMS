using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Forward,
        Backward,
        Right,
        FrontRight,
        BackRight,
        Left,
        FrontLeft,
        BackLeft,
        Jump,
        DashForward,
        DashBackward,
        DashRight,
        DashFrontRight,
        DashBackRight,
        DashLeft,
        DashFrontLeft,
        DashBackLeft,
    }

    Vector3 dir;
    Vector3 moveDir = Vector3.zero;
    GameObject target;
    CharacterController cc;
    JH_CameraMove cm;
    TrailRenderer tr;
    PlayerState state;
    public PlayerState State
    {
        get { return state; }
        set
        {
            state = value;

            Animator anim = GetComponent<Animator>();
            switch (state)
            {
                case PlayerState.Idle:
                    anim.SetInteger("StateNum", 0);
                    break;
                case PlayerState.Forward:
                    anim.SetInteger("StateNum", 3);
                    break;
                case PlayerState.FrontRight:
                    anim.SetInteger("StateNum", 4);
                    break;
                case PlayerState.Right:
                    anim.SetInteger("StateNum", 4);
                    break;
                case PlayerState.BackRight:
                    anim.SetInteger("StateNum", 4);
                    break;
                case PlayerState.FrontLeft:
                    anim.SetInteger("StateNum", 1);
                    break;
                case PlayerState.Left:
                    anim.SetInteger("StateNum", 1);
                    break;
                case PlayerState.BackLeft:
                    anim.SetInteger("StateNum", 1);
                    break;
                case PlayerState.Backward:
                    anim.SetInteger("StateNum", 2);
                    break;
                case PlayerState.Jump:
                    anim.SetInteger("StateNum", 5);
                    break;
                case PlayerState.DashForward:
                    break;
                case PlayerState.DashFrontRight:
                    break;
                case PlayerState.DashRight:
                    break;
                case PlayerState.DashBackRight:
                    break;
                case PlayerState.DashFrontLeft:
                    break;
                case PlayerState.DashLeft:
                    break;
                case PlayerState.DashBackLeft:
                    break;
                case PlayerState.DashBackward:
                    break;
            }
        }
    }
    float gravity = -9.81f;
    float yVelocity;
    float dashTime = 0.16f;
    float dashCool = 1f;
    bool canDash = true;
    bool isDash = false;
    bool changeAct = true;
    int ran = 0;

    [SerializeField]
    float speed = 2.0f;
    [SerializeField]
    public float jumpPower = 7f;
    [SerializeField]
    bool isEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        tr = transform.Find("DashTrail").GetComponent<TrailRenderer>();

        if (isEnemy)
        {
            target = GameObject.Find("Main Camera");
        }
        else
        {
            cm = transform.Find("Main Camera").GetComponent<JH_CameraMove>();
            target = GameObject.Find("Enemy Camera");
        }

        State = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }
        else
        {
            yVelocity = -1f;
        }

        if (!isEnemy)
        {
            Jump();
            Move();
            LookEnemy();
            Dash();
        }
        else
        {
            if (changeAct)
            {
                ran = Random.Range(1, 10);
                StartCoroutine("RandomAct");
            }

            Jump(ran);
            Move(ran);
            LookEnemy();
            Dash(ran);
        }


        SetPlayerState();
    }

    void LookEnemy()
    {
        dir = target.transform.position - transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Move()
    {
        if (cc.isGrounded)
            moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W) && Vector3.Magnitude(target.transform.position - transform.position) >= 5f)
        {
            moveDir += dir;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir -= dir;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += transform.right;
        }


        moveDir.Normalize();
        //moveDir.y = yVelocity;
        cc.Move(moveDir * speed * Time.deltaTime);
        cc.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void Move(int ran)
    {
        moveDir = Vector3.zero;
        if (ran <= 2 && Vector3.Magnitude(target.transform.position - transform.position) >= 10.0f)
        {
            moveDir += dir;
        }
        if (ran >= 3 && ran <= 4)
        {
            moveDir -= dir;
        }
        if (ran >= 4 && ran <= 6)
        {
            moveDir -= transform.right;
        }
        if (ran >= 6 && ran <= 8)
        {
            moveDir += transform.right;
        }

        moveDir.Normalize();
        //moveDir.y = yVelocity;
        cc.Move(moveDir * speed * Time.deltaTime);
        cc.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
        {
            yVelocity = jumpPower;
        }
    }

    void Jump(int ran)
    {
        if (ran > 8 && cc.isGrounded)
        {
            yVelocity = jumpPower;
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine("IncreaseSpeed");
        }
    }

    void Dash(int ran)
    {
        if (canDash && ran <= 3)
        {
            StartCoroutine("IncreaseSpeedEnemy");
        }
    }

    void SetPlayerState()
    {
        if (cc.isGrounded && !isDash)
        {
            // moveDir의 앵글을 계산해서 애니메이션 재생
            float angle = Vector3.Angle(moveDir, transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(moveDir, transform.right));
            float finalAngle = sign * angle;
            if (finalAngle >= -1f && finalAngle <= 1f)
                State = PlayerState.Forward;
            else if (finalAngle > 1f && finalAngle <= 89f)
                State = PlayerState.FrontRight;
            else if (finalAngle > 89 && finalAngle < 91)
                State = PlayerState.Right;
            else if (finalAngle > 91 && finalAngle < 179)
                State = PlayerState.BackRight;
            else if (finalAngle < -1f && finalAngle >= -89f)
                State = PlayerState.FrontLeft;
            else if (finalAngle < -89 && finalAngle > -91)
                State = PlayerState.Left;
            else if (finalAngle < -91 && finalAngle > -179)
                State = PlayerState.BackLeft;
            else if (finalAngle >= 179 || finalAngle <= -179)
                State = PlayerState.Backward;

            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                  Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space))
                && !isEnemy)
                State = PlayerState.Idle;
            else if (ran == 9 && isEnemy)
                State = PlayerState.Idle;
        }
        else if (!cc.isGrounded && !isDash)
        {
            // 공중에 있다면 점프 모션 재생
            State = PlayerState.Jump;
        }
        else if (isDash)
        {
            // 대쉬 중이라면 공중이라도 대쉬 모션 재생
            float angle = Vector3.Angle(moveDir, transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(moveDir, transform.right));
            float finalAngle = sign * angle;
            if (finalAngle >= -1f && finalAngle <= 1f)
                State = PlayerState.DashForward;
            else if (finalAngle > 1f && finalAngle <= 89f)
                State = PlayerState.DashFrontRight;
            else if (finalAngle > 89 && finalAngle < 91)
                State = PlayerState.DashRight;
            else if (finalAngle > 91 && finalAngle < 180)
                State = PlayerState.DashBackRight;
            else if (finalAngle < -1f && finalAngle >= -89f)
                State = PlayerState.DashFrontLeft;
            else if (finalAngle < -89 && finalAngle > -91)
                State = PlayerState.DashLeft;
            else if (finalAngle < -91 && finalAngle > -180)
                State = PlayerState.DashBackLeft;
            else if (finalAngle == 180 || finalAngle == -180)
                State = PlayerState.DashBackward;
        }
    }

    IEnumerator IncreaseSpeed()
    {
        float tmpSpeed = speed;
        float tmpAngle = cm.Angle;
        canDash = false;
        isDash = true;
        speed *= 7;
        cm.Angle *= 2;
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        isDash = false;
        cm.Angle = tmpAngle;
        tr.emitting = false;
        yield return new WaitForSeconds(dashCool - dashTime);
        canDash = true;
    }

    IEnumerator IncreaseSpeedEnemy()
    {
        float tmpSpeed = speed;
        canDash = false;
        isDash = true;
        speed *= 7;
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        isDash = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashCool - dashTime);
        canDash = true;
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(0.5f);
        changeAct = true;
    }
}
