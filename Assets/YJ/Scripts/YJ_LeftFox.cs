using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 왼쪽 버튼을 누르면 바깥쪽 대각선 방향으로 향하고 타겟포지션을 쳐다보고싶다.
public class YJ_LeftFox : YJ_Hand_left
{
    Vector3 dir;
    float speed = 5f;
    public float distance = 0f;
    float distance_e = 0f;

    public GameObject originPos;
    GameObject enemy;
    GameObject player;
    public GameObject lazer;
    Vector3 enemy_pos;

    bool go;
    bool back;
    public bool lazerOn; //protected 자식만 상속가능

    Animation anim;

    //bool goRay;
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        
        // 레이저 안보이게 할 것
        cylinder.GetComponent<MeshRenderer>().enabled = false;

        // 애니메이션
        anim = GetComponent<Animation>();

        //anim.Stop();
    }


    void Update()
    {
        // 필살기 감지
        if(yj_KillerGage)
        {
            speed = 10f;
        }
        if(!yj_KillerGage)
        {
            speed = 5f;
        }


        if (Input.GetMouseButtonDown(0) && !fire)
        {
            fire = true;
            // 눌렀을때 애너미 처음 위치
            enemy_pos = enemy.transform.position;

            //방향
            dir = -player.transform.position + transform.position;
            dir.Normalize();
            go = true;
        }
        if (go)
        {
            // 대각선으로 이동
            distance += speed * Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
        }


        GoRay();

        if(lazerOn)
        {
            anim.Play("Left_Fox_Attack");
            // 레이저 발사
            lazer.transform.localScale += new Vector3(0, 0, 1f * 5 * Time.deltaTime);
        }

        ScaleDown();

        if (back)
            GoBack();

    }


    float currentTime = 0;
    public bool scaleDown;
    private void GoRay()
    {
        // 손이 2.5만큼 이동하면
        if (distance > 2.5f)
        {
            go = false;
            lazerOn = true;

            // 보이게 켜두고
            cylinder.GetComponent<MeshRenderer>().enabled = true;

            // 발사할 곳 쳐다보기
            transform.LookAt(enemy_pos);

            // 어디로 쏠지 한번볼까
            Debug.DrawLine(transform.position, enemy_pos * 1f, Color.red, 5f);
            
            // 0.5초 후에 사이즈 줄일것
            currentTime += Time.deltaTime;

            if (currentTime > 0.5f)
            {
                scaleDown = true;
                lazerOn = false;
                distance = 0;
            }

        }

    }

    public GameObject cylinder;
    private void ScaleDown()
    {
        if (scaleDown)
        {
            distance = 0;
            currentTime = 0;

            // 레이저 크기 줄여주기
            lazer.transform.localScale -= new Vector3(0.5f, 0.5f, 0) * 1 * Time.deltaTime;

            // 많이 줄어 들었으면
            if(lazer.transform.localScale.x < 0.02f)
            {
                // 안보이게 꺼주고
                cylinder.GetComponent<MeshRenderer>().enabled = false;

                // 준비상태 크기로 변경
                lazer.transform.localScale = new Vector3(0.1f, 0.1f, 0.03f);
                back = true;
                scaleDown = false;
            }

        }
    }

    private void GoBack()
    {
        currentTime += Time.deltaTime;
        
        if(currentTime > 0.2f)
        {
            // 원래있던 자리로 방향지정
            dir = originPos.transform.position - transform.position;
            dir.Normalize();

            // 갑시다
            transform.position += dir * speed * Time.deltaTime;

            // 거의 다왔으면 완전히 되돌리기
            if (Vector3.Distance(transform.position, originPos.transform.position) < 0.2f)
            {
                transform.position = originPos.transform.position;

                // LookAt 풀기
                transform.forward = Camera.main.transform.forward;
                currentTime = 0;
                fire = false;
                back = false;
            }

        }
    }
}
