using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Trigger_enemy : MonoBehaviour
{
    GameObject enemy;
    GameObject player;
    JH_PlayerMove jh_PlayerMove;
    CharacterController cc;
    public MeshRenderer mr;
    public bool enemyCome;
    public bool enemyGo;
    float backspeed = 20f;
    float currentTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        // �ֳʹ�, �÷��̾�
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        // ������ �̾��� ��
        jh_PlayerMove = player.GetComponent<JH_PlayerMove>();
        // �÷��̾� cc
        cc = player.GetComponent<CharacterController>();
        // ���ӿ�����Ʈ ���������ϱ�
        gameObject.SetActive(false);
        // ���� �� mr
        mr = GetComponent<MeshRenderer>();
    }

    #region 1. �������� �ֳʹ��ν�
    private void OnTriggerEnter(Collider other)
    {
        // ���� other�� �̸��� Player�ϰ���
        if (other.gameObject.name.Contains("Player"))
        {
            // �÷��̾ �����ϴ� ���� �ѱ�
            enemyCome = true;
        }
    }
    #endregion

    void Update()
    {
        #region 2. �÷��̾� ���ܿ���
        if (enemyCome)
        {
            // �÷��̾ ����ġ�� ��������
            player.transform.position = transform.position - new Vector3(0, 0.5f, 0);

            // �ֳʹ̿� �÷��̾��� �Ÿ��� 4 �����϶�
            if (Vector3.Distance(player.transform.position, enemy.transform.position) < 2f)
            {
                // �׸����ܿ���
                enemyCome = false;
                // �о���غ�
                enemyGo = true;
            }
        }
        #endregion
        #region �÷��̾� ���������� ���� �� �������ϱ�
        else if (!enemyCome && !enemyGo)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // 1�� �� ���ǵ� ����, ���� ������Ʈ ����
                currentTime = 0;
                gameObject.SetActive(false);
            }
        }
        #endregion
        #region 3. �÷��̾� ������
        if (enemyGo)
        {
            // ī�޶� �ݴ� ���⺸�� ���� ���� ���⼳��
            Vector3 dir = transform.forward + (Vector3.up * 1f);

            // CC�� ������ ���� ������
            if (cc.collisionFlags == CollisionFlags.Sides)
            {
                // y���� �����༭ ���������ϱ�
                dir.y -= 2f;
            }
            // �������� �����̰��ϱ�
            cc.Move(dir * 30f * Time.deltaTime);

            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // y���� 0.7 ���Ϸ� �������� (�ٴڿ� ���� ������)
                if (enemy.transform.position.y < 1f)
                {
                    // ���߰��ϱ�
                    backspeed = 0;
                    dir = Vector3.zero;
                    // mr ���� ���ֱ�
                    mr.enabled = false;

                    // 1�ʵ��� �������̰��ϱ�
                    currentTime += Time.deltaTime;
                    if (currentTime > 1f)
                    {
                        // 1�� �� ���ǵ� ����, ���� ������Ʈ ����
                        enemyGo = false;
                        backspeed = 20f;
                        currentTime = 0;
                        gameObject.SetActive(false);
                    }
                }
            }

        }
        #endregion
    }
}
