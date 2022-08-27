using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��ư�� ������ �ٱ��� �밢�� �������� ���ϰ� Ÿ���������� �Ĵٺ���ʹ�.
public class YJ_LeftFox : YJ_Hand_left
{
    Vector3 dir;
    float speed = 7f;
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

    // �������� �ֳʹ̿� ��Ҵ��� Ȯ���� ��
    YJ_LeftFox_lazer yj_leftfox_lazer;

    //bool goRay;
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        
        // ������ �Ⱥ��̰� �� ��
        cylinder.GetComponent<MeshRenderer>().enabled = false;

        // �ִϸ��̼�
        anim = GetComponent<Animation>();

        anim.Play("idleee");

        yj_leftfox_lazer = cylinder.GetComponent<YJ_LeftFox_lazer>();
    }


    void Update()
    {
        //OpenMouse();

        // �ʻ�� ����
        if (yj_KillerGage)
        {
            speed = 20f;
        }
        if(!yj_KillerGage)
        {
            speed = 7f;
        }


        if (Input.GetMouseButtonDown(0) && !fire)
        {
            
            anim.Stop("idleee");
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
            if(openMouseTime > 0.25f && !yj_leftfox_lazer.triggerOn)
            {
                // ������ �߻�
                lazer.transform.localScale += new Vector3(0, 0, 1f) * 10 * Time.deltaTime;
            }
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

            // ���̰� �ѵΰ�
            cylinder.GetComponent<MeshRenderer>().enabled = true;

            // �߻��� �� �Ĵٺ���
            transform.LookAt(enemy_pos);

            // ���� ������
            OpenMouse();

            // �������� �ֳʹ̿� ���� �ʾ�����
          
                // �������߻�
                lazerOn = true;
            
            //anim.Play("Attack 1");
            // ���� ���� �ѹ�����
            //Debug.DrawLine(transform.position, enemy_pos * 1f, Color.red, 5f);
            
            // 0.5�� �Ŀ� ������ ���ϰ�
            currentTime += Time.deltaTime;

            if (currentTime > 1f )
            {
                //anim.Stop("Attack 1");
                //anim.Play("Attack 2");
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
            
            openMouseTime = 0;
            distance = 0;
            currentTime = 0;
            
            if(lazer.transform.localScale.x > 0.1f)
            {
                // ������ ũ�� �ٿ��ֱ�
                lazer.transform.localScale -= new Vector3(0.5f, 0.5f, 0) * 1 * Time.deltaTime;
            }
            // ���� �پ� �������
            else
            {
                // �Ⱥ��̰� ���ְ�
                cylinder.GetComponent<MeshRenderer>().enabled = false;

                // �غ���� ũ��� ����
                lazer.transform.localScale = new Vector3(0.1f, 0.1f, 0.03f);

                // �Դݱ� �ִϸ��̼�
                CloseMouse();

            }

            // �� �� �ݰ� ����
            if (closeMouseTime > 0.25f)
            { 
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
                yj_leftfox_lazer.triggerOn = false;
                // LookAt Ǯ��
                transform.forward = Camera.main.transform.forward;
                currentTime = 0;
                closeMouseTime = 0;
                anim.Play("idleee");
                fire = false;
                back = false;
            }

        }
    }

    public GameObject helm_visor;
    public GameObject ear_main_L;
    public GameObject ear_main_R;
    public GameObject sidewing_L;
    public GameObject sidewing_R;
    float openMouseTime = 0;
    float closeMouseTime = 0;
    void OpenMouse()
    {
        openMouseTime += Time.deltaTime;
        if(openMouseTime < 0.25f)
        {
            helm_visor.transform.eulerAngles += new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_L.transform.eulerAngles += new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_R.transform.eulerAngles += new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_L.transform.eulerAngles += new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_R.transform.eulerAngles += new Vector3(2, 0, 0) * 60 * Time.deltaTime;
        }
    }

    void CloseMouse()
    {
        closeMouseTime += Time.deltaTime;
        if (closeMouseTime < 0.25f)
        {
            helm_visor.transform.eulerAngles -= new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_L.transform.eulerAngles -= new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_R.transform.eulerAngles -= new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_L.transform.eulerAngles -= new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_R.transform.eulerAngles -= new Vector3(2, 0, 0) * 60 * Time.deltaTime;
        }
    }
}
