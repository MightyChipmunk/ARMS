using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Trigger : MonoBehaviour
{
    // 애너미랑 플레이어찾기
    GameObject enemy;
    GameObject player;

    // 공격할 위치
    public GameObject targetPos;
    Vector3 targetPosGet; // 타겟위치 잠깐 가져오기 음...

    // 애너미를 움직일 cc
    CharacterController cc;

    // 움직임 받아오기
    JH_PlayerMove jh_PlayerMove;

    // 목표위치
    Vector3 dir;

    // 원래위치
    public Transform triggerPos;

    // 끄고 킬 것
    MeshRenderer mr;
    Collider col;

    public bool grap = false; // 잡기상태확인
    public bool Grapp
    {
        get { return grap; }
    }

    public bool enemyCome = false; // 애너미 잡아올때
    public bool enemyGo = false; // 애너미 던질때
    bool turn = false;
    public bool goTrigger = false; // 트리거 앞으로가기
    bool backTrigger = false; // 트리거 혼자 뒤로오기

    // 손불러오기
    GameObject leftHand;
    GameObject rightHand;

    public GameObject spring;

    float currentTime = 0;
    float speed = 20f;
    public YJ_Trigger_enemy yj_trigger_enemy; // 애너미가 잡기상태인지 확인

    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip grapSound; // 잡기 날아갈때 사운드

    void Start()
    {
        // 애너미, 플레이어
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        rightHand = player.transform.Find("Right").gameObject;
        leftHand = player.transform.Find("Left").gameObject;

        // 무브와 이어질 것
        jh_PlayerMove = enemy.GetComponent<JH_PlayerMove>();

        // 애너미 cc
        cc = enemy.GetComponent<CharacterController>();

        // 안보이게 끄고시작
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        mr.enabled = false;
        col.enabled = false;
        spring.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        //print("enemycome : " + enemyCome + " enemygo : " + enemyGo + " grap : " + grap + " goTrigger : " + goTrigger + " backTrigger : " + backTrigger);
        #region 잡기공격 (휠버튼클릭)
        // 휠버튼을 누르면
        if (InputManager.Instance.Grap && !grap && !leftHand.GetComponent<YJ_Hand_left>().fire && !rightHand.GetComponent<YJ_Hand_right>().fire && !yj_trigger_enemy.goTrigger && !jh_PlayerMove.Knocked)
        {
            audioSource.PlayOneShot(grapSound);
            leftHand.GetComponent<Animation>().Stop();
            rightHand.GetComponent<Animation>().Stop();

            targetPosGet = targetPos.transform.position;

            // 콜라이더랑 매쉬 랜더러 켜주기
            //mr.enabled = true;
            spring.SetActive(true);
            col.enabled = true;

            // 잡기 시작 표시
            grap = true;
            goTrigger = true;

            // 자식으로 손들 불러오기
            leftHand.transform.SetParent(transform);
            rightHand.transform.SetParent(transform);

            leftHand.GetComponent<Collider>().enabled = false;
            rightHand.GetComponent<Collider>().enabled = false;

            
        }
        #endregion
        if (grap && !enemyCome && !enemyGo)
            transform.position += dir * speed * Time.deltaTime;

        // 움직임구현
        if (goTrigger)
        {
            dir = targetPosGet - transform.position;
            dir.Normalize();
            if (Vector3.Distance(transform.position, player.transform.position) > 10f)
            {
                // 콜라이더랑 매쉬 랜더러 끄기
                //mr.enabled = false;
                spring.SetActive(false);
                col.enabled = false;
                // 베지어곡선 그려주기
                p1 = transform.position;
                p2 = transform.position + new Vector3(0, 3f, 0);
                // 돌아오는 기능 켜기
                dir = Vector3.zero;

                timer += Time.deltaTime;

                if (timer > 0.2f)
                {
                    backTrigger = true;
                    goTrigger = false;
                }
            }
        }

        // 돌아오기
        if (backTrigger)
        {
            timer = 0;

            if (!enemyCome && !enemyGo)
            {
                // 베지어 곡선을 움직일 float 값
                currentTime += Time.deltaTime;

                // 베지어곡선의 도착구간 (점프 시 움직일 수 있기때문에 업데이트 해줘야함)
                p3 = triggerPos.position;
                transform.position = Go(currentTime * 1.5f);

                // 나와 플레이어의 거리가 2 이하일때
                if (Vector3.Distance(transform.position, triggerPos.position) < 2f)
                {
                    // 주먹의 콜라이더를 켜고
                    leftHand.GetComponent<Collider>().enabled = true;
                    rightHand.GetComponent<Collider>().enabled = true;

                    //dir = Vector3.zero;
                    // 내 위치를 원위치로 돌려놓고
                    transform.position = triggerPos.position;
                    // 플레이어의 자식으로 옮겨줌
                    leftHand.transform.SetParent(player.transform);
                    rightHand.transform.SetParent(player.transform);
                    leftHand.GetComponent<Animation>().Play();
                    rightHand.GetComponent<Animation>().Play();
                    currentTime = 0;
                    grap = false;
                    backTrigger = false;
                }
            }
        }


        #region 2. 애너미 당겨오기
        if (enemyCome)
        {
            Grap();
        }
        #endregion

        #region 3. 애너미 던지기
        if (enemyGo)
        {
            // 내 콜라이더에 부딪혀서 꺼주기
            col.enabled = false;

            // 카메라 방향보다 조금 높은 방향설정
            Vector3 go = transform.forward + (Vector3.up * 0.6f);

            // 방향으로 움직이게하기
            cc.Move(go * 30f * Time.deltaTime);

            currentTime += Time.deltaTime;
            if (currentTime > 0.2f)
            {
                // 플레이어와 애너미의 거리가 15이상이면 내려주기
                if (Vector3.Distance(player.transform.position, enemy.transform.position) > 10f || currentTime > 3f)
                {
                    // 주먹의 콜라이더를 켜고
                    leftHand.GetComponent<Collider>().enabled = true;
                    rightHand.GetComponent<Collider>().enabled = true;

                    // 플레이어의 자식으로 옮겨주기
                    leftHand.GetComponent<Animation>().Play();
                    rightHand.GetComponent<Animation>().Play();
                    leftHand.transform.SetParent(player.transform);
                    rightHand.transform.SetParent(player.transform);
                    // 멈추게하기
                    //backspeed = 0;
                    go = Vector3.zero;
                    // 안보이게 꺼주기
                    spring.SetActive(false);
                    //mr.enabled = false;
                    col.enabled = false;

                    currentTime = 0;
                    goTrigger = false;
                    grap = false;
                    enemyGo = false;

                }
            }

        }
        #endregion 
    }

    private void OnTriggerEnter(Collider other)
    {
        // 닿은 other의 이름이 Enemy일경우
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 애너미가 오게하는 기능 켜기
            graphands = true;
            enemyCome = true;
        }
    }

    // 베이지곡선을 실행할 로컬포지션
    Vector3 p1;
    Vector3 p2;
    Vector3 p3;
    bool graphands = false;
    float positionNom = 0;
    float timer;

    private void Grap()
    {
        if (graphands)
        {
            positionNom += speed * Time.deltaTime;
            List<Vector3> list = new List<Vector3>();
            list.Clear();
            for (int i = 0; i < 100; i++)
            {
                Vector3 p = Go(0.01f * i);
                list.Add(p);
            }
            for (int i = 0; i < 99; i++)
            {
                Debug.DrawLine(list[i], list[i + 1], Color.red);
            }
            if (positionNom > 0.45f || enemyCome)// || yj_trigger_enemy.enemyCome)
            {
                timer += Time.deltaTime;
                //speed = 0f;
                if (timer < 0.1f)
                {
                    p1 = transform.position;
                    p2 = transform.position + new Vector3(0, 3f, 0);

                    col.enabled = false;
                }

                if (timer > 0.3f)
                {
                    currentTime = 0;
                    positionNom = 0;
                    turn = true;
                    graphands = false;
                }
            }

        }
        // 애너미를 내위치로 끌어오기
        if (turn)
        {
            timer = 0;
            cc.enabled = false;
            enemy.transform.position = transform.position - new Vector3(0, 0.5f, 0);
            cc.enabled = true;
            currentTime += Time.deltaTime;

            // 베지어곡선의 도착구간 (점프 시 움직일 수 있기때문에 업데이트 해줘야함)
            p3 = triggerPos.position;

            transform.position = Go(currentTime * 1.5f);

            if (Vector3.Distance(transform.position, triggerPos.transform.position) < 2f)
            {
                transform.position = triggerPos.position;
            }

            // 애너미와 플레이어의 거리가 2 이하일때
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // 밀어내기준비
                enemyGo = true;
                currentTime = 0;
                turn = false;
                enemyCome = false;
            }
        }
    }

    Vector3 Go(float ratio)
    {
        Vector3 pp1 = Vector3.Lerp(p1, p2, ratio);
        Vector3 pp2 = Vector3.Lerp(p2, p3, ratio);
        Vector3 ppp1 = Vector3.Lerp(pp1, pp2, ratio);

        return ppp1;
    }
}
