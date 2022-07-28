using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_PlayerMove : MonoBehaviour
{
    Vector3 dir;
    Vector3 moveDir = Vector3.zero;
    GameObject target;
    CharacterController cc;
    JH_CameraMove cm;
    float gravity = -3;
    float yVelocity;
    float dashTime = 0.13f;
    float dashCool = 1f;
    bool canDash = true;

    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    public float jumpPower = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        cm = transform.Find("Main Camera").GetComponent<JH_CameraMove>();
        target = GameObject.Find("Enemy Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        Move();
        LookEnemy();
        Jump();
        Dash();
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

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine("IncreaseSpeed");
        }
    }

    IEnumerator IncreaseSpeed()
    {
        float tmpSpeed = speed;
        float tmpAngle = cm.Angle;
        canDash = false;
        speed *= 5;
        cm.Angle *= 2;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        cm.Angle = tmpAngle;
        yield return new WaitForSeconds(dashCool - dashTime);
        canDash = true;
    }
}
