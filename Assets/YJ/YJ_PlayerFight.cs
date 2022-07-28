using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
public class YJ_PlayerFight : MonoBehaviour
{
    // 누가 움직일것인지
    public GameObject left;
    public GameObject right;
    // 공격 속도
    float leftspeed = 10f;
    float rightspeed = 10f;
    float backspeed = 20f;
    // 타겟
    GameObject target;
    GameObject player;
    // 타겟위치
    Vector3 targetPos;
    Transform originPos;
    // 왼쪽버튼 눌림확인
    bool fire1 = false;
    bool fire2 = false;
    bool click = false;
    bool click2 = false;

    // Start is called before the first frame update
    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;
        targetPos = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        // 일정거리만큼 (Z 15)

            print(Vector3.Distance(transform.position, player.transform.position));

        // 왼쪽 마우스를 누르면
        if(Input.GetButtonDown("Fire1") && !click)
        {
            fire1 = true;            
        }
        if(fire1)
            LeftFight();

        if (Input.GetButtonDown("Fire2") && !click)
        {
            fire2 = true;
        }
        if (fire2)
            RightFight();

    }



    void LeftFight()
    {
        if (fire1)
        {
            Vector3 dir = targetPos - left.transform.position;
            dir.Normalize();
            // 이동하고싶다
            left.transform.position += dir * leftspeed * Time.deltaTime;
            // 만약에 캐릭터로부터 5만큼 앞으로 갔다면 정지
            if (Vector3.Distance(left.transform.position, player.transform.position) > 10f)
            {
                print("맞아?");
                leftspeed = 0f;
                click = true;
            }
        }
        // 캐릭터로부터 5만큼 떨어졌다면
        if (fire1 && click)
        {
            // 되돌아오기
            left.transform.position = Vector3.Lerp(left.transform.position, originPos.position + new Vector3(-1.23f, 0f, 0.75f), Time.deltaTime * backspeed);

            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(left.transform.position, player.transform.position) < 1.45f)
            {
                print("다돌아왔어?");
                click = false;
                fire1 = false;
                leftspeed = 10f;
            }
        }
    }

    void RightFight()
    {
        if (fire2)
        {
            Vector3 dir = targetPos - right.transform.position;
            dir.Normalize();
            // 이동하고싶다
            right.transform.position += dir * rightspeed * Time.deltaTime;
            // 만약에 캐릭터로부터 5만큼 앞으로 갔다면 정지
            if (Vector3.Distance(right.transform.position, player.transform.position) > 10f)
            {
                print("맞아?");
                rightspeed = 0f;
                click2 = true;
            }
        }
        // 캐릭터로부터 5만큼 떨어졌다면
        if (fire2 && click2)
        {
            // 되돌아오기
            right.transform.position = Vector3.Lerp(right.transform.position, originPos.position + new Vector3(1.23f, 0f, 0.75f), Time.deltaTime * backspeed);

            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(right.transform.position, player.transform.position) < 1.45f)
            {
                print("다돌아왔어?");
                click2 = false;
                fire2 = false;
                rightspeed = 10f;
            }
        }
    }
}
