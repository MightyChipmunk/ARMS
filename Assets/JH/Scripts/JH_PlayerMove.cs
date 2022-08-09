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

    #region 공용 필요 속성
    GameObject target;
    CharacterController cc;
    Animator anim;
    TrailRenderer tr;
    #endregion

    #region 플레이어 필요 속성
    YJ_LeftFight lf;
    YJ_RightFight rf;
    YJ_Trigger_enemy trigger;
    SY_LeftCharge lc;
    SY_PlayerHp ph;
    JH_CameraMove cm;
    #endregion

    #region 에너미 필요 속성
    YJ_LeftFight_enemy elf;
    YJ_RightFight_enemy erf;
    YJ_Trigger etrigger;
    SY_EnemyLeftCharge elc;
    SY_EnemyHp eh;
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
    bool hitted = false;

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
        tr = transform.Find("DashTrail").GetComponent<TrailRenderer>();

        if (isEnemy)
        {
            eh = GetComponent<SY_EnemyHp>();
            elf = transform.Find("Left").GetComponent<YJ_LeftFight_enemy>();
            erf = transform.Find("Right").GetComponent<YJ_RightFight_enemy>();
            elc = transform.Find("Left").GetComponent<SY_EnemyLeftCharge>(); 

            target = GameObject.Find("Player");
        }
        else
        {
            ph = GetComponent<SY_PlayerHp>();
            lf = transform.Find("Left").GetComponent<YJ_LeftFight>();
            rf = transform.Find("Right").GetComponent<YJ_RightFight>();
            lc = transform.Find("Left").GetComponent<SY_LeftCharge>();
            trigger = GameObject.Find("Enemy").transform.Find("Left").transform.Find("YJ_Trigger").GetComponent<YJ_Trigger_enemy>();

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
            SetPlayerState();
            Debug.Log("IsCanMove: " + IsCanMove());
        }
        else
        {
            Jump(isEnemy);
            Move(isEnemy);
            Dash(isEnemy);
            SetEnemyState();
        }
        LookEnemy();

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
            if (InputManager.Instance.Front && Vector3.Magnitude(target.transform.position - transform.position) >= 5f)
            {
                moveDir += dir;
            }
            if (InputManager.Instance.Back)
            {
                moveDir -= dir;
            }
            if (InputManager.Instance.Left)
            {
                moveDir -= transform.right;
            }
            if (InputManager.Instance.Right)
            {
                moveDir += transform.right;
            }
        }

        moveDir.Normalize();
        //moveDir.y = yVelocity;
        cc.Move(moveDir * speed * Time.deltaTime);
        cc.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void Move(bool isEnemy)
    {
        if (cc.isGrounded)
            moveDir = Vector3.zero;

        if (IsCanMove(isEnemy))
        {
            if (InputManager.Instance.EnemyFront && Vector3.Magnitude(target.transform.position - transform.position) >= 5.0f)
            {
                moveDir += dir;
            }
            if (InputManager.Instance.EnemyBack)
            {
                moveDir -= dir;
            }
            if (InputManager.Instance.EnemyRight)
            {
                moveDir -= transform.right;
            }
            if (InputManager.Instance.EnemyLeft)
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
        if (InputManager.Instance.Jump && cc.isGrounded && IsCanMove())
        {
            yVelocity = jumpPower;
            anim.SetTrigger("Jump");
        }
    }

    void Jump(bool isEnemy)
    {
        if (InputManager.Instance.EnemyJump && cc.isGrounded && IsCanMove(isEnemy))
        {
            yVelocity = jumpPower;
            anim.SetTrigger("Jump");
        }
    }

    void Dash()
    {
        if (InputManager.Instance.Dash && canDash)
        {
            StartCoroutine("IncreaseSpeed");
        }
    }

    void Dash(bool isEnemy)
    {
        if (canDash && InputManager.Instance.EnemyDash)
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
            float radian = finalAngle * Mathf.PI / 180;

            anim.SetFloat("PosX", Mathf.Lerp(anim.GetFloat("PosX"), Mathf.Sin(radian), Time.deltaTime * 5));
            anim.SetFloat("PosY", Mathf.Lerp(anim.GetFloat("PosY"), Mathf.Cos(radian), Time.deltaTime * 5));

            if (!(InputManager.Instance.Front || InputManager.Instance.Left ||
                  InputManager.Instance.Back || InputManager.Instance.Right || Input.GetKey(KeyCode.Space)))
                State = PlayerState.Idle;
            else
                State = PlayerState.Move;
        }
        else if (!cc.isGrounded && !isDash)
        {
            // 공중에 있다면 점프 모션 재생
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

    void SetEnemyState()
    {
        if (cc.isGrounded && !isDash)
        {
            // moveDir의 앵글을 계산해서 애니메이션 재생
            float angle = Vector3.Angle(moveDir, transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(moveDir, transform.right));
            float finalAngle = sign * angle;
            float radian = finalAngle * Mathf.PI / 180;

            anim.SetFloat("PosX", Mathf.Lerp(anim.GetFloat("PosX"), Mathf.Sin(radian), Time.deltaTime * 5));
            anim.SetFloat("PosY", Mathf.Lerp(anim.GetFloat("PosY"), Mathf.Cos(radian), Time.deltaTime * 5));

            if (moveDir == Vector3.zero)
                State = PlayerState.Idle;
            else
                State = PlayerState.Move;
        }
        else if (!cc.isGrounded && !isDash)
        {
            // 공중에 있다면 점프 모션 재생
            State = PlayerState.Fall;
        }

        if (eh.IsKnock)
        {
            //if (State != PlayerState.KnockBack)
            //    State = PlayerState.KnockBack;

            //if (eh.CanUp && ran == 8)
            //{
            //    if (ph.coroutine != null)
            //    {
            //        ph.StopCoroutine(ph.coroutine);
            //    }
            //    ph.IsKnock = false;
            //    ph.CanUp = false;
            //    //StartCoroutine("IncreaseSpeed");
            //    StartCoroutine("Fall");
            //    anim.SetTrigger("Fall");
            //}
        }
        else if (elf.Fire/* || erf.Fire*/)
        {
            if (State != PlayerState.Attack)
                State = PlayerState.Attack;
        }
        else if (elf.Grapp)
        {
            if (State != PlayerState.Grap)
                State = PlayerState.Grap;
        }
        else if (elc.IsGuard)
        {
            if (State != PlayerState.Guard)
                State = PlayerState.Guard;
        }
    }

    public bool IsCanMove()
    {
        if (/*rf.Fire == false && */lf.Fire == false && lc.IsGuard == false && ph.IsKnock == false && hitted == false &&
            trigger.enemyCome == false && trigger.enemyGo == false) // 가드, 넉백 당할때, 잡기 당할때 추가해야됨
            return true;
        else
            return false;
    }

    public bool IsCanMove(bool isEnemy)
    {
        if (/*erf.Fire == false && */elf.Fire == false && elc.IsGuard == false && eh.IsKnock == false && hitted == false) // 가드, 넉백 당할때, 잡기 당할때 추가해야됨
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

}
