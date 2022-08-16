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
    public Transform triggerPos; // 되돌아올위치
    Vector3 dir; // 방향
    bool near; // 가까울때


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
        // 안보이게 시작하기
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();

        col.enabled = false;
        mr.enabled = false ;

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
        // 활성화
        col.enabled = true;
        mr.enabled = true;


        // 거리멀어지면 멈추고 되돌아오기
        if (Vector3.Distance(transform.position, player.transform.position) > 8f)
        {
            near = true;
            Near();
        }
        // 직진
        transform.position += dir * 15f * Time.deltaTime;


        //#region 애너미 못잡았을때 잠시 후 꺼지게하기
        //if (!enemyCome && !enemyGo)
        //{
        //    currentTime += Time.deltaTime;
        //    if (currentTime > 0.2f)
        //    {
        //        // 1초 후 스피드 복구, 게임 오브젝트 끄기
        //        currentTime = 0;
        //    }
        //}
        //#endregion

    }

    void Near()
    {
        if (near)
        {
            // 끄기
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
        // 닿은 other의 이름이 Enemy일경우
        if (other.gameObject.name.Contains("Enemy"))
        {
            // 애너미가 오게하는 기능 켜기
            enemyCome = true;
            Come();
        }
    }

    void Come()
    {
        if (enemyCome)
        {
            // 애너미를 내위치로 끌어오기
            enemy.transform.position = transform.position - new Vector3(0, 0.5f, 0);

            // 애너미와 플레이어의 거리가 2 이하일때
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // 밀어내기준비
                enemyGo = true;
                Go();
                // 그만당겨오고
                enemyCome = false;
            }
        }
    }

    void Go()
    {
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
                if (Vector3.Distance(player.transform.position, enemy.transform.position) > 15f)
                {
                    dir = Vector3.zero;
                    // 끄기
                    col.enabled = false;
                    mr.enabled = false;

                    // 1초동안 못움직이게하기
                    currentTime += Time.deltaTime;
                    if (currentTime > 1f)
                    {
                        // 1초 후 스피드 복구, 게임 오브젝트 끄기
                        enemyGo = false;
                        currentTime = 0;
                    }
                }
            }
        }
    }
}
