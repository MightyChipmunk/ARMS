using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Vector3 dir;
    Vector3 moveDir = Vector3.zero;
    GameObject target;
    CharacterController cc;
    float gravity = -3;
    float yVelocity;
    float dashTime = 0.13f;
    float dashCool = 1f;
    bool canDash = true;
    int ran = 0;
    bool changeAct = true;

    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    public float jumpPower = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        target = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

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

    void LookEnemy()
    {
        dir = target.transform.position - transform.position;
        dir.Normalize();
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void Move(int ran)
    {
        moveDir = Vector3.zero;
        if (ran <= 2)
        {
            moveDir += dir;
        }
        if (ran > 2 && ran <= 4)
        {
            moveDir -= dir;
        }
        if (ran > 4 && ran <= 6)
        {
            moveDir -= transform.right;
        }
        if (ran > 6 && ran <= 8)
        {
            moveDir += transform.right;
        }

        moveDir.Normalize();
        moveDir.y = yVelocity;
        cc.Move(moveDir * speed * Time.deltaTime);
    }

    void Jump(int ran)
    {
        if (ran > 8 && cc.isGrounded)
        {
            yVelocity = jumpPower;
        }
    }

    void Dash(int ran)
    {
        if (canDash && ran <= 1)
        {
            StartCoroutine("IncreaseSpeed");
        }
    }

    IEnumerator IncreaseSpeed()
    {
        float tmpSpeed = speed;
        canDash = false;
        speed *= 5;
        yield return new WaitForSeconds(dashTime);
        speed = tmpSpeed;
        yield return new WaitForSeconds(dashCool - dashTime);
        canDash = true;
    }

    IEnumerator RandomAct()
    {
        changeAct = false;
        yield return new WaitForSeconds(0.3f);
        changeAct = true;
    }
}
