using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� �̵���Ų �� ������ ������ ������� �߻��ϰ�ʹ�.

public class YJ_RightRevolver : YJ_Hand_right
{
    // ����������
    public YJ_Revolver4 revolver_4;
    public YJ_Revolver5 revolver_5;
    public YJ_Revolver6 revolver_6;

    // origin��ġ
    public Transform originPos;

    // �̵��ӵ�
    float speed = 15f;
    float backspeed = 20f;

    // ����
    Vector3 dir;

    // �÷��̾���ġ ��������
    GameObject player;

    // ������ �߻��� bool��
    public bool isFire = false;

    // �ִϸ��̼�
    Animation anim;

    void Start()
    {
        player = GameObject.Find("Player");

        transform.forward = player.transform.forward;

        yj_KillerGage = GameObject.Find("KillerGage (2)").GetComponent<YJ_KillerGage>();
        originPos = GameObject.Find("rightPos").transform;

        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (yj_KillerGage.killerModeOn)
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
        if (InputManager.Instance.Fire2 && !fire)
        {
            anim.Stop();
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
        if (Vector3.Distance(transform.position, player.transform.position) > 2.5f)
        {
            dir = Vector3.zero;
            // �������߻�
            isFire = true;
        }
        // ��� �������� ���ڸ��� ���ƿԴٸ�
        if (revolver_4.end && revolver_5.end && revolver_6.end)
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
            revolver_4.end = false;
            revolver_5.end = false;
            revolver_6.end = false;
            // �ִϸ��̼� �÷���
            anim.Play();
            // ��������
            fire = false;
        }

    }
}
