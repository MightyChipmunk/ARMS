using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
// 마우스의 이동방향을 가져와서 내 주먹을 움직이게 하고싶다.
// 마우스 이동방향 (공격버튼을 눌렀을때 포지션, 그 이후 포지션)
public class YJ_RightFight : MonoBehaviour
{
    public GameObject left;
    YJ_LeftFight leftFight;

    // 공격 속도
    float rightspeed = 10f;
    // 되돌아오는 속도
    float backspeed = 15f;

    // 타겟
    GameObject target;
    GameObject player;


    // 타겟위치
    Vector3 targetPos;

    Transform originPos;
    // 버튼 눌림확인
    bool fire = false; // 오른쪽
    bool click = false;

    // 애너미랑 닿았을때
    bool overlap = false;

    // 마우스 위치 (시작, 이후)
    Vector3 mouseOrigin;
    Vector3 mousePos;

    // Rigidbody 불러오기
    Rigidbody rightrg;

    bool isRightROnce; // 오른손이 오른쪽으로 휘었는지
    bool isRightLOnce; // 오른손이 왼쪽으로 휘었는지


    float rightTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> rightPath; // 위치가 들어갈 리스트
    Vector3 rightOriginLocalPos;



    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;
        

        //Rigidbody 불러오기
        rightrg = GetComponent<Rigidbody>();


        // 로컬좌표의 값을 저장
        rightOriginLocalPos = transform.localPosition;
        // 이동 좌표를 저장할 리스트
        rightPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        leftFight = left.GetComponent<YJ_LeftFight>();

    }

    
    void Update()
    {
        if(overlap)
        {
            Return();
            if(Vector3.Distance(transform.position, player.transform.position) < 1.45f)
            {
                transform.localPosition = rightOriginLocalPos;
                rightPath.Clear();
                overlap = false;
            }
        }

        // 오른쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (Input.GetMouseButtonDown(1) && !click && !overlap && !leftFight.grap)
        {
            fire = true;
            mouseOrigin = Input.mousePosition;
            targetPos = target.transform.position;

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
            if (Vector3.Distance(transform.position, player.transform.position) > 10f)
            {
                rightrg.velocity = Vector3.zero;
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
                Vector3 dir = targetPos - transform.position;
                dir.Normalize();
                transform.position += dir * rightspeed * Time.deltaTime;

                mousePos = Input.mousePosition;

                // 마우스가 오른쪽을 향하면
                if (mousePos.x - mouseOrigin.x > 0)
                {
                    if (!isRightROnce)
                    {
                        rightrg.velocity = Vector3.zero; // addforce정지시켜주기 ( 초기화 )
                        rightrg.AddForce(Vector3.right * 5, ForceMode.Impulse); // 초기화 이후 addforce
                        isRightROnce = true; // 왼손 오른쪽으로 휨
                        isRightLOnce = false; // 왼손 왼쪽으로 휘지 않음.
                    }
                }

                // 마우스가 왼쪽을 향하면
                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    if (!isRightLOnce)
                    {
                        rightrg.velocity = Vector3.zero; // addforce정지시켜주기 ( 초기화 )
                        rightrg.AddForce(Vector3.left * 5, ForceMode.Impulse); // 초기화 이후 addforce
                        isRightLOnce = true; // 오른손 왼쪽으로 휨
                        isRightROnce = false; // 오른손 오른쪽으로 휘지 않음.
                    }
                }
            }
        }
        // 캐릭터로부터 n만큼 떨어졌다면
        if (click)
        {
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(transform.position, player.transform.position) < 1.45f)
            {
                fire = false;
                click = false;
                rightspeed = 10f;
                isRightROnce = false;
                isRightLOnce = false;
                transform.localPosition = rightOriginLocalPos;
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Enemy"))
        {
            print("닿음 애너미랑");
            overlap = true;
        }
    }

    void Return()
    {
        rightrg.velocity = Vector3.zero;
        fire = false;
        click = false;
        rightspeed = 10f;
        isRightROnce = false;
        isRightLOnce = false;
        transform.localPosition = Vector3.Lerp(transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
        
    }
}
