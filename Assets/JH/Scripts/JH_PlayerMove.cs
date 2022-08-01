using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_PlayerMove : MonoBehaviour
{
    enum State
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
        Dash,
    }

    Vector3 dir;
    Vector3 moveDir = Vector3.zero;
    GameObject target;
    CharacterController cc;
    JH_CameraMove cm;
    State state;
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

        if (isEnemy)
        {
            target = GameObject.Find("Main Camera");
        }
        else
        {
            cm = transform.Find("Main Camera").GetComponent<JH_CameraMove>();
            target = GameObject.Find("Enemy Camera");
        }

        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        if (!isEnemy)
        {
            Move();
            LookEnemy();
            Jump();
            Dash();
        }
        else
        {
            if (changeAct)
            {
                ran = Random.Range(1, 10);
                StartCoroutine("RandomAct");
            }

            Move(ran);
            LookEnemy();
            Jump(ran);
            Dash(ran);
        }

        SetState();
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
        if (Input.GetKey(KeyCode.W))
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
        if (ran <= 2)
        {
            moveDir += dir;
        }
        if (ran >= 2 && ran <= 4)
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

    IEnumerator IncreaseSpeed()
    {
        float tmpSpeed = speed;
        float tmpAngle = cm.Angle;
        canDash = false;
        isDash = true;
        speed *= 7;
        cm.Angle *= 2;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        isDash = false;
        cm.Angle = tmpAngle;
        yield return new WaitForSeconds(dashCool - dashTime);
        canDash = true;
    }

    void SetState()
    {
        if (cc.isGrounded && !isDash)
        {
            // moveDir의 앵글을 계산해서 애니메이션 재생
            float angle = Vector3.Angle(moveDir, transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(moveDir, transform.right));
            float finalAngle = sign * angle;
            if (finalAngle >= -1f && finalAngle <= 1f)
                state = State.Forward;
            else if (finalAngle > 1f && finalAngle <= 89f)
                state = State.FrontRight;
            else if (finalAngle > 89 && finalAngle < 91)
                state = State.Right;
            else if (finalAngle > 91 && finalAngle < 180)
                state = State.BackRight;
            else if (finalAngle < -1f && finalAngle >= -89f)
                state = State.FrontLeft;
            else if (finalAngle < -89 && finalAngle > -91)
                state = State.Left;
            else if (finalAngle < -91 && finalAngle > -180)
                state = State.BackLeft;
            else if (finalAngle == 180 || finalAngle == -180)
                state = State.Backward;
            
            if (!Input.anyKey)
                state = State.Idle;
        }
        else if (!cc.isGrounded && !isDash)
        {
            // 공중에 있다면 점프 모션 재생
            state = State.Jump;
        }
        else if (isDash)
        {
            // 대쉬 중이라면 공중이라도 대쉬 모션 재생
            state = State.Dash;
        }
    }

    IEnumerator IncreaseSpeedEnemy()
    {
        float tmpSpeed = speed;
        canDash = false;
        speed *= 7;
        isDash = true;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        isDash = false;
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
