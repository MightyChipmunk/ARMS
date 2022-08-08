using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ�ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_RightFight_enemy : MonoBehaviour
{
    public GameObject left;
    public GameObject targetCamera;
    public GameObject trigger;
    YJ_LeftFight_enemy leftFight;

    // ���� �ӵ�
    float rightspeed = 10f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 15f;

    // Ÿ��
    GameObject me;


    // Ÿ����ġ
    Vector3 targetPos;

    Transform originPos;
    // ��ư ����Ȯ��
    bool fire = false; // ������
    bool click = false;

    // �ֳʹ̶� �������
    bool overlap = false;

    bool BBBB = false;

    // �̵�����
    Vector3 dir;


    float rightTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> rightPath; // ��ġ�� �� ����Ʈ
    Vector3 rightOriginLocalPos;

    float currentTime = 0;
    int play = 0;

    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        //target = GameObject.Find("Enemy");
        me = GameObject.Find("Enemy");
        originPos = me.transform;
        


        // ������ǥ�� ���� ����
        rightOriginLocalPos = transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        rightPath = new List<Vector3>();

        leftFight = left.GetComponent<YJ_LeftFight_enemy>();
    }

    
    void Update()
    {

        #region ���� ���� 3�ʸ��ٸ����
        if (!fire  && !overlap)
        {
            currentTime += Time.deltaTime;
            //print(currentTime);
        }
        #endregion

        if (overlap)
        {
            print("overlap ����");
            Return();

            if(Vector3.Distance(transform.position, me.transform.position) < 1.9f)
            {
                transform.localPosition = rightOriginLocalPos;
                rightPath.Clear();
                currentTime = 0;
                overlap = false;
            }
        }

        // ������ ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (!click && !overlap && !leftFight.grap && !trigger.gameObject.activeSelf && InputManager.Instance.EnemyFire2)
        {
            targetPos = targetCamera.transform.position;
            fire = true;

        }
        if (fire)
        {
            rightFight();
        }
    }

    private void rightFight()
    {
        if (!click)
        {
            // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(transform.position, me.transform.position) > 10f)
            {
                //rightrg.velocity = Vector3.zero;
                rightspeed = 0f;
                click = true;
            }
            else
            {
                // ����Ʈ�� �����̴� ��ǥ ����
                rightTime += Time.deltaTime;
                if (rightTime > 0.1f)
                {
                    rightPath.Add(transform.localPosition);
                    rightTime = 0f;
                }

                // �̵�
                dir = targetPos - transform.position;
                dir.Normalize();


                transform.position += dir * rightspeed * Time.deltaTime;
                //transform.position += transform.TransformDirection(dir * rightspeed * Time.deltaTime);
            }
        }
        // ������� �ȴ�!! ������ �� ǫ �ڱ� �ٶ��� >< ���� �ð��� ȭ����!
        // ĳ���ͷκ��� n��ŭ �������ٸ�
        if (click)
        {
            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f)
            {
                fire = false;
                click = false;
                rightspeed = 10f;
                transform.localPosition = rightOriginLocalPos;
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, rightPath[f], Time.deltaTime * backspeed);

            if (Vector3.Distance(transform.localPosition, rightPath[f]) < 1f)
            {
                rightPath.RemoveAt(f); //����Ʈ ����ȣ���� �����ֱ�
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
            rightPath.Clear();
            print("�Ÿ�" + Vector3.Distance(transform.position, me.transform.position));
            if (Vector3.Distance(transform.localPosition, rightOriginLocalPos) < 0.05f)
            {
                transform.localPosition = rightOriginLocalPos;
                //currentTime = 0;

                fire = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player" && !trigger.gameObject.activeSelf)
        {
            overlap = true;
        }
    }

    void Return()
    {
        fire = false;
        click = false;
        rightspeed = 10f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
    }
}
