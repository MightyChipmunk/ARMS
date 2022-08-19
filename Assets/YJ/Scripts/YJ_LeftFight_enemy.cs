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
    public GameObject trigger; // 가운데 선
    public GameObject playertarget;
    // 공격 속도
    float leftspeed = 15f;
    // 되돌아오는 속도
    float backspeed = 20f;

    // 타겟
    //GameObject target;
    GameObject me;
    GameObject player;


    // 타겟위치
    Vector3 targetPos;

    bool fire = false; // 왼쪽
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;
    bool overlap = false; // 플레이어랑 닿았을때

    #endregion

    Vector3 dir;

    // 원위치
    public Transform originPos;

    float leftTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> leftPath; // 위치가 들어갈 리스트
    Vector3 leftOriginLocalPos;

    public YJ_Trigger_enemy yj_trigger_enemy;

    // 콜라이더 끄고 켜기위해 불러오기
    Collider col;

    // 잡혔을때 공격 못하게하기
    public YJ_Trigger yj_trigger;

    // 필살기 사용
    public YJ_KillerGage_enemy yj_KillerGage_enemy;

    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        me = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        // 콜라이더 가져오기
        col = GetComponent<Collider>();

        // 이동 좌표를 저장할 리스트
        leftPath = new List<Vector3>();

        yj_trigger_enemy = trigger.GetComponent<YJ_Trigger_enemy>();

    }

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


        if (overlap)
        {
            // 되돌아오기
            Return();

            // 거의 다 돌아왔다면
            if (Vector3.Distance(transform.position, originPos.position) < 1.9f)
            {
                // 오리진포스로 옮기기
                transform.position = originPos.position;
                // 저장된 Vector3 리스트 지우기
                leftPath.Clear();
                // 상태종료
                overlap = false;
            }
        }

        #region 왼쪽공격
        //print("overlap :" + overlap + " grap :" + grap + " fire :" + fire + " trigger :" + trigger.gameObject.activeSelf);
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (InputManager.Instance.EnemyFire1 && !yj_trigger_enemy.grap)
        {
            print("왼손공격-------------------------");
            col.enabled = true;
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
            col.enabled = false;
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(transform.position, originPos.position) < 1.7f)
            {
                fire = false;
                click = false;
                leftspeed = 15f;
                transform.position = originPos.position;
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
        if (!yj_trigger_enemy.grap)
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
        transform.position = Vector3.Lerp(transform.position, originPos.position, Time.deltaTime * backspeed);
    }
}

