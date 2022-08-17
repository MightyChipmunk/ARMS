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
    float dist;

    #region 공용 필요 속성
    GameObject target;
    CharacterController cc;
    Animator anim;
    JH_Effect effect;
    //TrailRenderer tr;
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
                    anim.SetBool("Falling", true);
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
    float dashCool = 0.5f;
    bool canDash = true;
    bool isDash = false;
    public bool IsDash 
    { 
        get 
        { 
            return isDash; 
        } 
        set
        {
            if (value != isDash)
            {
                effect.DashEffect(value);
            }
            isDash = value;
        }
    }
    bool hitted = false;
    public bool hittedp
    {
        get { return hitted; }
        set
        {
            if (value != hitted)
            {
                effect.HittedEffect(value);
            }
            hitted = value;
        }
    }

    bool knocked = false;
    public bool Knocked
    {
        get { return knocked; }
        set
        {
            if (value != knocked)
            {
                effect.HittedEffect(value);
                if (value)
                    StartCoroutine("KnockedEvent");
            }
            knocked = value;
        }
    }

    [SerializeField]
    float speed = 2.0f;
    [SerializeField]
    public float jumpPower = 7f;
    [SerializeField]
    bool isEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<JH_Effect>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //tr = transform.Find("DashTrail").GetComponent<TrailRenderer>();

        if (isEnemy)
        {
            eh = GetComponent<SY_EnemyHp>();
            elf = transform.Find("Left").GetComponent<YJ_LeftFight_enemy>();
            erf = transform.Find("Right").GetComponent<YJ_RightFight_enemy>();
            elc = transform.Find("Left").GetComponent<SY_EnemyLeftCharge>();
            etrigger = GameObject.Find("Player").transform.Find("Left").transform.Find("YJ_Trigger").GetComponent<YJ_Trigger>();

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
            Knocked = ph.IsKnock;
        }
        else
        {
            Jump(isEnemy);
            Move(isEnemy);
            Dash(isEnemy);
            SetEnemyState();
            Knocked = eh.IsKnock;
        }

        if (state != PlayerState.KnockBack)
            LookEnemy();
    }

    void LookEnemy()
    {
        dir = target.transform.position - transform.position;
        dir.y = 0;
        dist = dir.magnitude;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Move()
    {
        if (cc.isGrounded || dist <= 5.0f)
            moveDir = Vector3.zero;

        if (IsCanMove())
        {
            if (InputManager.Instance.Front && dist >= 5f)
            {
                moveDir += dir;
            }
            if (InputManager.Instance.Back)
            {
                moveDir -= dir;
            }
            if (InputManager.Instance.Left && dist < 5f)
            {
                moveDir -= transform.right;
                moveDir -= dir;
            }
            else if (InputManager.Instance.Left && dist >= 5f)
            {
                moveDir -= transform.right;
            }
            if (InputManager.Instance.Right && dist < 5f)
            {
                moveDir += transform.right;
                moveDir -= dir;
            }
            else if (InputManager.Instance.Right && dist >= 5f)
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
        if (cc.isGrounded || dist <= 5.0f)
            moveDir = Vector3.zero;

        if (IsCanMove(isEnemy))
        {
            if (InputManager.Instance.EnemyFront && dist >= 5.0f)
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
        if (InputManager.Instance.Dash && canDash && IsCanMove() && 
            ((InputManager.Instance.Front && Vector3.Magnitude(target.transform.position - transform.position) >= 5f) 
            || InputManager.Instance.Left || InputManager.Instance.Back || InputManager.Instance.Right))
        {
            StartCoroutine("IncreaseSpeed");
        }
    }

    void Dash(bool isEnemy)
    {
        if (InputManager.Instance.EnemyDash && canDash && IsCanMove(isEnemy) && 
            ((InputManager.Instance.EnemyFront && Vector3.Magnitude(target.transform.position - transform.position) >= 5f)
            || InputManager.Instance.EnemyLeft || InputManager.Instance.EnemyBack || InputManager.Instance.EnemyRight))
        {
            StartCoroutine("IncreaseSpeedEnemy");
        }
    }

    void SetPlayerState()
    {
        if (cc.isGrounded && !isDash)
        {
            anim.SetBool("Falling", false);
            // moveDir의 각도를 계산해서 상태 정함
            float angle = Vector3.Angle(moveDir, transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(moveDir, transform.right));
            float finalAngle = sign * angle;
            float radian = finalAngle * Mathf.PI / 180;

            anim.SetFloat("PosX", Mathf.Lerp(anim.GetFloat("PosX"), Mathf.Sin(radian), Time.deltaTime * 5));
            anim.SetFloat("PosY", Mathf.Lerp(anim.GetFloat("PosY"), Mathf.Cos(radian), Time.deltaTime * 5));

            if (!(InputManager.Instance.Front || InputManager.Instance.Left ||
                  InputManager.Instance.Back || InputManager.Instance.Right || InputManager.Instance.Jump))
                State = PlayerState.Idle;
            else
                State = PlayerState.Move;
        }
        else if (!cc.isGrounded && !isDash)
        {
            // 공중에 있을 때 낙하모션 재생
            State = PlayerState.Fall;
        }

        if (ph.IsKnock)
        {
            if (State != PlayerState.KnockBack)
                State = PlayerState.KnockBack;

            if (ph.CanUp && (InputManager.Instance.Front || InputManager.Instance.Left ||
                  InputManager.Instance.Back || InputManager.Instance.Right))
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
        else if (hitted)
        {
            State = PlayerState.Idle;
        }
        else if (lf.Fire || rf.Fire)
        {
            if (State != PlayerState.Attack)
                State = PlayerState.Attack;

            if (lf.Fire)
                anim.SetFloat("PunchLeft", Mathf.Lerp(anim.GetFloat("PunchLeft"), 1, Time.deltaTime * 5));
            else
                anim.SetFloat("PunchLeft", Mathf.Lerp(anim.GetFloat("PunchLeft"), 0, Time.deltaTime * 5));
            if (rf.Fire)
                anim.SetFloat("PunchRight", Mathf.Lerp(anim.GetFloat("PunchRight"), 1, Time.deltaTime * 5));
            else
                anim.SetFloat("PunchRight", Mathf.Lerp(anim.GetFloat("PunchRight"), 0, Time.deltaTime * 5));
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
            anim.SetBool("Falling", false);
            // moveDir의 각도를 계산해서 상태 정함
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
            // 공중에 있을 때 낙하모션 재생
            State = PlayerState.Fall;
        }

        if (eh.IsKnock)
        {
            if (State != PlayerState.KnockBack)
                State = PlayerState.KnockBack;

            if (eh.CanUp && (InputManager.Instance.EnemyFront || InputManager.Instance.EnemyLeft
                || InputManager.Instance.EnemyRight || InputManager.Instance.EnemyBack))
            {
                if (eh.coroutine != null)
                {
                    eh.StopCoroutine(eh.coroutine);
                }
                eh.IsKnock = false;
                eh.CanUp = false;
                //StartCoroutine("IncreaseSpeed");
                StartCoroutine("FallEnemy");
                anim.SetTrigger("Fall");
            } 
        }
        else if (hitted)
        {
            State = PlayerState.Idle;
        }
        else if (elf.Fire || erf.Fire)
        {
            if (State != PlayerState.Attack)
                State = PlayerState.Attack;

            if (elf.Fire)
                anim.SetFloat("PunchLeft", Mathf.Lerp(anim.GetFloat("PunchLeft"), 1, Time.deltaTime * 5));
            else
                anim.SetFloat("PunchLeft", Mathf.Lerp(anim.GetFloat("PunchLeft"), 0, Time.deltaTime * 5));
            if (erf.Fire)
                anim.SetFloat("PunchRight", Mathf.Lerp(anim.GetFloat("PunchRight"), 1, Time.deltaTime * 5));
            else
                anim.SetFloat("PunchRight", Mathf.Lerp(anim.GetFloat("PunchRight"), 0, Time.deltaTime * 5));
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

    public bool IsFire()
    {
        if (rf.Fire == true || lf.Fire == true)
        {
            return true;
        }
        else
            return false;
    }

    public bool IsGrapped()
    {
        if (trigger.enemyCome == true || trigger.enemyGo == true)
            return true;
        else
            return false;
    }

    public bool IsFire(bool isEnemy)
    {
        if (erf.Fire == true || elf.Fire == true)
        {
            return true;
        }
        else
            return false;
    }

    public bool IsGrapped(bool isEnemy)
    {
        if (etrigger.enemyCome == true || etrigger.enemyGo == true)
            return true;
        else
            return false;
    }

    public bool IsCanMove()
    {
        if (!IsFire() && lc.IsGuard == false && ph.IsKnock == false && hitted == false && !IsGrapped()) // 공격, 가드, 넉백 시 움직임 불가
            return true;
        else
            return false;
    }

    public bool IsCanMove(bool isEnemy)
    {
        if (!IsFire(isEnemy) && elc.IsGuard == false && eh.IsKnock == false && hitted == false && !IsGrapped(isEnemy)) // 공격, 가드, 넉백 시 움직임 불가
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
        IsDash = true;
        speed *= 7;
        cm.Angle *= 10;
        //tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        IsDash = false;
        cm.Angle = tmpAngle;
        //tr.emitting = false;
        yield return new WaitForSeconds(dashCool - dashTime);
        canDash = true;
    }

    IEnumerator IncreaseSpeedEnemy()
    {
        float tmpSpeed = speed;
        canDash = false;
        IsDash = true;
        speed *= 7;
        //tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        IsDash = false;
        //tr.emitting = false;
        yield return new WaitForSeconds(dashCool - dashTime);
        canDash = true;
    }

    IEnumerator Fall()
    {
        float tmpSpeed = speed;
        float tmpAngle = cm.Angle;
        canDash = false;
        speed *= 7;
        cm.Angle *= 10;
        yield return new WaitForSeconds(0.15f);
        speed = tmpSpeed;
        cm.Angle = tmpAngle;
        yield return new WaitForSeconds(0.35f);
        canDash = true;
    }

    IEnumerator FallEnemy()
    {
        float tmpSpeed = speed;
        canDash = false;
        speed *= 7;
        yield return new WaitForSeconds(0.15f);
        speed = tmpSpeed;
        yield return new WaitForSeconds(0.35f);
        canDash = true;
    }

    IEnumerator HittedEvent()
    {
        hittedp = true;
        yield return new WaitForSeconds(0.1f);
        anim.speed = 0;
        yield return new WaitForSeconds(0.2f);
        anim.speed = 1;
        yield return new WaitForSeconds(0.3f);
        hittedp = false;
    }

    IEnumerator KnockedEvent()
    {
        anim.speed = 0;
        yield return new WaitForSeconds(0.25f);
        anim.speed = 1;
    }

    public Transform leftHandTarget;
    public Transform rightHandTarget;
    private void OnAnimatorIK(int layerIndex)
    {
        if (state == PlayerState.Attack)
        {
            // 왼손 IK
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            // 오른손 IK
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
        anim.SetLookAtWeight(1);
        anim.SetLookAtPosition(target.transform.position);
    }
}
