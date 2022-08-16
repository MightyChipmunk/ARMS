using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
// 마우스의 이동방향을 가져와서 내 주먹을 움직이게 하고싶다.
// 마우스 이동방향 (공격버튼을 눌렀을때 포지션, 그 이후 포지션)
public class YJ_LeftFight_enemy : MonoBehaviour
{
    #region 공격관련
    public GameObject right; // 오른손
    public GameObject trigger; // 가운데 선
    public GameObject playertarget;
    // 공격 속도
    float leftspeed = 15f;
    // 되돌아오는 속도
    float backspeed = 20f;

    // 타겟
    GameObject target;
    GameObject me;
    GameObject player;


    // 타겟위치
    Vector3 targetPos;

    Transform originPos;
    bool fire = false; // 왼쪽
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;
    bool overlap = false; // 플레이어랑 닿았을때
    public bool grap = false; // 잡기 하고있는지
    public bool Grapp
    {
        get { return grap; }
    }
    #endregion

    Vector3 dir;

    float leftTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> leftPath; // 위치가 들어갈 리스트
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;

    YJ_Trigger_enemy yj_trigger_enemy;
    YJ_RightFight_enemy yj_right;

    // 콜라이더 끄고 켜기위해 불러오기
    SphereCollider leftCol;
    SphereCollider rightCol;

    // 잡혔을때 공격 못하게하기
    public YJ_Trigger yj_trigger;

    // 필살기 사용
    public YJ_KillerGage_enemy yj_KillerGage_enemy;

    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        target = playertarget;
        me = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        //originPos = target.transform;

        // 로컬 포지션을 저장
        leftOriginLocalPos = transform.localPosition;
        rightOriginLocalPos = right.transform.localPosition;

        // 이동 좌표를 저장할 리스트
        leftPath = new List<Vector3>();

        yj_trigger_enemy = trigger.GetComponent<YJ_Trigger_enemy>();
        yj_right = right.GetComponent<YJ_RightFight_enemy>();

