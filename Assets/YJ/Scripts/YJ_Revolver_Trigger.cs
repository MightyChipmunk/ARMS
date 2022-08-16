using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Revolver_Trigger : MonoBehaviour
{
    GameObject enemy;
    GameObject player;
    JH_PlayerMove jh_PlayerMove;
    CharacterController cc;
    MeshRenderer mr;
    Collider col;
    public bool enemyCome;
    public bool enemyGo;
    public bool grap;
    //float backspeed = 20f;
    float currentTime = 0;
    public Transform triggerPos; // �ǵ��ƿ���ġ
    Vector3 dir; // ����
    bool near; // ����ﶧ


    // Start is called before the first frame update
    void Start()
    {
        // �ֳʹ�, �÷��̾�
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        // ����� �̾��� ��
        jh_PlayerMove = enemy.GetComponent<JH_PlayerMove>();
        // �ֳʹ� cc
        cc = enemy.GetComponent<CharacterController>();
        // �Ⱥ��̰� �����ϱ�
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();

        col.enabled = false;
        mr.enabled = false ;

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
        if(Input.GetMouseButtonDown(2))
        {
            dir = transform.forward;
            grap = true;
            
        }
        if(grap)
        {
            Grap();
        }

    }

    void Grap()
    {
        // Ȱ��ȭ
        col.enabled = true;
        mr.enabled = true;


        // �Ÿ��־����� ���߰� �ǵ��ƿ���
        if (Vector3.Distance(transform.position, player.transform.position) > 8f)
        {
            near = true;
            Near();
        }
        // ����
        transform.position += dir * 15f * Time.deltaTime;


        //#region �ֳʹ� ��������� ��� �� �������ϱ�
        //if (!enemyCome && !enemyGo)
        //{
        //    currentTime += Time.deltaTime;
        //    if (currentTime > 0.2f)
        //    {
        //        // 1�� �� ���ǵ� ����, ���� ������Ʈ ����
        //        currentTime = 0;
        //    }
        //}
        //#endregion

    }

    void Near()
    {
        if (near)
        {
            // ����
            col.enabled = false;
            mr.enabled = false;
            dir = Vector3.zero;
            Vector3.Lerp(transform.position, triggerPos.position, Time.deltaTime * 20f);

            if (Vector3.Distance(transform.position, triggerPos.transform.position) < 3f)
            {
                dir = Vector3.zero;
                transform.position = triggerPos.position;
                grap = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���� other�� �̸��� Enemy�ϰ��
        if (other.gameObject.name.Contains("Enemy"))
        {
            // �ֳʹ̰� �����ϴ� ��� �ѱ�
            enemyCome = true;
            Come();
        }
    }

    void Come()
    {
        if (enemyCome)
        {
            // �ֳʹ̸� ����ġ�� �������
            enemy.transform.position = transform.position - new Vector3(0, 0.5f, 0);

            // �ֳʹ̿� �÷��̾��� �Ÿ��� 2 �����϶�
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // �о���غ�
                enemyGo = true;
                Go();
                // �׸���ܿ���
                enemyCome = false;
            }
        }
    }

    void Go()
    {
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
                if (Vector3.Distance(player.transform.position, enemy.transform.position) > 15f)
                {
                    dir = Vector3.zero;
                    // ����
                    col.enabled = false;
                    mr.enabled = false;

                    // 1�ʵ��� �������̰��ϱ�
                    currentTime += Time.deltaTime;
                    if (currentTime > 1f)
                    {
                        // 1�� �� ���ǵ� ����, ���� ������Ʈ ����
                        enemyGo = false;
                        currentTime = 0;
                    }
                }
            }
        }
    }
}
