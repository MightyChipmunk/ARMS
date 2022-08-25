using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 왼쪽 버튼을 누르면 바깥쪽 대각선 방향으로 향하고 타겟포지션을 쳐다보고싶다.
public class YJ_LeftFox : MonoBehaviour
{
    Vector3 dir;
    float speed = 3f;
    float distance = 0f;
    float distance_e = 0f;

    public GameObject originPos;
    GameObject enemy;
    GameObject player;
    public GameObject lazer;
    Vector3 enemy_pos;

    bool go;
    bool back;
    bool lazerOn;

    //bool goRay;
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        //lazer = GameObject.Find("L_Lazer");

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            enemy_pos = enemy.transform.position;
            dir = -player.transform.position + transform.position;
            dir.Normalize();
            go = true;
        }
        if (go)
        {
            distance += speed * Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
        }


        GoRay();

        ScaleDown();

        //if (back)
        //    GoBack();

        //if (!lazerOn)
        //{
        //    back = true;
        //}
    }


    float currentTime = 0;
    bool scaleDown;
    private void GoRay()
    {
        if (distance > 2.5f)
        {
            go = false;
            lazerOn = true;
            transform.LookAt(enemy_pos);
            lazer.transform.LookAt(enemy_pos);
            Debug.DrawLine(transform.position, enemy_pos * 1f, Color.red, 5f);
            lazer.transform.localScale += new Vector3(0, 0, 2f * 8 * Time.deltaTime);

            currentTime += Time.deltaTime;

            if (currentTime > 1f)
            {
                scaleDown = true;
                distance = 0;
            }

        }

    }

    public GameObject cylinder;
    private void ScaleDown()
    {
        if (scaleDown)
        {
            currentTime = 0;

            lazer.transform.localScale -= new Vector3(1, 1, 0) * 1 * Time.deltaTime;

            if(lazer.transform.localScale.x < 0.02f)
            {
                cylinder.GetComponent<MeshRenderer>().enabled = false;
            }

        }
    }

    private void GoBack()
    {
        dir = originPos.transform.position - transform.position;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, originPos.transform.position) < 0.2f)
        {
            transform.position = originPos.transform.position;
            back = false;
        }
    }
}
