using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ�ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YH_PlayerFight : MonoBehaviour
{
    // ���� �����ϰ�����
    public GameObject left;
    public GameObject right;

    // ���� �ӵ�
    float leftspeed = 10f;
    float rightspeed = 10f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 15f;

    // Ÿ��
    GameObject target;
    GameObject player;


    // Ÿ����ġ
    Vector3 targetPos;

    Transform originPos;
    // ����, �����ʹ�ư ����Ȯ��
    bool fire1 = false; // ����
    bool click = false;
    bool fire2 = false; // ������
    bool click2 = false;

    // ���콺 ��ġ (����, ����)
    Vector3 mouseOrigin;
    Vector3 mousePos;

    // Rigidbody �ҷ�����
    Rigidbody leftrg;
    Rigidbody rightrg;


    bool isLeftROnce; // �޼��� ���������� �־�����
    bool isLeftLOnce; // �޼��� �������� �־�����
    bool isRightROnce; // �������� ���������� �־�����
    bool isRightLOnce; // �������� �������� �־�����

    float leftTime = 0.5f; // ��ǥ���� ī����
    float rightTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� �� ����Ʈ
    [SerializeField] private List<Vector3> rightPath; // ��ġ�� �� ����Ʈ
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;



    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;
        

        //Rigidbody �ҷ�����
        leftrg = left.GetComponent<Rigidbody>();
        rightrg = right.GetComponent<Rigidbody>();


        // ������ǥ�� ���� ����
        leftOriginLocalPos = left.transform.localPosition;
        rightOriginLocalPos = right.transform.localPosition;
        leftPath = new List<Vector3>();
        rightPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

    }


    void Update()
    {
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (Input.GetMouseButtonDown(0) && !click)
        {
            fire1 = true;
            mouseOrigin = Input.mousePosition;
            targetPos = target.transform.position;

        }
        if (fire1)
        {
            LeftFight();
        }

        // ������ ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (Input.GetMouseButtonDown(1) && !click2)
        {
            fire2 = true;
            mouseOrigin = Input.mousePosition;
            targetPos = target.transform.position;

        }
        if (fire2)
        {
            rightFight();
        }
    }

    void LeftFight()
    {
        if (!click)
        {
            // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(left.transform.position, player.transform.position) > 10f)
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
                    leftPath.Add(left.transform.localPosition);
                    leftTime = 0f;
                }

                // �̵�
                Vector3 dir = targetPos - left.transform.position;
                dir.Normalize();
                left.transform.position += dir * leftspeed * Time.deltaTime;

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
            if (Vector3.Distance(left.transform.position, player.transform.position) < 1.45f)
            {
                fire1 = false;
                click = false;
                leftspeed = 10f;
                isLeftROnce = false;
                isLeftLOnce = false;
                left.transform.localPosition = leftOriginLocalPos;
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
            left.transform.localPosition = Vector3.Lerp(left.transform.localPosition, leftPath[i], Time.deltaTime * backspeed);

            if (Vector3.Distance(left.transform.localPosition, leftPath[i]) < 1f)
            {
                leftPath.RemoveAt(i); //����Ʈ ����ȣ���� �����ֱ�
            }
        }
        else
        {
            left.transform.localPosition = Vector3.Lerp(left.transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
        }

    }

    private void rightFight()
    {
        if (!click2)
        {
            // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(right.transform.position, player.transform.position) > 10f)
            {
                rightrg.velocity = Vector3.zero;
                rightspeed = 0f;
                click2 = true;
            }
            else
            {
                // ����Ʈ�� �����̴� ��ǥ ����
                rightTime += Time.deltaTime;
                if (rightTime > 0.1f)
                {
                    rightPath.Add(right.transform.localPosition);
                    rightTime = 0f;
                }

                // �̵�
                Vector3 dir = targetPos - right.transform.position;
                dir.Normalize();
                right.transform.position += dir * leftspeed * Time.deltaTime;

                mousePos = Input.mousePosition;

                // ���콺�� �������� ���ϸ�
                if (mousePos.x - mouseOrigin.x > 0)
                {
                    if (!isRightROnce)
                    {
                        rightrg.velocity = Vector3.zero; // addforce���������ֱ� ( �ʱ�ȭ )
                        rightrg.AddForce(Vector3.right * 5, ForceMode.Impulse); // �ʱ�ȭ ���� addforce
                        isRightROnce = true; // �޼� ���������� ��
                        isRightLOnce = false; // �޼� �������� ���� ����.
                    }
                }

                // ���콺�� ������ ���ϸ�
                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    if (!isRightLOnce)
                    {
                        rightrg.velocity = Vector3.zero; // addforce���������ֱ� ( �ʱ�ȭ )
                        rightrg.AddForce(Vector3.left * 5, ForceMode.Impulse); // �ʱ�ȭ ���� addforce
                        isRightLOnce = true; // ������ �������� ��
                        isRightROnce = false; // ������ ���������� ���� ����.
                    }
                }
            }
        }
        // ĳ���ͷκ��� n��ŭ �������ٸ�
        if (click2)
        {
            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(right.transform.position, player.transform.position) < 1.45f)
            {
                fire2 = false;
                click2 = false;
                rightspeed = 10f;
                isRightROnce = false;
                isRightLOnce = false;
                right.transform.localPosition = rightOriginLocalPos;
            }
            // �� �ǵ��ƿ��� �ʾ����� �ǵ��ƿ���
            else
            {
                if (rightPath.Count > 0)
                {
                    RightBack(rightPath.Count - 1); //rightPath�� �ں��� �ҷ��ֱ�
                }
                else
                {
                    RightBack(-1);
                }
            }
        }
    }

    private void RightBack(int f)
    {
        if (f != -1)
        {
            // ������ġ���� ����Ʈ�� ������ ��ġ�� �̵��ϱ�
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightPath[f], Time.deltaTime * backspeed);

            if (Vector3.Distance(right.transform.localPosition, rightPath[f]) < 1f)
            {
                rightPath.RemoveAt(f); //����Ʈ ����ȣ���� �����ֱ�
            }
        }
        else
        {
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
        }
    }
}