        // 콜라이더 가져오기
        leftCol = GetComponent<SphereCollider>();
        rightCol = right.GetComponent<SphereCollider>();

    }
    bool graphands = false; //잡으러갈때
    void Update()
    {
        if (yj_KillerGage_enemy.killerModeOn_enemy)
        {
            leftspeed = 60f;
            backspeed = 80f;
        }
        else
        {
            leftspeed = 15f;
            backspeed = 20f;
        }

        #region 잡기
        if (!fire && InputManager.Instance.EnemyGrap && !yj_right.fire && !grap && !yj_trigger.enemyCome)
        {
            // 그랩을 켜고
            grap = true;
            graphands = true;
            // 타겟의 위치는 Trigger가 가운데 갈 수 있도록 x값 수정하여 지정
            targetPos = target.transform.position + new Vector3(1.23f, 0f, 0f);
            // Trigger 활성화
            trigger.gameObject.SetActive(true);
            yj_trigger_enemy.mr.enabled = true;
        }
        // 잡기가 실행되면
        if (grap)
        {
            // 콜라이더를 끄고
            leftCol.enabled = false;
            rightCol.enabled = false;
            Grap();
        }
        #endregion

        if (overlap)
        {
            Return();

            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f)
            {
                print("안들어오니");
                transform.localPosition = leftOriginLocalPos;
                leftPath.Clear();
                overlap = false;
            }
        }

        #region 왼쪽공격
        //print("overlap :" + overlap + " grap :" + grap + " fire :" + fire + " trigger :" + trigger.gameObject.activeSelf);
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (InputManager.Instance.EnemyFire1 && !overlap && !grap && !fire && !trigger.gameObject.activeSelf && !yj_trigger.enemyCome)
        {
            targetPos = playertarget.transform.position;
            fire = true;
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
            // 만약에 캐릭터로부터 n만큼 앞으로 갔다면 정지
            if (Vector3.Distance(transform.position, me.transform.position) > 10f)
            {
                leftspeed = 0f;
                click = true;
            }
            else
            {
                // 리스트에 움직이는 좌표 저장
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

        #region 멀리 떨어졌을때
        // 캐릭터로부터 n만큼 떨어졌다면
        if (click)
        {
            //print("돌아왔니?" + Vector3.Distance(transform.position, me.transform.position));
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f)
            {
                fire = false;
                click = false;
                leftspeed = 15f;
                transform.localPosition = leftOriginLocalPos;
            }

            // 다 되돌아오지 않았으면 되돌아오기
            else
            {
                if (leftPath.Count > 0)
                {
                    LeftBack(leftPath.Count - 1); //leftPath를 뒤부터 불러주기
                }
                else
                {
                    LeftBack(-1);
                }
            }
        }
        #endregion
    }

    #region 다시돌아가기
    void LeftBack(int i)
    {
        if (i != -1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftPath[i], Time.deltaTime * backspeed);

            if (Vector3.Distance(transform.localPosition, leftPath[i]) < 1f)
            {
                leftPath.RemoveAt(i); //리스트 끝번호부터 지워주기
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
            leftPath.Clear();
            //print("거리" + Vector3.Distance(transform.position, me.transform.position));
            if (Vector3.Distance(transform.localPosition, leftOriginLocalPos) < 0.05f)
            {
                transform.localPosition = leftOriginLocalPos;
                fire = false;
            }
        }

    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        // 잡기 상태가 아닐때
        if (!trigger.gameObject.activeSelf)
        {
            // 애너미레이어와 닿았을 때
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                overlap = true;
            }
        }
    }

    void Return()
    {
        fire = false;
        click = false;
        leftspeed = 15f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }

    float timer;
    bool turn = false;

    // 베이지곡선을 실행할 로컬포지션
    Vector3 p1origin;
    Vector3 p1_left;
    Vector3 p1_right;
    Vector3 p2_left;
    Vector3 p2_right;
    public Transform p3_left;
    public Transform p3_right;
    float currentTime = 0;
    float positionNom = 0;
    void Grap()
    {
        if(graphands)
        {
            positionNom += leftspeed * Time.deltaTime;
            // 방향은 타겟방향으로
            Vector3 dir = targetPos - transform.position;
            // 왼손과 오른손을 움직인다
            transform.position += dir * 15f * Time.deltaTime;
            right.transform.position += dir * 15f * Time.deltaTime;

            // 양손이 플레이어에서 10만큼 떨어지거나 가운데 고리에 애너미가 닿으면 0.3초동안 멈추기
            //if (Vector3.Distance(transform.position, me.transform.position) > 10f && Vector3.Distance(right.transform.position, me.transform.position) > 10f || yj_trigger.enemyCome)
            if (positionNom > 0.45f && positionNom > 0.45f || yj_trigger_enemy.enemyCome)
            {

                timer += Time.deltaTime;
                leftspeed = 0f;
                p1_left = transform.position;
                p2_left = transform.position + new Vector3(0, 6f, 0);
                p1_right = right.transform.position;
                p2_right = right.transform.position + new Vector3(0, 6f, 0);
                if (timer > 0.3f)
                {
                    turn = true;
                    graphands = false;
                }
        }
        }
        // 다시 되돌아오기
        if (turn)
        {
            print(graphands);
            List<Vector3> list = new List<Vector3>();
            list.Clear();
            for (int i = 0; i < 100; i++)
            {
                Vector3 p = Go_left(0.01f * i);
                list.Add(p);
            }
            for (int i = 0; i < 99; i++)
            {
                Debug.DrawLine(list[i], list[i + 1], Color.red);
            }

            currentTime += Time.deltaTime;

            if (Vector3.Distance(transform.position, me.transform.position) > 2f && Vector3.Distance(right.transform.position, me.transform.position) > 2f)
            {
                transform.position = Go_left(currentTime * 1.5f);
                right.transform.position = Go_right(currentTime * 1.5f);
                // 양손 불러오기 ( 바로앞까지말고 조금 더 앞쪽으로 부르기 )
                //transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
                //right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
            }
            // 좀 더 가까워졌을때 아예 로컬로 가져오기
            if (Vector3.Distance(transform.position, me.transform.position) <= 2f && Vector3.Distance(right.transform.position, me.transform.position) <= 2f
                && Vector3.Distance(transform.position, me.transform.position) > 1.7f && Vector3.Distance(right.transform.position, me.transform.position) > 1.7f)
            {
                // 콜라이더를 켜고
                leftCol.enabled = true;
                rightCol.enabled = true;
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
            }
            // 완전히 가까워지면 끄기
            if (Vector3.Distance(transform.position, me.transform.position) < 1.7f && Vector3.Distance(right.transform.position, me.transform.position) < 1.7f)
            {
                currentTime = 0;
                leftspeed = 15;
                positionNom = 0;
                turn = false;
                grap = false;
            }
        }
    }
    Vector3 Go_left(float ratio)
    {
        Vector3 pp1 = Vector3.Lerp(p1_left, p2_left, ratio);
        Vector3 pp2 = Vector3.Lerp(p2_left, p3_left.position, ratio);
        Vector3 ppp1 = Vector3.Lerp(pp1, pp2, ratio);

        return ppp1;
    }

    Vector3 Go_right(float ratio)
    {
        Vector3 pp1 = Vector3.Lerp(p1_right, p2_right, ratio);
        Vector3 pp2 = Vector3.Lerp(p2_right, p3_right.position, ratio);
        Vector3 ppp2 = Vector3.Lerp(pp1, pp2, ratio);

        return ppp2;
    }
}

