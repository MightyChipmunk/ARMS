using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��ư�� ������ �ٱ��� �밢�� �������� ���ϰ� Ÿ���������� �Ĵٺ���ʹ�.
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
    public bool lazerOn; //protected �ڽĸ� ��Ӱ���

    Animation anim;

    //bool goRay;
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        
        // ������ �Ⱥ��̰� �� ��
        cylinder.GetComponent<MeshRenderer>().enabled = false;

        // �ִϸ��̼�
        anim = GetComponent<Animation>();

        //anim.Stop();
    }


    void Update()
    {
        // �ʻ�� ����
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
            // �������� �ֳʹ� ó�� ��ġ
            enemy_pos = enemy.transform.position;

            //����
            dir = -player.transform.position + transform.position;
            dir.Normalize();
            go = true;
        }
        if (go)
        {
            // �밢������ �̵�
            distance += speed * Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
        }


        GoRay();

        if(lazerOn)
        {
            anim.Play("Left_Fox_Attack");
            // ������ �߻�
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
        // ���� 2.5��ŭ �̵��ϸ�
        if (distance > 2.5f)
        {
            go = false;
            lazerOn = true;

            // ���̰� �ѵΰ�
            cylinder.GetComponent<MeshRenderer>().enabled = true;

            // �߻��� �� �Ĵٺ���
            transform.LookAt(enemy_pos);

            // ���� ���� �ѹ�����
            Debug.DrawLine(transform.position, enemy_pos * 1f, Color.red, 5f);
            
            // 0.5�� �Ŀ� ������ ���ϰ�
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

            // ������ ũ�� �ٿ��ֱ�
            lazer.transform.localScale -= new Vector3(0.5f, 0.5f, 0) * 1 * Time.deltaTime;

            // ���� �پ� �������
            if(lazer.transform.localScale.x < 0.02f)
            {
                // �Ⱥ��̰� ���ְ�
                cylinder.GetComponent<MeshRenderer>().enabled = false;

                // �غ���� ũ��� ����
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
            // �����ִ� �ڸ��� ��������
            dir = originPos.transform.position - transform.position;
            dir.Normalize();

            // ���ô�
            transform.position += dir * speed * Time.deltaTime;

            // ���� �ٿ����� ������ �ǵ�����
            if (Vector3.Distance(transform.position, originPos.transform.position) < 0.2f)
            {
                transform.position = originPos.transform.position;

                // LookAt Ǯ��
                transform.forward = Camera.main.transform.forward;
                currentTime = 0;
                fire = false;
                back = false;
            }

        }
    }
}
