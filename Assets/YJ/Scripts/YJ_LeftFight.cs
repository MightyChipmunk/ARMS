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
    float leftspeed = 10f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 15f;

    // Ÿ��
    GameObject target;
    GameObject player;


    // Ÿ����ġ
    Vector3 targetPos;

    Transform originPos;
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

    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;

        //print(Vector3.Distance(transform.position, player.transform.position));


        // ���� �������� ����
        leftOriginLocalPos = transform.localPosition;
        rightOriginLocalPos = right.transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        yj_trigger = trigger.GetComponent<YJ_Trigger>();
    }


    void Update()
    {
        // �� ȸ������ ī�޶� ����
        transform.localRotation = Camera.main.transform.localRotation;
        #region ������ (�ٹ�ưŬ��)
        // �ٹ�ư�� ������
        if (InputManager.Instance.Grap)
        {
            // �׷��� �Ѱ�
            grap = true;
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
        if (InputManager.Instance.Fire1 && !click && !overlap && !grap && !trigger.gameObject.activeSelf)
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
    void LeftFight()
    {        
        if (!click)
        {
            // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(transform.position, player.transform.position) > 10f)
            {
                //leftrg.velocity = Vector3.zero;
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
        if(!trigger.gameObject.activeSelf)
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
        leftspeed = 10f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }
    #endregion
    #region ������
    float timer;
    bool turn = false;
    void Grap()
    {
        // ������ Ÿ�ٹ�������
        Vector3 dir = targetPos - transform.position;
        // �޼հ� �������� �����δ�
        transform.position += dir * leftspeed * Time.deltaTime;
        right.transform.position += dir * leftspeed * Time.deltaTime;
        // ����� �÷��̾�� 10��ŭ �������ų� ��� ���� �ֳʹ̰� ������ 0.3�ʵ��� ���߱�
        if (Vector3.Distance(transform.position, player.transform.position) > 10f && Vector3.Distance(right.transform.position, player.transform.position) > 10f || yj_trigger.enemyCome)
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
            if (Vector3.Distance(transform.position, player.transform.position) > 2f && Vector3.Distance(right.transform.position, player.transform.position) > 2f)
            {
                // ��� �ҷ����� ( �ٷξձ������� ���� �� �������� �θ��� )
                transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
                right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
            }
            // �� �� ����������� �ƿ� ���÷� ��������
            if (Vector3.Distance(transform.position, player.transform.position) < 2.1f && Vector3.Distance(right.transform.position, player.transform.position) < 2.1f
                && Vector3.Distance(transform.position, player.transform.position) > 1.7f && Vector3.Distance(right.transform.position, player.transform.position) > 1.7f)
            {
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
            }
            // ������ ��������� ����
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f && Vector3.Distance(right.transform.position, player.transform.position) < 1.7f)
            {
                turn = false;
                grap = false;
                leftspeed = 10f;
            }
        }
    }
    #endregion
}

