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
    public bool goTrigger = false; // Ʈ���� �����ΰ���
    bool backTrigger = false; // Ʈ���� ȥ�� �ڷο���

    // �պҷ�����
    GameObject leftHand;
    GameObject rightHand;

    public GameObject spring;

    float currentTime = 0;
    float speed = 20f;
    public YJ_Trigger_enemy yj_trigger_enemy; // �ֳʹ̰� ���������� Ȯ��

    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip grapSound; // ��� ���ư��� ����

    void Start()
    {
        // �ֳʹ�, �÷��̾�
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        rightHand = player.transform.Find("Right").gameObject;
        leftHand = player.transform.Find("Left").gameObject;

        // ����� �̾��� ��
        jh_PlayerMove = enemy.GetComponent<JH_PlayerMove>();

        // �ֳʹ� cc
        cc = enemy.GetComponent<CharacterController>();

        // �Ⱥ��̰� �������
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        mr.enabled = false;
        col.enabled = false;
        spring.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        //print("enemycome : " + enemyCome + " enemygo : " + enemyGo + " grap : " + grap + " goTrigger : " + goTrigger + " backTrigger : " + backTrigger);
        #region ������ (�ٹ�ưŬ��)
        // �ٹ�ư�� ������
        if (InputManager.Instance.Grap && !grap && !leftHand.GetComponent<YJ_Hand_left>().fire && !rightHand.GetComponent<YJ_Hand_right>().fire && !yj_trigger_enemy.goTrigger && !jh_PlayerMove.Knocked)
        {
            audioSource.PlayOneShot(grapSound);
            leftHand.GetComponent<Animation>().Stop();
            rightHand.GetComponent<Animation>().Stop();

            targetPosGet = targetPos.transform.position;

            // �ݶ��̴��� �Ž� ������ ���ֱ�
            //mr.enabled = true;
            spring.SetActive(true);
            col.enabled = true;

            // ��� ���� ǥ��
            grap = true;
            goTrigger = true;

            // �ڽ����� �յ� �ҷ�����
            leftHand.transform.SetParent(transform);
            rightHand.transform.SetParent(transform);

            leftHand.GetComponent<Collider>().enabled = false;
            rightHand.GetComponent<Collider>().enabled = false;

            
        }
        #endregion
        if (grap && !enemyCome && !enemyGo)
            transform.position += dir * speed * Time.deltaTime;

        // �����ӱ���
        if (goTrigger)
        {
            dir = targetPosGet - transform.position;
            dir.Normalize();
            if (Vector3.Distance(transform.position, player.transform.position) > 10f)
            {
                // �ݶ��̴��� �Ž� ������ ����
                //mr.enabled = false;
                spring.SetActive(false);
                col.enabled = false;
                // ������ �׷��ֱ�
                p1 = transform.position;
                p2 = transform.position + new Vector3(0, 3f, 0);
                // ���ƿ��� ��� �ѱ�
                dir = Vector3.zero;

                timer += Time.deltaTime;

                if (timer > 0.2f)
                {
                    backTrigger = true;
                    goTrigger = false;
                }
            }
        }

        // ���ƿ���
        if (backTrigger)
        {
            timer = 0;

            if (!enemyCome && !enemyGo)
            {
                // ������ ��� ������ float ��
                currentTime += Time.deltaTime;

                // �������� �������� (���� �� ������ �� �ֱ⶧���� ������Ʈ �������)
                p3 = triggerPos.position;
                transform.position = Go(currentTime * 1.5f);

                // ���� �÷��̾��� �Ÿ��� 2 �����϶�
                if (Vector3.Distance(transform.position, triggerPos.position) < 2f)
                {
                    // �ָ��� �ݶ��̴��� �Ѱ�
                    leftHand.GetComponent<Collider>().enabled = true;
                    rightHand.GetComponent<Collider>().enabled = true;

                    //dir = Vector3.zero;
                    // �� ��ġ�� ����ġ�� ��������
                    transform.position = triggerPos.position;
                    // �÷��̾��� �ڽ����� �Ű���
                    leftHand.transform.SetParent(player.transform);
                    rightHand.transform.SetParent(player.transform);
                    leftHand.GetComponent<Animation>().Play();
                    rightHand.GetComponent<Animation>().Play();
                    currentTime = 0;
                    grap = false;
                    backTrigger = false;
                }
            }
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
                if (Vector3.Distance(player.transform.position, enemy.transform.position) > 10f || currentTime > 3f)
                {
                    // �ָ��� �ݶ��̴��� �Ѱ�
                    leftHand.GetComponent<Collider>().enabled = true;
                    rightHand.GetComponent<Collider>().enabled = true;

                    // �÷��̾��� �ڽ����� �Ű��ֱ�
                    leftHand.GetComponent<Animation>().Play();
                    rightHand.GetComponent<Animation>().Play();
                    leftHand.transform.SetParent(player.transform);
                    rightHand.transform.SetParent(player.transform);
                    // ���߰��ϱ�
                    //backspeed = 0;
                    go = Vector3.zero;
                    // �Ⱥ��̰� ���ֱ�
                    spring.SetActive(false);
                    //mr.enabled = false;
                    col.enabled = false;

                    currentTime = 0;
                    goTrigger = false;
                    grap = false;
                    enemyGo = false;

                }
            }

        }
        #endregion 
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� other�� �̸��� Enemy�ϰ��
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // �ֳʹ̰� �����ϴ� ��� �ѱ�
            graphands = true;
            enemyCome = true;
        }
    }

    // ��������� ������ ����������
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
                    p2 = transform.position + new Vector3(0, 3f, 0);

                    col.enabled = false;
                }

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

            // �������� �������� (���� �� ������ �� �ֱ⶧���� ������Ʈ �������)
            p3 = triggerPos.position;

            transform.position = Go(currentTime * 1.5f);

            if (Vector3.Distance(transform.position, triggerPos.transform.position) < 2f)
            {
                transform.position = triggerPos.position;
            }

            // �ֳʹ̿� �÷��̾��� �Ÿ��� 2 �����϶�
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
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
}
