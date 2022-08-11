using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ�ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_RightFight : MonoBehaviour
{
    public GameObject left;
    public GameObject enemyCamera;
    public GameObject trigger;
    YJ_LeftFight leftFight;

    // ���� �ӵ�
    float rightspeed = 10f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 20f;

    // Ÿ��
    GameObject target;
    GameObject player;


    // Ÿ����ġ
    Vector3 targetPos;

    Transform originPos;
    // ��ư ����Ȯ��
    bool fire = false; // ������
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;

    // �ֳʹ̶� �������
    bool overlap = false;

    // ���콺 ��ġ (����, ����)
    Vector3 mouseOrigin;
    Vector3 mousePos;
    Vector3 dir;


    float rightTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> rightPath; // ��ġ�� �� ����Ʈ
    Vector3 rightOriginLocalPos;



    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;
        


        // ������ǥ�� ���� ����
        rightOriginLocalPos = transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        rightPath = new List<Vector3>();
        //mouseOrigin = Vector3.zero;

        leftFight = left.GetComponent<YJ_LeftFight>();

    }

    
    void Update()
    {
        transform.localRotation = Camera.main.transform.localRotation;

        if (overlap)
        {
            //print("�ֳʹ̴���");
            Return();
            if(Vector3.Distance(transform.position, player.transform.position) < 1.7f)
            {
                transform.localPosition = rightOriginLocalPos;
                rightPath.Clear();
                overlap = false;
            }
        }

        // ������ ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (InputManager.Instance.Fire2 && !click && !overlap && !leftFight.grap && !trigger.gameObject.activeSelf)
        {
            fire = true;
            mouseOrigin = Input.mousePosition;
            targetPos = enemyCamera.transform.position;
            
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
            if (Vector3.Distance(transform.position, player.transform.position) > 10f)
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

                Vector3 cross = Vector3.Cross(dir, transform.up);
                mousePos = Input.mousePosition;

                // ���콺�� �������� ���ϸ�
                if (mousePos.x - mouseOrigin.x > 0)
                {
                    dir -= cross * 0.5f;
                }

                // ���콺�� ������ ���ϸ�
                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    dir += cross * 0.5f;
                }
                transform.position += dir * rightspeed * Time.deltaTime;
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
                rightspeed = 10f;
                //isRightROnce = false;
                //isRightLOnce = false;
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
        }
    }

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

    void Return()
    {
        //rightrg.velocity = Vector3.zero;
        fire = false;
        click = false;
        rightspeed = 10f;
        //isRightROnce = false;
        //isRightLOnce = false;
        transform.localPosition = Vector3.Lerp(transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
        
    }
}
