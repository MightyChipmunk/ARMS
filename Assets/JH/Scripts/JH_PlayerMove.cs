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
        Guard,
        Charge,
        Attack,
        Hitted,
        KnockBack,
        Grap,
        Grapped,
    }

    Vector3 dir;
    Vector3 moveDir = Vector3.zero;
    GameObject target;
    CharacterController cc;
    Animator anim;
    JH_CameraMove cm;
    TrailRenderer tr;
    YJ_LeftFight lf;
    YJ_RightFight rf;
    SY_LeftCharge lc;
    SY_PlayerHp ph;
    PlayerState state;
    public PlayerState State
    {
        get { return state; }
        set
        {
            state = value;

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
                case PlayerState.Guard:
                    anim.SetInteger("StateNum", 7);
                    break;
                case PlayerState.Charge:
                    break;
                case PlayerState.Attack:
                    anim.SetInteger("StateNum", 6);
                    break;
                case PlayerState.Hitted:
                    break;
                case PlayerState.Grap:
                    anim.SetInteger("StateNum", 9);
                    break;
                case PlayerState.KnockBack:
                    anim.SetInteger("StateNum", 8);
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
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        ph = GetComponent<SY_PlayerHp>();
        tr = transform.Find("DashTrail").GetComponent<TrailRenderer>();
        lf = transform.Find("Left").GetComponent<YJ_LeftFight>();
        rf = transform.Find("Left").GetComponent<YJ_RightFight>();
        lc = transform.Find("Left").GetComponent<SY_LeftCharge>();

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
            yVelocity += gravity * Time.deltaTime;
        else
            yVelocity = -1f;

        if (!isEnemy)
        {
            Jump();
            Move();
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
            Dash(ran);
        }
        LookEnemy();
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

        if (IsCanMove())
        {
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
        }

        moveDir.Normalize();
        //moveDir.y = yVelocity;
        cc.Move(moveDir * speed * Time.deltaTime);
        cc.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void Move(int ran)
    {
        if (cc.isGrounded)
            moveDir = Vector3.zero;

        if (IsCanMove())
        {
            if (ran <= 2 && Vector3.Magnitude(target.transform.position - transform.position) >= 5.0f + Vector3.Distance(new Vector3(0, 2.5f, -2), Vector3.zero))
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

        if (ph.IsKnock)
        {
            if (State != PlayerState.KnockBack)
                State = PlayerState.KnockBack;

            if (ph.CanUp && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)
                || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
            {
                if (ph.coroutine != null)
                {
                    ph.StopCoroutine(ph.coroutine);
                }
                ph.IsKnock = false;
                ph.CanUp = false;
                StartCoroutine("Fall");
                anim.SetTrigger("Fall");
            }
        }
        else if (lf.Fire/* || rf.Fire*/)
        {
            if (State != PlayerState.Attack) 
                State = PlayerState.Attack;
        }
        else if (lf.Grapp)
        {
            if (State != PlayerState.Grap)
                State = PlayerState.Grap;
        }
        else if (lc.IsGuard)
        {
            if (State != PlayerState.Guard)
                State = PlayerState.Guard;
        }
    }

    public bool IsCanMove()
    {
        if (/*rf.Fire == false && */lf.Fire == false && lc.IsGuard == false && ph.IsKnock == false) // 가드, 넉백 당할때, 잡기 당할때 추가해야됨
            return true;
        else
            return false;
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

    IEnumerator Fall()
    {
        float tmpSpeed = speed;
        float tmpAngle = cm.Angle;
        speed *= 7;
        cm.Angle *= 2;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        cm.Angle = tmpAngle;
        yield return new WaitForSeconds(dashCool - dashTime);
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(0.5f);
        changeAct = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적 팔에 닿으면 충돌 이벤트 구현
        if (other.gameObject.tag == "EnemyArms")
        {
            if ((ph.IsKnock || lc.IsGuard) /*차지 안된 공격 받으면 피격모션 재생*/ )
            {
                anim.SetTrigger("Hitted");
            }
        }
    }
}
