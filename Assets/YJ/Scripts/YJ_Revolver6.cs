using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// isFire�� true�� targetPos�� ����ʹ�
public class YJ_Revolver6 : MonoBehaviour
{
    // ���� bool ��
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
    float speed = 5f;

    // �ǵ��ƿ��¼ӵ�
    float backspeed = 10f;
    bool go = false;

    // ���ƿ� ��
    public Transform originPos;

    // isFire�� ������ ��
    public YJ_RightRevolver rightRevolver;

    // ��߽ð�
    float currnetTime = 0;
    public float creatTime = 0;
    
    // �ݶ��̴� �Ѱ� ����
    Collider col;

    // �ʻ�� ���
    public YJ_KillerGage yj_KillerGage;

    // �ڿ� ������� ��ó�� �����
    

    void Start()
    {
        col = GetComponent<Collider>();
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
            speed = 5f;
            backspeed = 10f;
        }
        // isFire�� true�϶� 0.2�� �� ���ư���
        if (rightRevolver.isFire && !end)
        {
            // ���� �ݶ��̴� �ѱ�
            col.enabled = true;
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
            if(!trigger)
                dir = target - transform.position;
            if(trigger)
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            trigger = true;
            Back();
        }
    }
}
