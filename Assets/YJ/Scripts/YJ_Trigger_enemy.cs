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
        // 애너미, 플레이어
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        // 무브와 이어질 것
        jh_PlayerMove = player.GetComponent<JH_PlayerMove>();
        // 플레이어 cc
        cc = player.GetComponent<CharacterController>();
        // 게임오브젝트 끄고시작하기
        gameObject.SetActive(false);
        // 잠깐 끌 mr
        mr = GetComponent<MeshRenderer>();
    }

    #region 1. 닿았을때 애너미인식
    private void OnTriggerEnter(Collider other)
    {
        // 닿은 other의 이름이 Player일경우
        if (other.gameObject.name.Contains("Player"))
        {
            // 플레이어가 오게하는 기능 켜기
            enemyCome = true;
        }
    }
    #endregion

    void Update()
    {
        #region 2. 플레이어 당겨오기
        if (enemyCome)
        {
            // 플레이어를 내위치로 끌어오기
            player.transform.position = transform.position - new Vector3(0, 0.5f, 0);

            // 애너미와 플레이어의 거리가 4 이하일때
            if (Vector3.Distance(player.transform.position, enemy.transform.position) < 2f)
            {
                // 그만당겨오고
                enemyCome = false;
                // 밀어내기준비
                enemyGo = true;
            }
        }
        #endregion
        #region 플레이어 못잡았을때 잠시 후 꺼지게하기
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
        #region 3. 플레이어 던지기
        if (enemyGo)
        {
            // 카메라 반대 방향보다 조금 높은 방향설정
            Vector3 dir = -Camera.main.transform.forward + (Vector3.up * 0.3f);

            // CC의 몸통이 벽에 닿으면
            if (cc.collisionFlags == CollisionFlags.Sides)
            {
                // y값을 더해줘서 떨어지게하기
                dir.y -= 0.5f;
            }
            // 방향으로 움직이게하기
            cc.Move(dir * backspeed * Time.deltaTime);

            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // y값이 0.7 이하로 떨어지면 (바닥에 거의 닿으면)
                if (enemy.transform.position.y < 1f)
                {
                    // 멈추게하기
                    backspeed = 0;
                    dir = Vector3.zero;
                    // mr 잠깐 꺼주기
                    mr.enabled = false;

                    // 1초동안 못움직이게하기
                    currentTime += Time.deltaTime;
                    if (currentTime > 1f)
                    {
                        // 1초 후 스피드 복구, 게임 오브젝트 끄기
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
