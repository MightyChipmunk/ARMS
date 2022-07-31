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
    bool click = false;

    // �ֳʹ̶� �������
    bool overlap = false;

    // ���콺 ��ġ (����, ����)
    Vector3 mouseOrigin;
    Vector3 mousePos;

    // Rigidbody �ҷ�����
    Rigidbody leftrg;


    bool isLeftROnce; // �޼��� ���������� �־�����
    bool isLeftLOnce; // �޼��� �������� �־�����

    float leftTime = 0.5f; // ��ǥ���� ī����
    float rightTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� �� ����Ʈ
    Vector3 leftOriginLocalPos;



    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;


        //Rigidbody �ҷ�����
        leftrg = GetComponent<Rigidbody>();


        // ������ǥ�� ���� ����
        leftOriginLocalPos = transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

    }


    void Update()
    {
        if (overlap)
        {
            Return();
            if (Vector3.Distance(transform.position, player.transform.position) < 1.45f)
            {
                transform.localPosition = leftOriginLocalPos;
                leftPath.Clear();
                overlap = false;
            }
        }
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (Input.GetMouseButtonDown(0) && !click && !overlap)
        {
            fire = true;
            mouseOrigin = Input.mousePosition;
            targetPos = target.transform.position;

        }
        if (fire)
        {
            LeftFight();
        }
    }
        void LeftFight()
        {
            if (!click)
            {
                // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
                if (Vector3.Distance(transform.position, player.transform.position) > 10f)
                {
                    leftrg.velocity = Vector3.zero;
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
                    Vector3 dir = targetPos - transform.position;
                    dir.Normalize();
                    transform.position += dir * leftspeed * Time.deltaTime;

                    mousePos = Input.mousePosition;

                    if (mousePos.x - mouseOrigin.x > 0)
                    {
                        if (!isLeftROnce)
                        {
                            leftrg.velocity = Vector3.zero; // addforce���������ֱ� ( �ʱ�ȭ )
                            leftrg.AddForce(Vector3.right * 5, ForceMode.Impulse); // �ʱ�ȭ ���� addforce
                            isLeftROnce = true; // �޼� ���������� ��
                            isLeftLOnce = false; // �޼� �������� ���� ����.
                        }
                    }

                    else if (mousePos.x - mouseOrigin.x < 0)
                    {
                        if (!isLeftLOnce)
                        {
                            leftrg.velocity = Vector3.zero; // addforce���������ֱ� ( �ʱ�ȭ )
                            leftrg.AddForce(Vector3.left * 5, ForceMode.Impulse); // �ʱ�ȭ ���� addforce
                            isLeftLOnce = true; // �޼� �������� ��
                            isLeftROnce = false; // �޼� ���������� ���� ����.
                        }
                    }
                }



            }
            // ĳ���ͷκ��� n��ŭ �������ٸ�
            if (click)
            {
                // �� �ǵ��ƿ����� �������� �����
                if (Vector3.Distance(transform.position, player.transform.position) < 1.45f)
                {
                    print("����");
                    fire = false;
                    click = false;
                    leftspeed = 10f;
                    isLeftROnce = false;
                    isLeftLOnce = false;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("Enemy"))
            {
                overlap = true;
            }
        }

        void Return()
        {
            leftrg.velocity = Vector3.zero;
            fire = false;
            click = false;
            leftspeed = 10f;
            isLeftROnce = false;
            isLeftLOnce = false;
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);

        }
    
}

