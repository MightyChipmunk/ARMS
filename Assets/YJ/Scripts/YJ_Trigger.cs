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


    bool gograp = false; // 앞으로 가기

    // 손불러오기
    public GameObject leftHand;
    public GameObject rightHand;

    float currentTime = 0;
    float speed = 5f;
    public YJ_Trigger_enemy yj_trigger_enemy; // 애너미가 잡기상태인지 확인

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

        // 안보이게 끄고시작
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        mr.enabled = false;
        col.enabled = false;
    }


    void Update()
    {

        print("enemycome : " + enemyCome + " enemygo : " + enemyGo + " grap : " + grap);
        #region 잡기공격 (휠버튼클릭)
        // 휠버튼을 누르면
        if (Input.GetMouseButtonDown(2)) //InputManager.Instance.Grap) ;// && !yj_trigger_enemy.enemyCome)
        {
            targetPosGet = targetPos.transform.position;
            // 콜라이더랑 매쉬 랜더러 켜주기
            mr.enabled = true;
            col.enabled = true;

            // 잡기 시작 표시
            grap = true;
            //graphands = true;

            // 자식으로 손들 불러오기
            leftHand.transform.SetParent(transform);
            rightHand.transform.SetParent(transform);

            leftHand.GetComponent<Collider>().enabled = false;
            rightHand.GetComponent<Collider>().enabled = false;

        }
        #endregion

        // 움직임구현
        if (grap)
        {
            dir = targetPosGet - transform.position;

            transform.position += dir * speed * Time.deltaTime;
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
                if (Vector3.Distance(player.transform.position, enemy.transform.position) > 15f)
                {
                    leftHand.transform.SetParent(player.transform);
                    rightHand.transform.SetParent(player.transform);
                    // 멈추게하기
                    //backspeed = 0;
                    go = Vector3.zero;
                    // 안보이게 꺼주기
                    mr.enabled = false;
                    col.enabled = false;

                    currentTime = 0;
                    enemyGo = false;
                    grap = false;

                }
            }

        }
        #endregion 
    }
    #region 1. 닿았을때 애너미인식
    private void OnTriggerEnter(Collider other)
    {
        // 닿은 other의 이름이 Enemy일경우
        if (other.gameObject.name.Contains("Enemy"))
        {
            // 애너미가 오게하는 기능 켜기
            grap = false;
            graphands = true;
            enemyCome = true;
        }
    }

    // 베이지곡선을 실행할 로컬포지션
    Vector3 p1origin;
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
                    p2 = transform.position + new Vector3(0, 6f, 0);
                    p3 = triggerPos.position;

                    col.enabled = false;
                }

                print("p1 :" + p1);
                print("p2 :" + p2);
                print("p3 :" + p3);

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

            transform.position = Go(currentTime * 1.5f);

            if (Vector3.Distance(transform.position, triggerPos.transform.position) < 2f)
            {
                transform.position = triggerPos.position;
            }

            // 애너미와 플레이어의 거리가 2 이하일때
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2f)
            {
                // 멈추고
                //dir = Vector3.zero;
                p1origin = transform.position;
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

    #endregion
    //    #region 잡기실행
    //    float timer;
    //    bool turn = false;

    //    // 베이지곡선을 실행할 로컬포지션
    //    Vector3 p1origin;
    //    Vector3 p1_left;
    //    Vector3 p1_right;
    //    Vector3 p2_left;
    //    Vector3 p2_right;
    //    public Transform p3_left;
    //    public Transform p3_right;
    //    float currentTime = 0;
    //    float positionNom = 0;
    //    float grapspeed = 15f;
    //    void Grap()
    //    {
    //        if (graphands)
    //        {
    //            positionNom += leftspeed * Time.deltaTime;
    //            // 방향은 타겟방향으로
    //            Vector3 dir = targetPos - transform.position;
    //            // 왼손과 오른손을 움직인다
    //            transform.position += dir * grapspeed * Time.deltaTime;
    //            right.transform.position += dir * grapspeed * Time.deltaTime;

    //            // 양손이 플레이어에서 10만큼 떨어지거나 가운데 고리에 애너미가 닿으면 0.3초동안 멈추기
    //            //if (Vector3.Distance(transform.position, player.transform.position) > 10f && Vector3.Distance(right.transform.position, player.transform.position) > 10f || yj_trigger.enemyCome)
    //            if (positionNom > 0.45f && positionNom > 0.45f || yj_trigger.enemyCome)
    //            {
    //                timer += Time.deltaTime;
    //                grapspeed = 0f;
    //                p1_left = transform.position;
    //                p2_left = transform.position + new Vector3(0, 6f, 0);
    //                p1_right = right.transform.position;
    //                p2_right = right.transform.position + new Vector3(0, 6f, 0);
    //                if (timer > 0.3f)
    //                {
    //                    turn = true;
    //                    graphands = false;
    //                }
    //            }
    //        }
    //        // 다시 되돌아오기
    //        if (turn)
    //        {
    //            print(graphands);
    //            List<Vector3> list = new List<Vector3>();
    //            list.Clear();
    //            for (int i = 0; i < 100; i++)
    //            {
    //                Vector3 p = Go_left(0.01f * i);
    //                list.Add(p);
    //            }
    //            for (int i = 0; i < 99; i++)
    //            {
    //                Debug.DrawLine(list[i], list[i + 1], Color.red);
    //            }

    //            currentTime += Time.deltaTime;

    //            if (Vector3.Distance(transform.position, player.transform.position) > 2f && Vector3.Distance(right.transform.position, player.transform.position) > 2f)
    //            {
    //                transform.position = Go_left(currentTime * 1.5f);
    //                right.transform.position = Go_right(currentTime * 1.5f);
    //                // 양손 불러오기 ( 바로앞까지말고 조금 더 앞쪽으로 부르기 )
    //                //transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
    //                //right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
    //            }
    //            // 좀 더 가까워졌을때 아예 로컬로 가져오기
    //            if (Vector3.Distance(transform.position, player.transform.position) < 2.1f && Vector3.Distance(right.transform.position, player.transform.position) < 2.1f
    //                && Vector3.Distance(transform.position, player.transform.position) > 1.7f && Vector3.Distance(right.transform.position, player.transform.position) > 1.7f)
    //            {
    //                // 콜라이더를 켜고
    //                leftCol.enabled = true;
    //                rightCol.enabled = true;
    //                transform.localPosition = leftOriginLocalPos;
    //                right.transform.localPosition = rightOriginLocalPos;
    //                timer = 0;
    //            }
    //            // 완전히 가까워지면 끄기
    //            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f && Vector3.Distance(right.transform.position, player.transform.position) < 1.7f)
    //            {
    //                currentTime = 0;
    //                grapspeed = 15;
    //                positionNom = 0;
    //                turn = false;
    //                grap = false;
    //            }
    //        }
    //    }
    //    #endregion

    //    

    //}

}
