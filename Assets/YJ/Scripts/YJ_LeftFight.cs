using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ�ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_LeftFight : MonoBehaviour
{

    public GameObject right; // ������
    public GameObject trigger; // ��� ��
    public GameObject enemyCamera;
    // ���� �ӵ�
    float leftspeed = 15f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 30f;

    // Ÿ��
    GameObject target;
    GameObject player;

    // Ÿ����ġ
    Vector3 targetPos;

    // ����, �����ʹ�ư ����Ȯ��
    bool fire = false; // ����
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;
    //bool isLeftROnce; // �޼��� ���������� �־�����
    //bool isLeftLOnce; // �޼��� �������� �־�����
    bool overlap = false; // �ֳʹ̶� �������
    public bool grap = false; // ��� �ϰ��ִ���
    public bool Grapp
    {
        get { return grap; }
    }

    // ���콺 ��ġ (����, ����)
    Vector3 mouseOrigin;
    Vector3 mousePos;
    Vector3 dir;

    float leftTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� �� ����Ʈ
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;

    YJ_Trigger yj_trigger;
    public YJ_Trigger_enemy yj_trigger_enemy;

    //�ݶ��̴� ���� �ѱ����� �ҷ�����
    SphereCollider leftCol;
    SphereCollider rightCol;

    public YJ_KillerGage yj_KillerGage;

    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        // �ݶ��̴� ��������
        leftCol = GetComponent<SphereCollider>();
        rightCol = right.GetComponent<SphereCollider>();


        // ���� �������� ����
        leftOriginLocalPos = transform.localPosition;
        rightOriginLocalPos = right.transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        yj_trigger = trigger.GetComponent<YJ_Trigger>();
    }

    bool graphands = false; //����������
    void Update()
    {
        print(leftspeed);

        if (yj_KillerGage.killerModeOn)
        {
            leftspeed = 60f;
            backspeed = 80f;
        }
        else
        {
            leftspeed = 15f;
            backspeed = 20f;
        }



        // �� ȸ������ ī�޶� ����
        transform.localRotation = Camera.main.transform.localRotation;
        #region ������ (�ٹ�ưŬ��)
        // �ٹ�ư�� ������
        if (InputManager.Instance.Grap && !yj_trigger_enemy.enemyCome )
        {
            leftspeed = 15f;
            // �׷��� �Ѱ�
            grap = true;
            graphands = true;
            // Ÿ���� ��ġ�� Trigger�� ��� �� �� �ֵ��� x�� �����Ͽ� ����
            targetPos = enemyCamera.transform.position + new Vector3(-1.23f, 0f, 0f);
            // Trigger Ȱ��ȭ
            trigger.gameObject.SetActive(true);
            yj_trigger.mr.enabled = true;
        }
        #endregion
        // �ٹ�ư�� �����ٸ�
        if (grap)
        {
            // �ݶ��̴��� ����
            leftCol.enabled = false;
            rightCol.enabled = false;
            Grap();
        }
        #region �ָ��� �ֳʹ̿� �������
        // �����°� �ƴѵ� �ֳʹ̶� ��������
        if (overlap)
        {
            // �ǵ��ƿ���
            Return();

            // ���� �� ���ƿԴٸ�
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
            {
                // ������������ �ű��
                transform.localPosition = leftOriginLocalPos;
                // ����� Vector3 ����Ʈ �����
                leftPath.Clear();
                // ��������
                overlap = false;
            }
        }
        #endregion
        #region �޼հ��� (���ʸ��콺Ŭ��)
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (InputManager.Instance.Fire1 && !click && !overlap && !grap && !trigger.gameObject.activeSelf && !yj_trigger_enemy.enemyCome)
        {
            fire = true;
            mouseOrigin = Input.mousePosition;
            targetPos = enemyCamera.transform.position;

        }
        if (fire)
        {
            LeftFight();
        }
        #endregion
    }
    #region �޼հ���
    // �̵��� ����
    float leftDistance = 0;
    void LeftFight()
    {
        if (!click)
        {
            // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
            if (leftDistance > 9.2f)
            {
                //leftrg.velocity = Vector3.zero;
                leftspeed = 0f;
                click = true;
                leftDistance = 0;
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

                // �̵�
                dir = enemyCamera.transform.position - transform.position;
                dir.Normalize();

                mousePos = Input.mousePosition;
                // �ܰ�
                Vector3 cross = Vector3.Cross(dir, transform.up);
                print(mousePos.x - mouseOrigin.x);

                if (mousePos.x - mouseOrigin.x > 0)
                {
                    //dir.x += 0.5f;
                    dir -= cross * 0.5f;
                }

                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    dir += cross * 0.5f;
                    //dir.x -= 0.5f;
                    //if (!isLeftLOnce)
                    //{
                    //leftrg.velocity = Vector3.zero; // addforce���������ֱ� ( �ʱ�ȭ )
                    //leftrg.AddForce(Vector3.left * 5, ForceMode.Impulse); // �ʱ�ȭ ���� addforce
                    //isLeftLOnce = true; // �޼� �������� ��
                    //isLeftROnce = false; // �޼� ���������� ���� ����.
                    //}
                }
                transform.position += dir * leftspeed * Time.deltaTime;
                leftDistance += leftspeed * Time.deltaTime;
            }



        }
        // ĳ���ͷκ��� n��ŭ �������ٸ�
        if (click)
        {
            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
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
    }
    #endregion
    #region �޼հ��� �� ���ƿ���
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
            print(click);
        }
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        // ��� ���°� �ƴҶ�
        if (!trigger.gameObject.activeSelf)
        {
            // �ֳʹ̷��̾�� ����� ��
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                overlap = true;
            }
        }
    }
    #region Ʈ���� �� ���ƿ���
    void Return()
    {
        fire = false;
        click = false;
        leftspeed = 15f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }
    #endregion
    #region ������
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
    float grapspeed = 15f;
    void Grap()
    {
        if (graphands)
        {
            positionNom += leftspeed * Time.deltaTime;
            // ������ Ÿ�ٹ�������
            Vector3 dir = targetPos - transform.position;
            // �޼հ� �������� �����δ�
            transform.position += dir * grapspeed * Time.deltaTime;
            right.transform.position += dir * grapspeed * Time.deltaTime;

            // ����� �÷��̾�� 10��ŭ �������ų� ��� ���� �ֳʹ̰� ������ 0.3�ʵ��� ���߱�
            //if (Vector3.Distance(transform.position, player.transform.position) > 10f && Vector3.Distance(right.transform.position, player.transform.position) > 10f || yj_trigger.enemyCome)
            if (positionNom > 0.45f && positionNom > 0.45f || yj_trigger.enemyCome)
            {
                timer += Time.deltaTime;
                grapspeed = 0f;
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

            if (Vector3.Distance(transform.position, player.transform.position) > 2f && Vector3.Distance(right.transform.position, player.transform.position) > 2f)
            {
                transform.position = Go_left(currentTime * 1.5f);
                right.transform.position = Go_right(currentTime * 1.5f);
                // ��� �ҷ����� ( �ٷξձ������� ���� �� �������� �θ��� )
                //transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
                //right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
            }
            // �� �� ����������� �ƿ� ���÷� ��������
            if (Vector3.Distance(transform.position, player.transform.position) < 2.1f && Vector3.Distance(right.transform.position, player.transform.position) < 2.1f
                && Vector3.Distance(transform.position, player.transform.position) > 1.7f && Vector3.Distance(right.transform.position, player.transform.position) > 1.7f)
            {
                // �ݶ��̴��� �Ѱ�
                leftCol.enabled = true;
                rightCol.enabled = true;
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
            }
            // ������ ��������� ����
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f && Vector3.Distance(right.transform.position, player.transform.position) < 1.7f)
            {
                currentTime = 0;
                grapspeed = 15;
                positionNom = 0;
                turn = false;
                grap = false;
            }
        }
    }
    #endregion
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
    // ������ �޼��� ���Ʒ��� �Դٰ����ϸ� ������ʹ�.

}

