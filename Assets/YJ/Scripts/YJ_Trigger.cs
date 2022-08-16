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
    //float backspeed = 20f;
    float currentTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        // 애너미, 플레이어
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        // 무브와 이어질 것
        jh_PlayerMove = enemy.GetComponent<JH_PlayerMove>();
        // 애너미 cc
        cc = enemy.GetComponent<CharacterController>();
        // 게임오브젝트 끄고시작하기
        gameObject.SetActive(false);
        // 잠깐 끌 mr
        mr = GetComponent<MeshRenderer>();

    }
    #region 1. 닿았을때 애너미인식
    private void OnTriggerEnter(Collider other)
    {
        // 닿은 other의 이름이 Enemy일경우
        if (other.gameObject.name.Contains("Enemy"))
        {
            // 애너미가 오게하는 기능 켜기
            enemyCome = true;
        }
    }
    #endregion


    void Update()
    {
        #region 2. 애너미 당겨오기
        if (enemyCome)
        {
            // 애너미를 내위치로 끌어오기
            enemy.transform.position = transform.position - new Vector3(0, 0.5f, 0);

            // 애너미와 플레이어의 거리가 2 이하일때
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // 그만당겨오고
                enemyCome = false;
                // 밀어내기준비
                enemyGo = true;
            }
        }
        #endregion
        #region 애너미 못잡았을때 잠시 후 꺼지게하기
        else if (!enemyCome && !enemyGo)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // 1초 후 스피드 복구, 게임 오브젝트 끄기
                currentTime = 0;
                gameObject.SetActive(false);
            }
        }
        #endregion
        #region 3. 애너미 던지기
        if (enemyGo)
        {
            // 카메라 방향보다 조금 높은 방향설정
            Vector3 dir = transform.forward + (Vector3.up * 0.8f);

            // 방향으로 움직이게하기
            cc.Move(dir * 30f * Time.deltaTime);

            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // 플레이어와 애너미의 거리가 15이상이면 내려주기
                if (Vector3.Distance(player.transform.position,enemy.transform.position) > 15f)
                {
                    // 멈추게하기
                    //backspeed = 0;
                    dir = Vector3.zero;
                    // mr 잠깐 꺼주기
                    mr.enabled = false;

                    // 1초동안 못움직이게하기
                    currentTime += Time.deltaTime;
                    if (currentTime > 1f)
                    {
                        // 1초 후 스피드 복구, 게임 오브젝트 끄기
                        enemyGo = false;
                        currentTime = 0;
                        gameObject.SetActive(false);
                    }
                }
            }

        }
        #endregion
    }
}
