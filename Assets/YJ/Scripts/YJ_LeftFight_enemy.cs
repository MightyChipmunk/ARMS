using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ�ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_LeftFight_enemy : MonoBehaviour
{
    #region ���ݰ���
    public GameObject right; // ������
    public GameObject trigger; // ��� ��
    public GameObject playertarget;
    // ���� �ӵ�
    float leftspeed = 5f;
    // �ǵ��ƿ��� �ӵ�
    float backspeed = 15f;

    // Ÿ��
    GameObject target;
    GameObject me;
    GameObject player;


    // Ÿ����ġ
    Vector3 targetPos;

    Transform originPos;
    bool fire = false; // ����
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;
    bool overlap = false; // �÷��̾�� �������
    public bool grap = false; // ��� �ϰ��ִ���
    public bool Grapp
    {
        get { return grap; }
    }
    #endregion


    Vector3 dir;



    float leftTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� �� ����Ʈ
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;

    YJ_Trigger_enemy yj_trigger;
    YJ_RightFight_enemy yj_right;

    void Start()
    {
        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = playertarget;
        me = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        //originPos = target.transform;

        // ������ǥ�� ���� ����
        leftOriginLocalPos = transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();

        yj_trigger = trigger.GetComponent<YJ_Trigger_enemy>();
        yj_right = right.GetComponent<YJ_RightFight_enemy>();

    }

    float currentTime = 0;
    public int play = 0;
    void Update()
    {
        #region ���� ���� 3�ʸ��ٸ����
        if(!click && !fire && !grap && !overlap && !turn)
        {
            currentTime += Time.deltaTime;
            if(currentTime > 3)
            {
                play = UnityEngine.Random.Range(0, 10);
                //print(play);
            }
            //print(currentTime);
        }
        #endregion

        #region ���
        if (!fire && InputManager.Instance.EnemyGrap && !yj_right.fire)
        {
            if (play > 7)
            {
                grap = true;
                leftOriginLocalPos = transform.localPosition;
                print("��¥��" + leftOriginLocalPos);
                rightOriginLocalPos = right.transform.localPosition;
                targetPos = target.transform.position + new Vector3(1.23f, 0f, 0f);
                trigger.gameObject.SetActive(true);
                play = 0;
            }
        }
        if (grap)
        {
            Grap();
        }
        #endregion

        if (overlap)
        {
            Return();

            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f)
            {
                print("�ȵ�����");
                transform.localPosition = leftOriginLocalPos;
                leftPath.Clear();
                currentTime = 0;
                overlap = false;
            }
        }

        #region ���ʰ���
        //print("overlap :" + overlap + " grap :" + grap + " fire :" + fire + " trigger :" + trigger.gameObject.activeSelf);
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (!overlap && !grap && !fire && InputManager.Instance.EnemyFire1 && !trigger.gameObject.activeSelf)
        {
            targetPos = playertarget.transform.position;

            if(play > 0 && play < 8)
            {
                fire = true;
                play = 0;
            } 

        }
        if (fire)
        {
            LeftFight();
        }

        #endregion
    }

    void LeftFight()
    {
        if (!click)
        {
            // ���࿡ ĳ���ͷκ��� n��ŭ ������ ���ٸ� ����
            if (Vector3.Distance(transform.position, me.transform.position) > 10f)
            {
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

                dir = targetPos - transform.position;
                dir.Normalize();

                transform.position += dir * leftspeed * Time.deltaTime;
                
            }
        }

        #region �ָ� ����������
        // ĳ���ͷκ��� n��ŭ �������ٸ�
        if (click)
        {
            //print("���ƿԴ�?" + Vector3.Distance(transform.position, me.transform.position));
            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f)
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
        #endregion
    }

    #region �ٽõ��ư���
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
            //print("�Ÿ�" + Vector3.Distance(transform.position, me.transform.position));
            if(Vector3.Distance(transform.localPosition, leftOriginLocalPos) < 0.05f)
            {
                transform.localPosition = leftOriginLocalPos;
                currentTime = 0;
                
                fire = false;
            }
        }

    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!trigger.gameObject.activeSelf) //other.gameObject.name == "Player" && 
        {
            overlap = true;
        }
    }

    void Return()
    {
        fire = false;
        click = false;
        leftspeed = 10f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }

    float timer;
    bool turn = false;
    void Grap()
    {
        Vector3 dir = targetPos - transform.position;
        transform.position += dir * leftspeed * Time.deltaTime;
        right.transform.position += dir * leftspeed * Time.deltaTime;

        // ����� �÷��̾�� 10��ŭ �������ų� ��� ���� �ֳʹ̰� ������ 0.3�ʵ��� ���߱�
        if (Vector3.Distance(transform.position, me.transform.position) > 10f && Vector3.Distance(right.transform.position, me.transform.position) > 10f || yj_trigger.enemyCome)
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
            // ��� �ҷ�����
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);

            if (Vector3.Distance(trigger.transform.position, player.transform.position) > 5f &&!yj_trigger.enemyGo)
            {
                trigger.gameObject.SetActive(false);
            }
            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f && Vector3.Distance(right.transform.position, me.transform.position) < 1.9f)
            {
                print("�� ����ȵ���???");
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
                leftspeed = 10f;
                currentTime = 0;
                grap = false;
                turn = false;
            }
        }
    }

}

