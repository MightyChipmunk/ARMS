using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� �̵���Ų �� ������ ������ ������� �߻��ϰ�ʹ�.

public class YJ_LeftRevolver : MonoBehaviour
{
    // ����������
    public YJ_Revolver1 revolver_1;
    public YJ_Revolver2 revolver_2;
    public YJ_Revolver3 revolver_3;

    // origin��ġ
    public Transform originPos;

    // �̵��ӵ�
    float speed = 15f;
    float backspeed = 20f;

    // ����
    Vector3 dir;

    // �÷��̾���ġ ��������
    GameObject player;

    // ���� bool��
    bool fire = false;

    // ������ �߻��� bool��
    public bool isFire = false;

    // �ʻ�� ���
    public YJ_KillerGage yj_KillerGage;

    void Start()
    {
        player = GameObject.Find("Player");

        transform.forward = player.transform.forward;
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
        if (Input.GetMouseButtonDown(0))
        {
            speed = 15f;
            fire = true;
        }

        // ������ ���ư���(����)
        if (fire)
            Fire();
    }
    void Fire()
    {
        dir = transform.forward;
        if (Vector3.Distance(transform.position, player.transform.position) > 2.5f)
        {
            dir = Vector3.zero;
            // �������߻�
            isFire = true;
        }
        // ��� �������� ���ڸ��� ���ƿԴٸ�
        if (revolver_1.end && revolver_2.end && revolver_3.end)
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
            revolver_1.end = false;
            revolver_2.end = false;
            revolver_3.end = false;
            // ��������
            fire = false;
        }

    }
}
