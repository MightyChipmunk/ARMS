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
        Left,
        Jump,
        Dash,
    }

    Vector3 dir;
    Vector3 moveDir = Vector3.zero;
    GameObject target;
    CharacterController cc;
    JH_CameraMove cm;
    State state;
    float gravity = -3;
    float yVelocity;
    float dashTime = 0.13f;
    float dashCool = 1f;
    bool canDash = true;
    bool isDash = false;
    bool changeAct = true;
    int ran = 0;

    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    public float jumpPower = 1.5f;
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
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Move()
    {
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

        if (cc.isGrounded)
            moveDir = Vector3.zero;

        moveDir.Normalize();
        moveDir.y = yVelocity;
        cc.Move(moveDir * speed * Time.deltaTime);
    }

    void Move(int ran)
    {
        moveDir = Vector3.zero;
        if (ran <= 3)
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
        moveDir.y = yVelocity;
        cc.Move(moveDir * speed * Time.deltaTime);
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
        if (canDash && ran <= 1)
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
        speed *= 5;
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
            moveDir = Vector3.zero;
            // moveDir의 앵글을 계산해서 애니메이션 재생
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
        Debug.Log(state);
    }

    IEnumerator IncreaseSpeedEnemy()
    {
        float tmpSpeed = speed;
        canDash = false;
        speed *= 5;
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
