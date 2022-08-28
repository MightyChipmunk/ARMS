using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// isFire�� true�� targetPos�� �����ʹ�
public class YJ_Revolver8 : MonoBehaviour
{
    // ���� bool �� (�ǵ��ƿ����� �˸�)
    public bool end = false;
    bool trigger = false; // �������

    // �ֳʹ̸� �����
    public GameObject targetPos;
    // �ֳʹ� ��ġ����
    Vector3 target;

    // ����
    Vector3 dir;

    //�Ÿ�
    float distance = 0;

    // �ӵ�
    float speed = 3f;

    // �ǵ��ƿ��¼ӵ�
    float backspeed = 5f;
    bool go = false;

    // ���ƿ� ��
    public Transform originPos;

    // isFire�� ������ ��
    public YJ_LeftRevolver_enemy leftRevolver;

    // ��߽ð�
    float currnetTime = 0;
    public float creatTime = 0;

    // �ݶ��̴� �Ѱ� ����
    Collider col;

    // �ʻ�� ���
    public YJ_KillerGage_enemy yj_KillerGage_enemy;

    // Ʈ����
    TrailRenderer trail;


    void Start()
    {
        col = GetComponent<Collider>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        leftRevolver = GameObject.Find("Enemy").transform.Find("Left").GetComponent<YJ_LeftRevolver_enemy>();
        yj_KillerGage_enemy = GameObject.Find("KillerGage_e (2)").GetComponent<YJ_KillerGage_enemy>();
        originPos = GameObject.Find("Revolver_8_Pos").transform;
        targetPos = GameObject.Find("PlayerAttackPos");
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
            speed = 5f;
            backspeed = 10f;
        }

        // isFire�� true�϶� 0.2�� �� ���ư���
        if (leftRevolver.isFire && !end )
        {
            // ���� �ݶ��̴� �ѱ�
            col.enabled = true;
            // Ʈ���� �ѱ�
            trail.enabled = true;
            //speed = 3f;
            currnetTime += Time.deltaTime;
            if (currnetTime < 0.2f)
            {
                target = targetPos.transform.position;
            }
            if (currnetTime > creatTime)
            {
                go = true;
                Go();
            }
            if (!trigger)
                dir = target - transform.position;
            if (trigger)
                dir = originPos.position - transform.position;
        }
        if (go)
            distance += speed * Time.deltaTime;
    }

    void Go()
    {
        // �Ÿ��� 1.7�̻��϶� �ǵ��ƿ���
        if (distance > 1.7f)
        {
            // Ʈ���� ����
            trail.enabled = false;
            // �ǵ��ƿ����Լ�
            Back();
        }
        // ������ ���ư���
        transform.position += dir * speed * Time.deltaTime;
    }

    void Back()
    {
        // �ö� �ݶ��̴� ����
        col.enabled = false;
        // Ʈ���� ����
        trail.enabled = false;
        // ����ٲ��ֱ�
        dir = originPos.position - transform.position;
        // �ö� ���ǵ�� ������
        speed = backspeed;
        // ������ ��������� ���ڸ��� ������
        if (Vector3.Distance(transform.position, originPos.position) < 0.3f)
        {
            dir = Vector3.zero;
            transform.position = originPos.position;
            go = false;
            distance = 0;
            currnetTime = 0;
            trigger = false;
            end = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // �ֳʹ̷��̾�� ����� ��
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            trigger = true;
            Back();
        }
    }
}