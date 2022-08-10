using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Trigger : MonoBehaviour
{
    GameObject enemy;
    GameObject player;
    JH_PlayerMove jh_PlayerMove;
    CharacterController cc;
    public MeshRenderer mr;
    public bool enemyCome;
    public bool enemyGo;
    float backspeed = 15f;
    float currentTime = 0;


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
        // ���ӿ�����Ʈ ��������ϱ�
        gameObject.SetActive(false);
        // ��� �� mr
        mr = GetComponent<MeshRenderer>();

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
        #region 2. �ֳʹ� ��ܿ���
        if (enemyCome)
        {
            // �ֳʹ̸� ����ġ�� �������
            enemy.transform.position = transform.position - new Vector3(0,0.5f,0);

            // �ֳʹ̿� �÷��̾��� �Ÿ��� 2 �����϶�
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // �׸���ܿ���
                enemyCome = false;
                // �о���غ�
                enemyGo = true;
            }
        }
        #endregion
        #region �ֳʹ� ��������� ��� �� �������ϱ�
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
        #region 3. �ֳʹ� ������
        if (enemyGo)
        {
            // ī�޶� ���⺸�� ���� ���� ���⼳��
            Vector3 dir = Camera.main.transform.forward + (Vector3.up * 1f);

            // CC�� ������ ���� ������
            if(cc.collisionFlags == CollisionFlags.Sides)
            {
                // y���� �����༭ ���������ϱ�
                dir.y -= 0.5f;
            }
            // �������� �����̰��ϱ�
            cc.Move(dir * backspeed * Time.deltaTime);

            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // y���� 0.7 ���Ϸ� �������� (�ٴڿ� ���� ������)
                if ( enemy.transform.position.y < 1f )
                {
                    // ���߰��ϱ�
                    backspeed = 0;
                    dir = Vector3.zero;
                    // mr ��� ���ֱ�
                    mr.enabled = false;

                    // 1�ʵ��� �������̰��ϱ�
                    currentTime += Time.deltaTime;
                    if( currentTime > 1f )
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
