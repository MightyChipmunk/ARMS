using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
// �ʿ��� : ���� (�ֳʹ� ��ġ) , �ӵ�
// ���콺�� �̵������� �����ͼ� �� �ָ��� �����̰� �ϰ�ʹ�.
// ���콺 �̵����� (���ݹ�ư�� �������� ������, �� ���� ������)
public class YJ_LeftFight : YJ_Hand_left
{

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
    //bool fire = false; // ����

    bool click = false;
    bool overlap = false; // �ֳʹ̶� �������


    // ���콺 ��ġ (����, ����)
    Vector3 mouseOrigin;
    Vector3 mousePos;
    Vector3 dir;

    float leftTime = 0.5f; // ��ǥ���� ī����
    [SerializeField] private List<Vector3> leftPath; // ��ġ�� �� ����Ʈ
    Vector3 leftOriginLocalPos;

    YJ_Trigger yj_trigger;
    public YJ_Trigger_enemy yj_trigger_enemy;

    //�ݶ��̴� ���� �ѱ����� �ҷ�����
    Collider col;


    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip shoockSound; // �ָ� ���ư��� ����

    Animation anim;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ÿ���� ��ġ ã��
        // �ֳʹ��� ó����ġ��
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        trigger = player.transform.Find("YJ_Trigger").gameObject;
        enemyCamera = GameObject.Find("EnemyAttackPos");
        // �ݶ��̴� ��������
        col = GetComponent<Collider>();


        // ���� �������� ����
        leftOriginLocalPos = transform.localPosition;
        // �̵� ��ǥ�� ������ ����Ʈ
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        yj_trigger = trigger.GetComponent<YJ_Trigger>();

        col.enabled = false;

        anim = GetComponent<Animation>();

        yj_KillerGage = GameObject.Find("KillerGage (2)").GetComponent<YJ_KillerGage>();
    }

    void Update()
    {
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
                anim.Play();
                // ��������
                overlap = false;
            }
        }
        #endregion
        #region �޼հ��� (���ʸ��콺Ŭ��)
        // ���� ���콺�� ������ �����Ÿ���ŭ �ֳʹ��� ó����ġ�� �̵��ϰ�ʹ�.
        if (InputManager.Instance.Fire1 && !click && !overlap && !yj_trigger.grap)// && !trigger.gameObject.activeSelf && !yj_trigger_enemy.enemyCome)
        {
            anim.Stop();
            audioSource.PlayOneShot(shoockSound);
            col.enabled = true;
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

                if (mousePos.x - mouseOrigin.x > 0)
                {
                    dir -= cross * 0.5f;
                }

                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    dir += cross * 0.5f;
                }
                transform.position += dir * leftspeed * Time.deltaTime;
                leftDistance += leftspeed * Time.deltaTime;
            }



        }
        // ĳ���ͷκ��� n��ŭ �������ٸ�
        if (click)
        {
            col.enabled = false;
            // �� �ǵ��ƿ����� �������� �����
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
            {
                // �ݶ��̴� ���ֱ�
                col.enabled = true;

                leftspeed = 15f;
                transform.localPosition = leftOriginLocalPos;
                click = false;
                fire = false;
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
            anim.Play();
        }
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        // ��� ���°� �ƴҶ�
        if (!yj_trigger.grap)
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
}
