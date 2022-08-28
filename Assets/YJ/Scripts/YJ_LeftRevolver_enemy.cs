using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� �̵���Ų �� ������ ������ ������� �߻��ϰ�ʹ�.

public class YJ_LeftRevolver_enemy : YJ_Hand_left
{
    GameObject trigger; // ��� ��

    // ����������
    public YJ_Revolver7 revolver_7;
    public YJ_Revolver8 revolver_8;
    public YJ_Revolver9 revolver_9;

    // origin��ġ
    Transform originPos;

    // �̵��ӵ�
    float speed = 15f;
    float backspeed = 20f;

    // ����
    Vector3 dir;

    // �÷��̾���ġ ��������
    GameObject enemy;

    // ������ �߻��� bool��
    public bool isFire = false;

    // �ִϸ��̼�
    //Animation anim;


    YJ_Trigger_enemy yj_trigger_enemy;

    void Start()
    {
        enemy = GameObject.Find("Enemy");

        transform.forward = enemy.transform.forward;

        yj_KillerGage_enemy = GameObject.Find("KillerGage_e (2)").GetComponent<YJ_KillerGage_enemy>();

        //anim = GetComponent<Animation>();

        originPos = GameObject.Find("leftPos_e").transform;

        trigger = enemy.transform.Find("YJ_Trigger").gameObject;

        yj_trigger_enemy = trigger.GetComponent<YJ_Trigger_enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (yj_KillerGage_enemy.killerModeOn_enemy)
        {
            speed = 30f;
            backspeed = 50f;
        }
        else
        {
            speed = 15f;
            backspeed = 20f;
        }
        // ���� ���콺 ��ư�� ������ ������ ���� �̵��ϰ�ʹ�
        if (InputManager.Instance.EnemyFire1 && !fire && !yj_trigger_enemy.grap)
        {
            //anim.Stop();
            speed = 15f;
            fire = true;
        }

        // ������ ���ư���(����)
        if (fire)
            Fire_Revolver();
    }
    void Fire_Revolver()
    {
        dir = transform.forward;
        if (Vector3.Distance(transform.position, enemy.transform.position) > 2.5f)
        {
            dir = Vector3.zero;
            // �������߻�
            isFire = true;
        }
        // ��� �������� ���ڸ��� ���ƿԴٸ�
        if (revolver_7.end && revolver_8.end && revolver_9.end)
        {
            isFire = false;
            Return();
        }
        transform.position += dir * speed * Time.deltaTime;
    }

    void Return()
    {
        // �ݴ�� ���ƿ���
        speed = backspeed;
        dir = originPos.position - transform.position;
        if (Vector3.Distance(transform.position, originPos.position) < 0.3f)
        {
            // ���߱�
            dir = Vector3.zero;
            // ����ġ ���ƿ���
            transform.position = originPos.position;
            // �������� bool�� ����
            revolver_7.end = false;
            revolver_8.end = false;
            revolver_9.end = false;
            // �ִϸ��̼� �÷���
            //anim.Play();
            // ��������
            fire = false;
        }

    }
}
