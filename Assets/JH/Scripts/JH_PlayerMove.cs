using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JH_PlayerMove : MonoBehaviour
{
    AudioSource source;
    public AudioClip hittedSound;
    public AudioClip chargeHittedSound;
    public AudioClip dieSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;
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
        Die,
        Win,
    }

    Vector3 dir;
    Vector3 moveDir = Vector3.zero;
    float dist;
    bool camReturn = false;
    public bool CamReturn
    {
        get { return camReturn; }
    }

    #region 공용 필요 속성
    GameObject target;
    CharacterController cc;
    Animator anim;
    JH_Effect effect;
    //TrailRenderer tr;
    #endregion

    #region 플레이어 필요 속성
    YJ_Hand_left lf;
    YJ_Hand_right rf;
    YJ_Trigger_enemy etrigger; bool enemyGo = false;
    bool EnemyGo
    {
        get { return enemyGo; }
        set
        {
            if (value != enemyGo && value == true)
            {
                SY_EnemyHp enemyhp = GameObject.Find("Enemy").GetComponent<SY_EnemyHp>();
                enemyhp.SetHP(enemyhp.GetHP() - 150);
                enemyhp.IsKnock = true;
                enemyhp.coroutine = enemyhp.StartCoroutine(enemyhp.Ondamaged());
            }
            else if (value != enemyGo && value == false)
            {
                SY_EnemyHp enemyhp = GameObject.Find("Enemy").GetComponent<SY_EnemyHp>();
                cm.StopCamHit();
                cm.StartCamHit();
            }
            enemyGo = value;
        }
    }

    JH_PlayerCharge ch;
    SY_PlayerHp ph;
    JH_CameraMove cm;
    #endregion

    #region 에너미 필요 속성
    YJ_LeftFight_enemy elf;
    YJ_RightFight_enemy erf;
    YJ_Trigger trigger;
    bool playerGo = false;
    bool PlayerGo
    {
        get { return playerGo; }
        set
        {
            if (value != playerGo && value == true)
            {
                SY_PlayerHp playerhp = GameObject.Find("Player").GetComponent<SY_PlayerHp>();
                playerhp.SetHP(playerhp.GetHP() - 150);
                playerhp.IsKnock = true;
                playerhp.coroutine = playerhp.StartCoroutine(playerhp.Ondamaged());
            }
            else if (value != playerGo && value == false)
            {
                SY_PlayerHp playerhp = GameObject.Find("Player").GetComponent<SY_PlayerHp>();
                target.transform.Find("Main Camera").GetComponent<JH_CameraMove>().StopCamHitted();
                target.transform.Find("Main Camera").GetComponent<JH_CameraMove>().StartCamHitted();
            }
            playerGo = value;
        }
    }
    JH_EnemyCharge ech;
    SY_EnemyHp eh;
    #endregion
    [SerializeField]
    PlayerState state;
    public PlayerState State
    {
        get { return state; }
        set
        {
            if ((state == PlayerState.Grap || state == PlayerState.Grapped) && value != state)
            {
                StopCoroutine("CamReturnCo");
                StartCoroutine("CamReturnCo");
            }
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
                case PlayerState.Grapped:
                    anim.SetInteger("StateNum", 10);
                    break;
                case PlayerState.KnockBack:
                    anim.SetInteger("StateNum", 8);
                    break;
                case PlayerState.Die:
                    anim.SetInteger("StateNum", 999);
                    //GetComponent<CharacterController>().enabled = false;
                    StartCoroutine("DieDown");
                    break;
                case PlayerState.Win:
                    GetComponent<CharacterController>().enabled = false;
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
    bool canJump = true;
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
                //if (value)
                //    source.PlayOneShot(hittedSound);
                effect.HittedEffect(value);
                if (value && isEnemy)
                    target.transform.Find("Main Camera").GetComponent<JH_CameraMove>().StartCamHit();
                else if (value)
                    cm.StartCamHitted();
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
                {
                    source.PlayOneShot(chargeHittedSound);
                    StartCoroutine("KnockedEvent");
                }

                if (value && isEnemy)
                    target.transform.Find("Main Camera").GetComponent<JH_CameraMove>().StartCamHit();
                else if (value)
                    cm.StartCamHitted();
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
        source = GetComponent<AudioSource>();

        effect = GetComponent<JH_Effect>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //tr = transform.Find("DashTrail").GetComponent<TrailRenderer>();

        if (isEnemy)
        {
            eh = GetComponent<SY_EnemyHp>();
            elf = transform.Find("Left").GetComponent<YJ_LeftFight_enemy>();
            erf = transform.Find("Right").GetComponent<YJ_RightFight_enemy>();
            ech = GetComponent<JH_EnemyCharge>();
            
            target = GameObject.Find("Player");
        }
        else
        {
            ph = GetComponent<SY_PlayerHp>();
            lf = transform.Find("Left").GetComponent<YJ_Hand_left>();
            rf = transform.Find("Right").GetComponent<YJ_Hand_right>();
            ch = GetComponent<JH_PlayerCharge>();

            cm = transform.Find("Main Camera").GetComponent<JH_CameraMove>();
            target = GameObject.Find("Enemy Camera");
        }
        trigger = GameObject.Find("Player").transform.Find("YJ_Trigger").GetComponent<YJ_Trigger>();
        etrigger = GameObject.Find("Enemy").transform.Find("YJ_Trigger").GetComponent<YJ_Trigger_enemy>();
        State = PlayerState.Idle;

        leftHandTarget = transform.Find("Left");
        rightHandTarget = transform.Find("Right");
    }

    // Update is called once per frame
    void Update()
    {

        if (!cc.isGrounded)
            yVelocity += gravity * Time.deltaTime;
        else
            yVelocity = -1f;

        if (!isEnemy && State != PlayerState.Die && State != PlayerState.Win)
        {
            Jump();
            Move();
            Dash();
            SetPlayerState();
            EnemyGo = trigger.enemyGo;
            Knocked = ph.IsKnock;
        }
        else if (State != PlayerState.Die && State != PlayerState.Win)
        {
            Jump(isEnemy);
            Move(isEnemy);
            Dash(isEnemy);
            SetEnemyState();
            PlayerGo = etrigger.playerGo;
            Knocked = eh.IsKnock;
        }

        if (state != PlayerState.KnockBack && State != PlayerState.Die)
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
        if (InputManager.Instance.Jump && cc.isGrounded && IsCanMove() && canJump)
        {
            yVelocity = jumpPower;
            source.PlayOneShot(jumpSound);
            anim.SetTrigger("Jump");
        }
    }

    void Jump(bool isEnemy)
    {
        if (InputManager.Instance.EnemyJump && cc.isGrounded && IsCanMove(isEnemy) && canJump)
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
            source.PlayOneShot(dashSound);
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
                anim.SetTrigger("Fall");
                StartCoroutine("Fall");
            }
        }
        else if (hitted)
        {
            State = PlayerState.Idle;
        }
        else if (IsGrapped())
        {
            State = PlayerState.Grapped;
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
        else if (trigger.Grapp)
        {
            if (State != PlayerState.Grap)
                State = PlayerState.Grap;
        }
        else if (ch.IsGuard)
        {
            if (State != PlayerState.Guard)
                State = PlayerState.Guard;
        }
        else if (cc.isGrounded && !isDash)
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
                anim.SetTrigger("Fall");
                StartCoroutine("FallEnemy");
            }
        }
        else if (hitted)
        {
            State = PlayerState.Idle;
        }
        else if (IsGrapped(isEnemy))
        {
            State = PlayerState.Grapped;
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
        else if (etrigger.Grapp)
        {
            if (State != PlayerState.Grap)
                State = PlayerState.Grap;
        }
        else if (ech.IsGuard)
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
        if (etrigger.playerCome == true || etrigger.playerGo == true)
        {
            return true;
        }
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
        if (trigger.enemyCome == true || trigger.enemyGo == true)
        {
            return true;
        }
        else
            return false;
    }

    public bool IsCanMove()
    {
        if (!IsFire() && ch.IsGuard == false && ph.IsKnock == false && hitted == false && !IsGrapped()
            && state != PlayerState.Grap) // 공격, 가드, 넉백 시 움직임 불가
            return true;
        else
            return false;
    }

    public bool IsCanMove(bool isEnemy)
    {
        if (!IsFire(isEnemy) && ech.IsGuard == false && eh.IsKnock == false && hitted == false && !IsGrapped(isEnemy)
            && state != PlayerState.Grap) // 공격, 가드, 넉백 시 움직임 불가
            return true;
        else
            return false;
    }

    public void Hitted()
    {
        source.PlayOneShot(hittedSound);
        anim.SetTrigger("Hitted");
        StartCoroutine("HittedEvent");
    }

    public void Died()
    {
        source.PlayOneShot(dieSound);
        anim.SetTrigger("Die");
        hittedp = true;
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
        canJump = false;
        speed *= 7;
        cm.Angle *= 10;
        yield return new WaitForSeconds(0.15f);
        speed = tmpSpeed;
        cm.Angle = tmpAngle;
        yield return new WaitForSeconds(0.35f);
        canDash = true;
        canJump = true;
    }

    IEnumerator FallEnemy()
    {
        float tmpSpeed = speed;
        canDash = false;
        canJump = false;
        speed *= 7;
        yield return new WaitForSeconds(0.15f);
        speed = tmpSpeed;
        yield return new WaitForSeconds(0.35f);
        canDash = true;
        canJump = true;
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

    IEnumerator CamReturnCo()
    {
        camReturn = true;
        yield return new WaitForSeconds(0.7f);
        camReturn = false;
    }

    IEnumerator DieDown()
    {
        yield return new WaitForSeconds(0.5f);
        while (transform.position.y > 0)
        {
            transform.position += transform.position.y * Vector3.down * Time.deltaTime * 2;
            yield return null;
        }
    }

    Transform leftHandTarget;
    Transform rightHandTarget;
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
