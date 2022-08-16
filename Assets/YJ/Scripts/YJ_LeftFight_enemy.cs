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
    public GameObject right; // ������
    public GameObject trigger; // ��� ��
    public GameObject playertarget;
    // ���� �ӵ�
    float leftspeed = 15f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 20f;

    // Ÿ��
    GameObject target;
    GameObject me;
    GameObject player;


    // Ÿ����ġ
    Vector3 targetPos;

    Transform originPos;
    bool fire = false; // ����
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;
    bool overlap = false; // �÷��̾�� �������
    public bool grap = false; // ��� �ϰ��ִ���
    public bool Grapp
    {
        get { return grap; }
    }
    #endregion

    Vector3 dir;

    float leftTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� �� ����Ʈ
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;

    YJ_Trigger_enemy yj_trigger_enemy;
    YJ_RightFight_enemy yj_right;

    // �ݶ��̴� ���� �ѱ����� �ҷ�����
    SphereCollider leftCol;
    SphereCollider rightCol;

    // �������� ���� ���ϰ��ϱ�
    public YJ_Trigger yj_trigger;

    // �ʻ�� ���
    public YJ_KillerGage_enemy yj_KillerGage_enemy;

    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = playertarget;
        me = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        //originPos = target.transform;

        // ���� �������� ����
        leftOriginLocalPos = transform.localPosition;
        rightOriginLocalPos = right.transform.localPosition;

        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();

        yj_trigger_enemy = trigger.GetComponent<YJ_Trigger_enemy>();
        yj_right = right.GetComponent<YJ_RightFight_enemy>();

        // �ݶ��̴� ��������
        leftCol = GetComponent<SphereCollider>();
        rightCol = right.GetComponent<SphereCollider>();

    }
    bool graphands = false; //����������
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

        #region ���
        if (!fire && InputManager.Instance.EnemyGrap && !yj_right.fire && !grap && !yj_trigger.enemyCome)
        {
            // �׷��� �Ѱ�
            grap = true;
            graphands = true;
            // Ÿ���� ��ġ�� Trigger�� ��� �� �� �ֵ��� x�� �����Ͽ� ����
            targetPos = target.transform.position + new Vector3(1.23f, 0f, 0f);
            // Trigger Ȱ��ȭ
            trigger.gameObject.SetActive(true);
            yj_trigger_enemy.mr.enabled = true;
        }
        // ��Ⱑ ����Ǹ�
        if (grap)
        {
            // �ݶ��̴��� ����
            leftCol.enabled = false;
            rightCol.enabled = false;
            Grap();
        }
        #endregion

        if (overlap)
        {
            Return();

            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f)
            {
                print("�ȵ�����");
                transform.localPosition = leftOriginLocalPos;
                leftPath.Clear();
                overlap = false;
            }
        }

        #region ���ʰ���
        //print("overlap :" + overlap + " grap :" + grap + " fire :" + fire + " trigger :" + trigger.gameObject.activeSelf);
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (InputManager.Instance.EnemyFire1 && !overlap && !grap && !fire && !trigger.gameObject.activeSelf && !yj_trigger.enemyCome)
        {
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
            //print("���ƿԴ�?" + Vector3.Distance(transform.position, me.transform.position));
            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f)
            {
                fire = false;
                click = false;
                leftspeed = 15f;
                transform.localPosition = leftOriginLocalPos;
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
            //print("�Ÿ�" + Vector3.Distance(transform.position, me.transform.position));
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
        if (!trigger.gameObject.activeSelf)
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
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }

    float timer;
    bool turn = false;

    // ��������� ������ ����������
    Vector3 p1origin;
    Vector3 p1_left;
    Vector3 p1_right;
    Vector3 p2_left;
    Vector3 p2_right;
    public Transform p3_left;
    public Transform p3_right;
    float currentTime = 0;
    float positionNom = 0;
    void Grap()
    {
        if(graphands)
        {
            positionNom += leftspeed * Time.deltaTime;
            // ������ Ÿ�ٹ�������
            Vector3 dir = targetPos - transform.position;
            // �޼հ� �������� �����δ�
            transform.position += dir * 15f * Time.deltaTime;
            right.transform.position += dir * 15f * Time.deltaTime;

            // ����� �÷��̾�� 10��ŭ �������ų� ��� ���� �ֳʹ̰� ������ 0.3�ʵ��� ���߱�
            //if (Vector3.Distance(transform.position, me.transform.position) > 10f && Vector3.Distance(right.transform.position, me.transform.position) > 10f || yj_trigger.enemyCome)
            if (positionNom > 0.45f && positionNom > 0.45f || yj_trigger_enemy.enemyCome)
            {

                timer += Time.deltaTime;
                leftspeed = 0f;
                p1_left = transform.position;
                p2_left = transform.position + new Vector3(0, 6f, 0);
                p1_right = right.transform.position;
                p2_right = right.transform.position + new Vector3(0, 6f, 0);
                if (timer > 0.3f)
                {
                    turn = true;
                    graphands = false;
                }
        }
        }
        // �ٽ� �ǵ��ƿ���
        if (turn)
        {
            print(graphands);
            List<Vector3> list = new List<Vector3>();
            list.Clear();
            for (int i = 0; i < 100; i++)
            {
                Vector3 p = Go_left(0.01f * i);
                list.Add(p);
            }
            for (int i = 0; i < 99; i++)
            {
                Debug.DrawLine(list[i], list[i + 1], Color.red);
            }

            currentTime += Time.deltaTime;

            if (Vector3.Distance(transform.position, me.transform.position) > 2f && Vector3.Distance(right.transform.position, me.transform.position) > 2f)
            {
                transform.position = Go_left(currentTime * 1.5f);
                right.transform.position = Go_right(currentTime * 1.5f);
                // ��� �ҷ����� ( �ٷξձ������� ���� �� �������� �θ��� )
                //transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
                //right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
            }
            // �� �� ����������� �ƿ� ���÷� ��������
            if (Vector3.Distance(transform.position, me.transform.position) <= 2f && Vector3.Distance(right.transform.position, me.transform.position) <= 2f
                && Vector3.Distance(transform.position, me.transform.position) > 1.7f && Vector3.Distance(right.transform.position, me.transform.position) > 1.7f)
            {
                // �ݶ��̴��� �Ѱ�
                leftCol.enabled = true;
                rightCol.enabled = true;
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
            }
            // ������ ��������� ����
            if (Vector3.Distance(transform.position, me.transform.position) < 1.7f && Vector3.Distance(right.transform.position, me.transform.position) < 1.7f)
            {
                currentTime = 0;
                leftspeed = 15;
                positionNom = 0;
                turn = false;
                grap = false;
            }
        }
    }
    Vector3 Go_left(float ratio)
    {
        Vector3 pp1 = Vector3.Lerp(p1_left, p2_left, ratio);
        Vector3 pp2 = Vector3.Lerp(p2_left, p3_left.position, ratio);
        Vector3 ppp1 = Vector3.Lerp(pp1, pp2, ratio);

        return ppp1;
    }

    Vector3 Go_right(float ratio)
    {
        Vector3 pp1 = Vector3.Lerp(p1_right, p2_right, ratio);
        Vector3 pp2 = Vector3.Lerp(p2_right, p3_right.position, ratio);
        Vector3 ppp2 = Vector3.Lerp(pp1, pp2, ratio);

        return ppp2;
    }
}

