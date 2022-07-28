using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
public class YJ_PlayerFight : MonoBehaviour
{
    // ���� �����ϰ�����
    public GameObject left;
    public GameObject right;
    // ���� �ӵ�
    float leftspeed = 10f;
    float rightspeed = 10f;
    float backspeed = 20f;
    // Ÿ��
    GameObject target;
    GameObject player;
    // Ÿ����ġ
    Vector3 targetPos;
    Transform originPos;
    // ���ʹ�ư ����Ȯ��
    bool fire1 = false;
    bool fire2 = false;
    bool click = false;
    bool click2 = false;

    // Start is called before the first frame update
    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;
        targetPos = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        // �����Ÿ���ŭ (Z 15)

            print(Vector3.Distance(transform.position, player.transform.position));

        // ���� ���콺�� ������
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
            // �̵��ϰ�ʹ�
            left.transform.position += dir * leftspeed * Time.deltaTime;
            // ���࿡ ĳ���ͷκ��� 5��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(left.transform.position, player.transform.position) > 10f)
            {
                print("�¾�?");
                leftspeed = 0f;
                click = true;
            }
        }
        // ĳ���ͷκ��� 5��ŭ �������ٸ�
        if (fire1 && click)
        {
            // �ǵ��ƿ���
            left.transform.position = Vector3.Lerp(left.transform.position, originPos.position + new Vector3(-1.23f, 0f, 0.75f), Time.deltaTime * backspeed);

            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(left.transform.position, player.transform.position) < 1.45f)
            {
                print("�ٵ��ƿԾ�?");
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
            // �̵��ϰ�ʹ�
            right.transform.position += dir * rightspeed * Time.deltaTime;
            // ���࿡ ĳ���ͷκ��� 5��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(right.transform.position, player.transform.position) > 10f)
            {
                print("�¾�?");
                rightspeed = 0f;
                click2 = true;
            }
        }
        // ĳ���ͷκ��� 5��ŭ �������ٸ�
        if (fire2 && click2)
        {
            // �ǵ��ƿ���
            right.transform.position = Vector3.Lerp(right.transform.position, originPos.position + new Vector3(1.23f, 0f, 0.75f), Time.deltaTime * backspeed);

            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(right.transform.position, player.transform.position) < 1.45f)
            {
                print("�ٵ��ƿԾ�?");
                click2 = false;
                fire2 = false;
                rightspeed = 10f;
            }
        }
    }
}
