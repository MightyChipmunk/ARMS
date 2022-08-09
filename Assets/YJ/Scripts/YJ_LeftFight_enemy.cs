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
    float leftspeed = 5f;
    // 되돌아오는 속도
    float backspeed = 15f;

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

    YJ_Trigger_enemy yj_trigger;
    YJ_RightFight_enemy yj_right;

    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        target = playertarget;
        me = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        //originPos = target.transform;

        // 로컬좌표의 값을 저장
        leftOriginLocalPos = transform.localPosition;
        // 이동 좌표를 저장할 리스트
        leftPath = new List<Vector3>();

        yj_trigger = trigger.GetComponent<YJ_Trigger_enemy>();
        yj_right = right.GetComponent<YJ_RightFight_enemy>();

    }

    float currentTime = 0;
    public int play = 0;
    void Update()
    {
        #region 랜덤 숫자 3초마다만들기
        if(!click && !fire && !grap && !overlap && !turn)
        {
            currentTime += Time.deltaTime;
            if(currentTime > 3)
            {
                play = UnityEngine.Random.Range(0, 10);
                //print(play);
            }
            //print(currentTime);
        }
        #endregion

        #region 잡기
        if (!fire && InputManager.Instance.EnemyGrap && !yj_right.fire)
        {
            if (play > 7)
            {
                grap = true;
                leftOriginLocalPos = transform.localPosition;
                print("진짜값" + leftOriginLocalPos);
                rightOriginLocalPos = right.transform.localPosition;
                targetPos = target.transform.position + new Vector3(1.23f, 0f, 0f);
                trigger.gameObject.SetActive(true);
                play = 0;
            }
        }
        if (grap)
        {
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
                currentTime = 0;
                overlap = false;
            }
        }

        #region 왼쪽공격
        //print("overlap :" + overlap + " grap :" + grap + " fire :" + fire + " trigger :" + trigger.gameObject.activeSelf);
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (!overlap && !grap && !fire && InputManager.Instance.EnemyFire1 && !trigger.gameObject.activeSelf)
        {
            targetPos = playertarget.transform.position;

            if(play > 0 && play < 8)
            {
                fire = true;
                play = 0;
            } 

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
                leftspeed = 10f;
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
            if(Vector3.Distance(transform.localPosition, leftOriginLocalPos) < 0.05f)
            {
                transform.localPosition = leftOriginLocalPos;
                currentTime = 0;
                
                fire = false;
            }
        }

    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!trigger.gameObject.activeSelf) //other.gameObject.name == "Player" && 
        {
            overlap = true;
        }
    }

    void Return()
    {
        fire = false;
        click = false;
        leftspeed = 10f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }

    float timer;
    bool turn = false;
    void Grap()
    {
        Vector3 dir = targetPos - transform.position;
        transform.position += dir * leftspeed * Time.deltaTime;
        right.transform.position += dir * leftspeed * Time.deltaTime;

        // 양손이 플레이어에서 10만큼 떨어지거나 가운데 고리에 애너미가 닿으면 0.3초동안 멈추기
        if (Vector3.Distance(transform.position, me.transform.position) > 10f && Vector3.Distance(right.transform.position, me.transform.position) > 10f || yj_trigger.enemyCome)
        {
            timer += Time.deltaTime;
            leftspeed = 0f;
            if (timer > 0.3f)
            {
                turn = true;
            }
        }
        // 다시 되돌아오기
        if (turn)
        {
            // 양손 불러오기
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);

            if (Vector3.Distance(trigger.transform.position, player.transform.position) > 5f &&!yj_trigger.enemyGo)
            {
                trigger.gameObject.SetActive(false);
            }
            if (Vector3.Distance(transform.position, me.transform.position) < 1.9f && Vector3.Distance(right.transform.position, me.transform.position) < 1.9f)
            {
                print("야 여기안들어와???");
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
                leftspeed = 10f;
                currentTime = 0;
                grap = false;
                turn = false;
            }
        }
    }

}

