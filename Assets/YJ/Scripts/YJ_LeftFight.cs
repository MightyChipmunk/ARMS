using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ��ʹ�.
// �ʿ����� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ��ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_LeftFight : MonoBehaviour
{
    MeshRenderer mesh;
    Material mat;
    Renderer color;

    float currentTime;
    float creatTime = 1f;


    public GameObject right; // ������
    public GameObject trigger; // ��� ��
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
    bool isLeftROnce; // �޼��� ���������� �־�����
    bool isLeftLOnce; // �޼��� �������� �־�����
    bool overlap = false; // �ֳʹ̶� ��������
    public bool grap = false; // ���� �ϰ��ִ���
    public bool Grapp
    {
        get { return grap; }
    }

    // ���콺 ��ġ (����, ����)
    Vector3 mouseOrigin;
    Vector3 mousePos;

    // Rigidbody �ҷ�����
    Rigidbody leftrg;


    float leftTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� ��� ����Ʈ
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;

    YJ_Trigger yj_trigger;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
        color = GetComponent<Renderer>();

        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;

        print(Vector3.Distance(transform.position, player.transform.position));

        //Rigidbody �ҷ�����
        leftrg = GetComponent<Rigidbody>();


        // ������ǥ�� ���� ����
        leftOriginLocalPos = transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        yj_trigger = trigger.GetComponent<YJ_Trigger>();
    }


    void Update()
    {
        // "FŰ" ������ ��¡ ���� ����
        if (Input.GetKey(KeyCode.F))
        {
            currentTime += Time.deltaTime;

            if (currentTime > creatTime)
            {
                mat.color = new Color(0, 0, 1);  //-> ���� ĳ���� �ִϸŽü��� ���� Charging ����
                currentTime = 0;
                StopCoroutine("WaitForIt");
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                StartCoroutine("WaitForIt");
                currentTime = 0;
            }
        }


        if (Input.GetMouseButtonDown(2))
        {
            grap = true;
            leftOriginLocalPos = transform.localPosition;
            rightOriginLocalPos = right.transform.localPosition;
            targetPos = target.transform.position + new Vector3(-1.23f, 0f, 0f);
            trigger.gameObject.SetActive(true);

        }
        if (grap)
        {
            Grap();
        }

        if (overlap)
        {
            Return();
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
            {
                transform.localPosition = leftOriginLocalPos;
                leftPath.Clear();
                overlap = false;
            }
        }
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ��ʹ�.
        if (Input.GetMouseButtonDown(0) && !click && !overlap && !grap)
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
            // �� �ǵ��ƿ����� �������� ������
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
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
        if (other.gameObject.name.Contains("Enemy") && !grap)
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

    float timer;
    bool turn = false;
    void Grap()
    {
        Vector3 dir = targetPos - transform.position;
        transform.position += dir * leftspeed * Time.deltaTime;
        right.transform.position += dir * leftspeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) > 10f && Vector3.Distance(right.transform.position, player.transform.position) > 10f || yj_trigger.enemyCome)
        {
            timer += Time.deltaTime;
            leftspeed = 0f;
            if (timer > 0.3f)
            {
                turn = true;
            }
        }
        if (turn)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);

            if(Vector3.Distance(transform.position, player.transform.position) < 1.45f && Vector3.Distance(right.transform.position, player.transform.position) < 1.7f)
            {
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
                turn = false;
                grap = false;
                leftspeed = 10f;
                //trigger.gameObject.SetActive(false);
            }
        }
    }

    // 5�� �� ���� Ǯ��
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(5.0f);
        mat.color = new Color(1, 1, 1);
    }

}
