using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ�ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_LeftFight_enemy : MonoBehaviour
{
    #region ���ݰ���
    public GameObject trigger; // ��� ��
    public GameObject playertarget;
    // ���� �ӵ�
    float leftspeed = 15f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 20f;

    // Ÿ��
    //GameObject target;
    GameObject me;
    GameObject player;


    // Ÿ����ġ
    Vector3 targetPos;

    bool fire = false; // ����
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;
    bool overlap = false; // �÷��̾�� �������

    #endregion

    Vector3 dir;

    // ����ġ
    public Transform originPos;

    float leftTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� �� ����Ʈ
    Vector3 leftOriginLocalPos;

    public YJ_Trigger_enemy yj_trigger_enemy;

    // �ݶ��̴� ���� �ѱ����� �ҷ�����
    Collider col;

    // �������� ���� ���ϰ��ϱ�
    public YJ_Trigger yj_trigger;

    // �ʻ�� ���
    public YJ_KillerGage_enemy yj_KillerGage_enemy;

    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        me = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        // �ݶ��̴� ��������
        col = GetComponent<Collider>();

        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();

        yj_trigger_enemy = trigger.GetComponent<YJ_Trigger_enemy>();

    }

    void Update()
    {
        if (yj_KillerGage_enemy.killerModeOn_enemy)
        {
            leftspeed = 60f;
            backspeed = 80f;
        }
        else
        {
            leftspeed = 15f;
            backspeed = 20f;
        }


        if (overlap)
        {
            // �ǵ��ƿ���
            Return();

            // ���� �� ���ƿԴٸ�
            if (Vector3.Distance(transform.position, originPos.position) < 1.9f)
            {
                // ������������ �ű��
                transform.position = originPos.position;
                // ����� Vector3 ����Ʈ �����
                leftPath.Clear();
                // ��������
                overlap = false;
            }
        }

        #region ���ʰ���
        //print("overlap :" + overlap + " grap :" + grap + " fire :" + fire + " trigger :" + trigger.gameObject.activeSelf);
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (InputManager.Instance.EnemyFire1 && !yj_trigger_enemy.grap)
        {
            print("�޼հ���-------------------------");
            col.enabled = true;
            targetPos = playertarget.transform.position;
            fire = true;
        }
        if (fire)
        {
            LeftFight();
        }

        #endregion
    }

    void LeftFight()
    {
        if (!click)
        {
            // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(transform.position, me.transform.position) > 10f)
            {
                leftspeed = 0f;
                click = true;
            }
            else
            {
                // ����Ʈ�� �����̴� ��ǥ ����
                leftTime += Time.deltaTime;
                if (leftTime > 0.1f)
                {
                    leftPath.Add(transform.localPosition);
                    leftTime = 0f;
                }

                dir = targetPos - transform.position;
                dir.Normalize();

                transform.position += dir * leftspeed * Time.deltaTime;

            }
        }

        #region �ָ� ����������
        // ĳ���ͷκ��� n��ŭ �������ٸ�
        if (click)
        {
            col.enabled = false;
            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(transform.position, originPos.position) < 1.7f)
            {
                fire = false;
                click = false;
                leftspeed = 15f;
                transform.position = originPos.position;
            }

            // �� �ǵ��ƿ��� �ʾ����� �ǵ��ƿ���
            else
            {
                if (leftPath.Count > 0)
                {
                    LeftBack(leftPath.Count - 1); //leftPath�� �ں��� �ҷ��ֱ�
                }
                else
                {
                    LeftBack(-1);
                }
            }
        }
        #endregion
    }

    #region �ٽõ��ư���
    void LeftBack(int i)
    {
        if (i != -1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftPath[i], Time.deltaTime * backspeed);

            if (Vector3.Distance(transform.localPosition, leftPath[i]) < 1f)
            {
                leftPath.RemoveAt(i); //����Ʈ ����ȣ���� �����ֱ�
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
            leftPath.Clear();

            if (Vector3.Distance(transform.localPosition, leftOriginLocalPos) < 0.05f)
            {
                transform.localPosition = leftOriginLocalPos;
                fire = false;
            }
        }

    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        // ��� ���°� �ƴҶ�
        if (!yj_trigger_enemy.grap)
        {
            // �ֳʹ̷��̾�� ����� ��
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                overlap = true;
            }
        }
    }

    void Return()
    {
        fire = false;
        click = false;
        leftspeed = 15f;
        transform.position = Vector3.Lerp(transform.position, originPos.position, Time.deltaTime * backspeed);
    }
}

