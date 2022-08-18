using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Trigger : MonoBehaviour
{
    // �ֳʹ̶� �÷��̾�ã��
    GameObject enemy;
    GameObject player;

    // ������ ��ġ
    public GameObject targetPos;
    Vector3 targetPosGet; // Ÿ����ġ ��� �������� ��...

    // �ֳʹ̸� ������ cc
    CharacterController cc;

    // ������ �޾ƿ���
    JH_PlayerMove jh_PlayerMove;

    // ��ǥ��ġ
    Vector3 dir;

    // ������ġ
    public Transform triggerPos;

    // ���� ų ��
    MeshRenderer mr;
    Collider col;

    public bool grap = false; // ������Ȯ��
    public bool Grapp
    {
        get { return grap; }
    }

    public bool enemyCome = false; // �ֳʹ� ��ƿö�
    public bool enemyGo = false; // �ֳʹ� ������
    bool turn = false;


    bool gograp = false; // ������ ����

    // �պҷ�����
    public GameObject leftHand;
    public GameObject rightHand;

    float currentTime = 0;
    float speed = 5f;
    public YJ_Trigger_enemy yj_trigger_enemy; // �ֳʹ̰� ���������� Ȯ��

    // Start is called before the first frame update
    void Start()
    {
        // �ֳʹ�, �÷��̾�
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");


        // ����� �̾��� ��
        jh_PlayerMove = enemy.GetComponent<JH_PlayerMove>();

        // �ֳʹ� cc
        cc = enemy.GetComponent<CharacterController>();

        // �Ⱥ��̰� �������
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        mr.enabled = false;
        col.enabled = false;
    }


    void Update()
    {

        print("enemycome : " + enemyCome + " enemygo : " + enemyGo + " grap : " + grap);
        #region ������ (�ٹ�ưŬ��)
        // �ٹ�ư�� ������
        if (Input.GetMouseButtonDown(2)) //InputManager.Instance.Grap) ;// && !yj_trigger_enemy.enemyCome)
        {
            targetPosGet = targetPos.transform.position;
            // �ݶ��̴��� �Ž� ������ ���ֱ�
            mr.enabled = true;
            col.enabled = true;

            // ��� ���� ǥ��
            grap = true;
            //graphands = true;

            // �ڽ����� �յ� �ҷ�����
            leftHand.transform.SetParent(transform);
            rightHand.transform.SetParent(transform);

            leftHand.GetComponent<Collider>().enabled = false;
            rightHand.GetComponent<Collider>().enabled = false;

        }
        #endregion

        // �����ӱ���
        if (grap)
        {
            dir = targetPosGet - transform.position;

            transform.position += dir * speed * Time.deltaTime;
        }


        #region 2. �ֳʹ� ��ܿ���
        if (enemyCome)
        {
            Grap();
        }
        #endregion

        #region 3. �ֳʹ� ������
        if (enemyGo)
        {
            // �� �ݶ��̴��� �ε����� ���ֱ�
            col.enabled = false;

            // ī�޶� ���⺸�� ���� ���� ���⼳��
            Vector3 go = transform.forward + (Vector3.up * 0.6f);

            // �������� �����̰��ϱ�
            cc.Move(go * 30f * Time.deltaTime);

            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // �÷��̾�� �ֳʹ��� �Ÿ��� 15�̻��̸� �����ֱ�
                if (Vector3.Distance(player.transform.position, enemy.transform.position) > 15f)
                {
                    leftHand.transform.SetParent(player.transform);
                    rightHand.transform.SetParent(player.transform);
                    // ���߰��ϱ�
                    //backspeed = 0;
                    go = Vector3.zero;
                    // �Ⱥ��̰� ���ֱ�
                    mr.enabled = false;
                    col.enabled = false;

                    currentTime = 0;
                    enemyGo = false;
                    grap = false;

                }
            }

        }
        #endregion 
    }
    #region 1. ������� �ֳʹ��ν�
    private void OnTriggerEnter(Collider other)
    {
        // ���� other�� �̸��� Enemy�ϰ��
        if (other.gameObject.name.Contains("Enemy"))
        {
            // �ֳʹ̰� �����ϴ� ��� �ѱ�
            grap = false;
            graphands = true;
            enemyCome = true;
        }
    }

    // ��������� ������ ����������
    Vector3 p1origin;
    Vector3 p1;
    Vector3 p2;
    Vector3 p3;
    bool graphands = false;
    float positionNom = 0;
    float timer;

    private void Grap()
    {
        if (graphands)
        {
            positionNom += speed * Time.deltaTime;
            List<Vector3> list = new List<Vector3>();
            list.Clear();
            for (int i = 0; i < 100; i++)
            {
                Vector3 p = Go(0.01f * i);
                list.Add(p);
            }
            for (int i = 0; i < 99; i++)
            {
                Debug.DrawLine(list[i], list[i + 1], Color.red);
            }
            if (positionNom > 0.45f || enemyCome)// || yj_trigger_enemy.enemyCome)
            {
                timer += Time.deltaTime;
                //speed = 0f;
                if (timer < 0.1f)
                {
                    p1 = transform.position;
                    p2 = transform.position + new Vector3(0, 6f, 0);
                    p3 = triggerPos.position;

                    col.enabled = false;
                }

                print("p1 :" + p1);
                print("p2 :" + p2);
                print("p3 :" + p3);

                if (timer > 0.3f)
                {
                    currentTime = 0;
                    positionNom = 0;
                    turn = true;
                    graphands = false;
                }
            }

        }
        // �ֳʹ̸� ����ġ�� �������
        if (turn)
        {
            timer = 0;
            cc.enabled = false;
            enemy.transform.position = transform.position - new Vector3(0, 0.5f, 0);
            cc.enabled = true;
            currentTime += Time.deltaTime;

            transform.position = Go(currentTime * 1.5f);

            if (Vector3.Distance(transform.position, triggerPos.transform.position) < 2f)
            {
                transform.position = triggerPos.position;
            }

            // �ֳʹ̿� �÷��̾��� �Ÿ��� 2 �����϶�
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // ���߰�
                //dir = Vector3.zero;
                p1origin = transform.position;
                // �о���غ�
                enemyGo = true;
                currentTime = 0;
                turn = false;
                enemyCome = false;
            }
        }
    }

    Vector3 Go(float ratio)
    {
        Vector3 pp1 = Vector3.Lerp(p1, p2, ratio);
        Vector3 pp2 = Vector3.Lerp(p2, p3, ratio);
        Vector3 ppp1 = Vector3.Lerp(pp1, pp2, ratio);

        return ppp1;
    }

    #endregion
    //    #region ������
    //    float timer;
    //    bool turn = false;

    //    // ��������� ������ ����������
    //    Vector3 p1origin;
    //    Vector3 p1_left;
    //    Vector3 p1_right;
    //    Vector3 p2_left;
    //    Vector3 p2_right;
    //    public Transform p3_left;
    //    public Transform p3_right;
    //    float currentTime = 0;
    //    float positionNom = 0;
    //    float grapspeed = 15f;
    //    void Grap()
    //    {
    //        if (graphands)
    //        {
    //            positionNom += leftspeed * Time.deltaTime;
    //            // ������ Ÿ�ٹ�������
    //            Vector3 dir = targetPos - transform.position;
    //            // �޼հ� �������� �����δ�
    //            transform.position += dir * grapspeed * Time.deltaTime;
    //            right.transform.position += dir * grapspeed * Time.deltaTime;

    //            // ����� �÷��̾�� 10��ŭ �������ų� ��� ���� �ֳʹ̰� ������ 0.3�ʵ��� ���߱�
    //            //if (Vector3.Distance(transform.position, player.transform.position) > 10f && Vector3.Distance(right.transform.position, player.transform.position) > 10f || yj_trigger.enemyCome)
    //            if (positionNom > 0.45f && positionNom > 0.45f || yj_trigger.enemyCome)
    //            {
    //                timer += Time.deltaTime;
    //                grapspeed = 0f;
    //                p1_left = transform.position;
    //                p2_left = transform.position + new Vector3(0, 6f, 0);
    //                p1_right = right.transform.position;
    //                p2_right = right.transform.position + new Vector3(0, 6f, 0);
    //                if (timer > 0.3f)
    //                {
    //                    turn = true;
    //                    graphands = false;
    //                }
    //            }
    //        }
    //        // �ٽ� �ǵ��ƿ���
    //        if (turn)
    //        {
    //            print(graphands);
    //            List<Vector3> list = new List<Vector3>();
    //            list.Clear();
    //            for (int i = 0; i < 100; i++)
    //            {
    //                Vector3 p = Go_left(0.01f * i);
    //                list.Add(p);
    //            }
    //            for (int i = 0; i < 99; i++)
    //            {
    //                Debug.DrawLine(list[i], list[i + 1], Color.red);
    //            }

    //            currentTime += Time.deltaTime;

    //            if (Vector3.Distance(transform.position, player.transform.position) > 2f && Vector3.Distance(right.transform.position, player.transform.position) > 2f)
    //            {
    //                transform.position = Go_left(currentTime * 1.5f);
    //                right.transform.position = Go_right(currentTime * 1.5f);
    //                // ��� �ҷ����� ( �ٷξձ������� ���� �� �������� �θ��� )
    //                //transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
    //                //right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
    //            }
    //            // �� �� ����������� �ƿ� ���÷� ��������
    //            if (Vector3.Distance(transform.position, player.transform.position) < 2.1f && Vector3.Distance(right.transform.position, player.transform.position) < 2.1f
    //                && Vector3.Distance(transform.position, player.transform.position) > 1.7f && Vector3.Distance(right.transform.position, player.transform.position) > 1.7f)
    //            {
    //                // �ݶ��̴��� �Ѱ�
    //                leftCol.enabled = true;
    //                rightCol.enabled = true;
    //                transform.localPosition = leftOriginLocalPos;
    //                right.transform.localPosition = rightOriginLocalPos;
    //                timer = 0;
    //            }
    //            // ������ ��������� ����
    //            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f && Vector3.Distance(right.transform.position, player.transform.position) < 1.7f)
    //            {
    //                currentTime = 0;
    //                grapspeed = 15;
    //                positionNom = 0;
    //                turn = false;
    //                grap = false;
    //            }
    //        }
    //    }
    //    #endregion

    //    

    //}

}
