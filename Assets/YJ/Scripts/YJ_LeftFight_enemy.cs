using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ��ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ��ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_LeftFight_enemy : MonoBehaviour
{
    #region ���ݰ���
    public GameObject right; // ������
    public GameObject trigger; // ��� ��
    public GameObject playertarget;
    // ���� �ӵ�
    float leftspeed = 10f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 15f;

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

    YJ_Trigger_enemy yj_trigger;
    YJ_RightFight_enemy yj_right;

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

        yj_trigger = trigger.GetComponent<YJ_Trigger_enemy>();
        yj_right = right.GetComponent<YJ_RightFight_enemy>();

    }

    void Update()
    {
        #region ���
        if (!fire && InputManager.Instance.EnemyGrap && !yj_right.fire)
        {
            // �׷��� �Ѱ�
            grap = true;
            // Ÿ���� ��ġ�� Trigger�� ��� �� �� �ֵ��� x�� �����Ͽ� ����
            targetPos = target.transform.position + new Vector3(1.23f, 0f, 0f);
            // Trigger Ȱ��ȭ
            trigger.gameObject.SetActive(true);
            yj_trigger.mr.enabled = true;
        }
        // ��Ⱑ ����Ǹ�
        if (grap)
        {
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
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ��ʹ�.
        if (InputManager.Instance.EnemyFire1 && !overlap && !grap && !fire && !trigger.gameObject.activeSelf)
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
                leftspeed = 10f;
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
        leftspeed = 10f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }

    float timer;
    bool turn = false;
    void Grap()
    {
        // ������ Ÿ�ٹ�������
        Vector3 dir = targetPos - transform.position;
        // �޼հ� �������� �����δ�
        transform.position += dir * leftspeed * Time.deltaTime;
        right.transform.position += dir * leftspeed * Time.deltaTime;
        // ����� �÷��̾�� 10��ŭ �������ų� ��� ������ �ֳʹ̰� ������ 0.3�ʵ��� ���߱�
        if (Vector3.Distance(transform.position, me.transform.position) > 10f && Vector3.Distance(right.transform.position, me.transform.position) > 10f || yj_trigger.enemyCome)
        {
            timer += Time.deltaTime;
            leftspeed = 0f;
            if (timer > 0.3f)
            {
                turn = true;
            }
        }
        // �ٽ� �ǵ��ƿ���
        if (turn)
        {
            if (Vector3.Distance(transform.position, me.transform.position) > 2f && Vector3.Distance(right.transform.position, me.transform.position) > 2f)
            {
                // ��� �ҷ����� ( �ٷξձ������� ���� �� �������� �θ��� )
                transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
                right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
            }
            // �� �� ����������� �ƿ� ���÷� ��������
            if (Vector3.Distance(transform.position, me.transform.position) < 2.1f && Vector3.Distance(right.transform.position, me.transform.position) < 2.1f
                && Vector3.Distance(transform.position, me.transform.position) > 1.7f && Vector3.Distance(right.transform.position, me.transform.position) > 1.7f)
            {
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
            }
            // ������ ��������� ����
            if (Vector3.Distance(transform.position, me.transform.position) < 1.7f && Vector3.Distance(right.transform.position, me.transform.position) < 1.7f)
            {
                turn = false;
                grap = false;
                leftspeed = 10f;
            }
        }
    }

}
