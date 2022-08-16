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

    // �ֳʹ̸� ������ cc
    CharacterController cc;

    // ������ �޾ƿ���
    JH_PlayerMove jh_PlayerMove;

    // ��ǥ��ġ
    Vector3 dir;

    // ���� ų ��
    MeshRenderer mr;
    Collider col;

    public bool grap = false; // ������Ȯ��
    public bool enemyCome = false; // �ֳʹ� ��ƿö�
    public bool enemyGo = false; // �ֳʹ� ������
    bool targetPosGet = false; // Ÿ����ġ ��� �������� ��...

    // �պҷ�����
    public GameObject leftHand;
    public GameObject rightHand;

    // �÷��̾� �ڽ����� ������ �� �÷��̾� Transform
    Transform player_trans;

    float currentTime = 0;
    float speed = 15f;
    public YJ_Trigger_enemy yj_trigger_enemy; // �ֳʹ̰� ���������� Ȯ��

    // Start is called before the first frame update
    void Start()
    {
        // �ֳʹ�, �÷��̾�
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        // �ڽ����� �־��ֱ����� Transform
        player_trans = player.transform;

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
    #region 1. ������� �ֳʹ��ν�
    private void OnTriggerEnter(Collider other)
    {
        // ���� other�� �̸��� Enemy�ϰ��
        if (other.gameObject.name.Contains("Enemy"))
        {
            // �ֳʹ̰� �����ϴ� ��� �ѱ�
            enemyCome = true;
        }
    }
    #endregion


    void Update()
    {
        #region ������ (�ٹ�ưŬ��)
        // �ٹ�ư�� ������
        if (Input.GetMouseButtonDown(2)) //InputManager.Instance.Grap) ;// && !yj_trigger_enemy.enemyCome)
        {
            // �ݶ��̴��� �Ž� ������ ���ֱ�
            mr.enabled = true;
            col.enabled = true;

            // ��� ���� ǥ��
            grap = true;

            // �ڽ����� �յ� �ҷ�����
            leftHand.transform.SetParent(transform);
            rightHand.transform.SetParent(transform);



            //graphands = true;
        }
        #endregion

        // �ٹ�ư�� �����ٸ�
        if (grap)
        {
            Vector3.Lerp(transform.position, targetPos.transform.position, speed * Time.deltaTime);
        }


        #region 2. �ֳʹ� ��ܿ���
        if (enemyCome)
        {
            // �ֳʹ̸� ����ġ�� �������
            enemy.transform.position = transform.position - new Vector3(0, 0.5f, 0);

            // �ֳʹ̿� �÷��̾��� �Ÿ��� 2 �����϶�
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // �׸���ܿ���
                enemyCome = false;
                // �о���غ�
                enemyGo = true;
            }
        }
        #endregion
        #region �ֳʹ� ��������� ��� �� �������ϱ�
        else if (!enemyCome && !enemyGo)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // 1�� �� ���ǵ� ����, ���� ������Ʈ ����
                currentTime = 0;
                //gameObject.SetActive(false);
            }
        }
        #endregion
        #region 3. �ֳʹ� ������
        if (enemyGo)
        {
            // ī�޶� ���⺸�� ���� ���� ���⼳��
            Vector3 dir = transform.forward + (Vector3.up * 0.8f);

            // �������� �����̰��ϱ�
            cc.Move(dir * 30f * Time.deltaTime);

            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // �÷��̾�� �ֳʹ��� �Ÿ��� 15�̻��̸� �����ֱ�
                if (Vector3.Distance(player.transform.position,enemy.transform.position) > 15f)
                {
                    // ���߰��ϱ�
                    //backspeed = 0;
                    dir = Vector3.zero;
                    // mr ��� ���ֱ�
                    mr.enabled = false;

                    // 1�ʵ��� �������̰��ϱ�
                    currentTime += Time.deltaTime;
                    if (currentTime > 1f)
                    {
                        // 1�� �� ���ǵ� ����, ���� ������Ʈ ����
                        enemyGo = false;
                        currentTime = 0;
                        //gameObject.SetActive(false);
                    }
                }
            }

        }
        #endregion
    }
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
//    Vector3 Go_left(float ratio)
//    {
//        Vector3 pp1 = Vector3.Lerp(p1_left, p2_left, ratio);
//        Vector3 pp2 = Vector3.Lerp(p2_left, p3_left.position, ratio);
//        Vector3 ppp1 = Vector3.Lerp(pp1, pp2, ratio);

//        return ppp1;
//    }

//    Vector3 Go_right(float ratio)
//    {
//        Vector3 pp1 = Vector3.Lerp(p1_right, p2_right, ratio);
//        Vector3 pp2 = Vector3.Lerp(p2_right, p3_right.position, ratio);
//        Vector3 ppp2 = Vector3.Lerp(pp1, pp2, ratio);

//        return ppp2;
//    }
//    

//}

}
