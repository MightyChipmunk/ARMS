using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JH_PlayerMove : MonoBehaviour
{

    public enum PlayerState
    {
        Idle,
        Move,
        Jump,
        Fall,
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

    #region ���� �ʿ� �Ӽ�
    GameObject target;
    CharacterController cc;
    Animator anim;
    TrailRenderer tr;
    #endregion

    #region �÷��̾� �ʿ� �Ӽ�
    YJ_LeftFight lf;
    YJ_RightFight rf;
    SY_LeftCharge lc;
    SY_PlayerHp ph;
    JH_CameraMove cm;
    #endregion

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
                case PlayerState.Move:
                    anim.SetInteger("StateNum", 1);
                    break;
                case PlayerState.Fall:
                    anim.SetInteger("StateNum", 2);
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
    bool hitted = false;
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
        rf = transform.Find("Right").GetComponent<YJ_RightFight>();
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
            anim.SetTrigger("Jump");
        }
    }

    void Jump(int ran)
    {
        if (ran > 8 && cc.isGrounded)
        {
            yVelocity = jumpPower;
            anim.SetTrigger("Jump");
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
            // moveDir�� �ޱ��� ����ؼ� �ִϸ��̼� ���
            float angle = Vector3.Angle(moveDir, transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(moveDir, transform.right));
            float finalAngle = sign * angle;
            float radian = finalAngle * Mathf.PI / 180;

            anim.SetFloat("PosX", Mathf.Lerp(anim.GetFloat("PosX"), Mathf.Sin(radian), Time.deltaTime * 5));
            anim.SetFloat("PosY", Mathf.Lerp(anim.GetFloat("PosY"), Mathf.Cos(radian), Time.deltaTime * 5));

            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                  Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space))
                && !isEnemy)
                State = PlayerState.Idle;
            else if (ran == 9 && isEnemy)
                State = PlayerState.Idle;
            else
                State = PlayerState.Move;
        }
        else if (!cc.isGrounded && !isDash)
        {
            // ���߿� �ִٸ� ���� ��� ���
            State = PlayerState.Fall;
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
                //StartCoroutine("IncreaseSpeed");
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
        if (/*rf.Fire == false && */lf.Fire == false && lc.IsGuard == false && ph.IsKnock == false && hitted == false) // ����, �˹� ���Ҷ�, ��� ���Ҷ� �߰��ؾߵ�
            return true;
        else
            return false;
    }

    public void Hitted()
    {
        anim.SetTrigger("Hitted");
        StartCoroutine("HittedEvent");
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
        canDash = false;
        speed *= 7;
        cm.Angle *= 2;
        yield return new WaitForSeconds(0.15f);
        speed = tmpSpeed;
        cm.Angle = tmpAngle;
        yield return new WaitForSeconds(0.35f);
        canDash = true;
    }

    IEnumerator HittedEvent()
    {
        hitted = true;
        yield return new WaitForSeconds(0.5f);
        hitted = false;
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(0.5f);
        changeAct = true;
    }
}
