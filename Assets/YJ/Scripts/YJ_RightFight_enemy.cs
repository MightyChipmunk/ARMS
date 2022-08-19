using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
// 마우스의 이동방향을 가져와서 내 주먹을 움직이게 하고싶다.
// 마우스 이동방향 (공격버튼을 눌렀을때 포지션, 그 이후 포지션)
public class YJ_RightFight_enemy : MonoBehaviour
{
    public GameObject left;
    public GameObject targetCamera;
    public GameObject trigger;
    YJ_LeftFight_enemy leftFight;

    // 공격 속도
    float rightspeed = 15f;
    // 되돌아오는 속도
    float backspeed = 30f;

    // 타겟
    GameObject me;

    // 타겟위치
    Vector3 targetPos;

    // 내 콜라이더 끄고켜기
    Collider col;

    // 버튼 눌림확인
    public bool fire = false; // 오른쪽
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;

    // 애너미랑 닿았을때
    bool overlap = false;

    // 이동방향
    Vector3 dir;

    float rightTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> rightPath; // 위치가 들어갈 리스트
    Vector3 rightOriginLocalPos;

    public YJ_Trigger yj_trigger;

    // 필살기 사용 가능
    public YJ_KillerGage_enemy yj_KillerGage_enemy;

    // 돌아갈 곳
    public Transform originPos;

    // 내 트리거
    public YJ_Trigger_enemy yj_trigger_enemy;

    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        //target = GameObject.Find("Enemy");
        me = GameObject.Find("Enemy");



        // 이동 좌표를 저장할 리스트
        rightPath = new List<Vector3>();

        leftFight = left.GetComponent<YJ_LeftFight_enemy>();

        col = GetComponent<Collider>();
    }

    
    void Update()
    {
        if (yj_KillerGage_enemy.killerModeOn_enemy)
        {
            rightspeed = 60f;
            backspeed = 80f;
        }
        else
        {
            rightspeed = 15f;
            backspeed = 20f;
        }

        if (overlap)
        {
            //print("overlap 가동");
            Return();

            if(Vector3.Distance(transform.position, originPos.position) < 1.9f)
            {
                transform.position = originPos.position;
                rightPath.Clear();
                overlap = false;
            }
        }

        // 오른쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if ( InputManager.Instance.EnemyFire2 && !yj_trigger_enemy.grap )
        {
            // 로컬좌표의 값을 저장
            rightOriginLocalPos = transform.localPosition;
            col.enabled = true;
            targetPos = targetCamera.transform.position;
            fire = true;

        }
        if (fire)
        {
            rightFight();
        }
    }

    private void rightFight()
    {
        if (!click)
        {
            // 만약에 캐릭터로부터 n만큼 앞으로 갔다면 정지
            if (Vector3.Distance(transform.position, me.transform.position) > 10f)
            {
                //rightrg.velocity = Vector3.zero;
                rightspeed = 0f;
                click = true;
            }
            else
            {
                // 리스트에 움직이는 좌표 저장
                rightTime += Time.deltaTime;
                if (rightTime > 0.1f)
                {
                    rightPath.Add(transform.localPosition);
                    rightTime = 0f;
                }

                // 이동
                dir = targetPos - transform.position;
                dir.Normalize();


                transform.position += dir * rightspeed * Time.deltaTime;
                //transform.position += transform.TransformDirection(dir * rightspeed * Time.deltaTime);
            }
        }
        // 윤정언니 안뇽!! 내일은 좀 푹 자길 바랄게 >< 남은 시간도 화이팅!
        // 캐릭터로부터 n만큼 떨어졌다면
        if (click)
        {
            col.enabled = false;
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(transform.position, originPos.position) < 1.9f)
            {
                fire = false;
                click = false;
                rightspeed = 15f;
                transform.position = originPos.position;
            }

            // 다 되돌아오지 않았으면 되돌아오기
            else
            {
                if (rightPath.Count > 0)
                {
                    RightBack(rightPath.Count - 1); //rightPath를 뒤부터 불러주기
                }
                else
                {
                    RightBack(-1);
                }
            }
        }
    }

    private void RightBack(int f)
    {
        if (f != -1)
        {
            // 지금위치에서 리스트의 마지막 위치로 이동하기
            transform.localPosition = Vector3.Lerp(transform.localPosition, rightPath[f], Time.deltaTime * backspeed);

            if (Vector3.Distance(transform.localPosition, rightPath[f]) < 1f)
            {
                rightPath.RemoveAt(f); //리스트 끝번호부터 지워주기
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
            rightPath.Clear();
            if (Vector3.Distance(transform.localPosition, rightOriginLocalPos) < 0.05f)
            {
                transform.localPosition = rightOriginLocalPos;
                //currentTime = 0;

                fire = false;
            }
        }
    }

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
        rightspeed = 15f;
        transform.position = Vector3.Lerp(transform.position, originPos.position, Time.deltaTime * backspeed);
    }
}
