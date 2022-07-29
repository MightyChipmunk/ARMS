using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
// 마우스의 이동방향을 가져와서 내 주먹을 움직이게 하고싶다.
// 마우스 이동방향 (공격버튼을 눌렀을때 포지션, 그 이후 포지션)
public class YH_PlayerFight : MonoBehaviour
{
    // 누가 움직일것인지
    public GameObject left;
    public GameObject right;

    // 공격 속도
    float leftspeed = 10f;
    float rightspeed = 10f;
    // 되돌아오는 속도
    float backspeed = 15f;

    // 타겟
    GameObject target;
    GameObject player;


    // 타겟위치
    Vector3 targetPos;

    Transform originPos;
    // 왼쪽, 오른쪽버튼 눌림확인
    bool fire1 = false; // 왼쪽
    bool click = false;
    bool fire2 = false; // 오른쪽
    bool click2 = false;

    // 마우스 위치 (시작, 이후)
    Vector3 mouseOrigin;
    Vector3 mousePos;

    // Rigidbody 불러오기
    Rigidbody leftrg;
    Rigidbody rightrg;


    bool isLeftROnce; // 왼손이 오른쪽으로 휘었는지
    bool isLeftLOnce; // 왼손이 왼쪽으로 휘었는지
    bool isRightROnce; // 오른손이 오른쪽으로 휘었는지
    bool isRightLOnce; // 오른손이 왼쪽으로 휘었는지

    float leftTime = 0.5f; // 좌표저장 카운터
    float rightTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> leftPath; // 위치가 들어갈 리스트
    [SerializeField] private List<Vector3> rightPath; // 위치가 들어갈 리스트
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;



    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;
        

        //Rigidbody 불러오기
        leftrg = left.GetComponent<Rigidbody>();
        rightrg = right.GetComponent<Rigidbody>();


        // 로컬좌표의 값을 저장
        leftOriginLocalPos = left.transform.localPosition;
        rightOriginLocalPos = right.transform.localPosition;
        leftPath = new List<Vector3>();
        rightPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

    }


    void Update()
    {
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (Input.GetMouseButtonDown(0) && !click)
        {
            fire1 = true;
            mouseOrigin = Input.mousePosition;
            targetPos = target.transform.position;

        }
        if (fire1)
        {
            LeftFight();
        }

        // 오른쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (Input.GetMouseButtonDown(1) && !click2)
        {
            fire2 = true;
            mouseOrigin = Input.mousePosition;
            targetPos = target.transform.position;

        }
        if (fire2)
        {
            rightFight();
        }
    }

    void LeftFight()
    {
        if (!click)
        {
            // 만약에 캐릭터로부터 n만큼 앞으로 갔다면 정지
            if (Vector3.Distance(left.transform.position, player.transform.position) > 10f)
            {
                leftrg.velocity = Vector3.zero;
                leftspeed = 0f;
                click = true;
            }
            else
            {
                // 리스트에 움직이는 좌표 저장
                leftTime += Time.deltaTime;
                if (leftTime > 0.1f)
                {
                    leftPath.Add(left.transform.localPosition);
                    leftTime = 0f;
                }

                // 이동
                Vector3 dir = targetPos - left.transform.position;
                dir.Normalize();
                left.transform.position += dir * leftspeed * Time.deltaTime;

                mousePos = Input.mousePosition;

                if (mousePos.x - mouseOrigin.x > 0)
                {
                    if (!isLeftROnce)
                    {
                        leftrg.velocity = Vector3.zero; // addforce정지시켜주기 ( 초기화 )
                        leftrg.AddForce(Vector3.right * 5, ForceMode.Impulse); // 초기화 이후 addforce
                        isLeftROnce = true; // 왼손 오른쪽으로 휨
                        isLeftLOnce = false; // 왼손 왼쪽으로 휘지 않음.
                    }
                }

                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    if (!isLeftLOnce)
                    {
                        leftrg.velocity = Vector3.zero; // addforce정지시켜주기 ( 초기화 )
                        leftrg.AddForce(Vector3.left * 5, ForceMode.Impulse); // 초기화 이후 addforce
                        isLeftLOnce = true; // 왼손 왼쪽으로 휨
                        isLeftROnce = false; // 왼손 오른쪽으로 휘지 않음.
                    }
                }
            }



        }
        // 캐릭터로부터 n만큼 떨어졌다면
        if (click)
        {
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(left.transform.position, player.transform.position) < 1.45f)
            {
                fire1 = false;
                click = false;
                leftspeed = 10f;
                isLeftROnce = false;
                isLeftLOnce = false;
                left.transform.localPosition = leftOriginLocalPos;
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
    }

    void LeftBack(int i)
    {
        if (i != -1)
        {
            left.transform.localPosition = Vector3.Lerp(left.transform.localPosition, leftPath[i], Time.deltaTime * backspeed);

            if (Vector3.Distance(left.transform.localPosition, leftPath[i]) < 1f)
            {
                leftPath.RemoveAt(i); //리스트 끝번호부터 지워주기
            }
        }
        else
        {
            left.transform.localPosition = Vector3.Lerp(left.transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
        }

    }

    private void rightFight()
    {
        if (!click2)
        {
            // 만약에 캐릭터로부터 n만큼 앞으로 갔다면 정지
            if (Vector3.Distance(right.transform.position, player.transform.position) > 10f)
            {
                rightrg.velocity = Vector3.zero;
                rightspeed = 0f;
                click2 = true;
            }
            else
            {
                // 리스트에 움직이는 좌표 저장
                rightTime += Time.deltaTime;
                if (rightTime > 0.1f)
                {
                    rightPath.Add(right.transform.localPosition);
                    rightTime = 0f;
                }

                // 이동
                Vector3 dir = targetPos - right.transform.position;
                dir.Normalize();
                right.transform.position += dir * leftspeed * Time.deltaTime;

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
        if (click2)
        {
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(right.transform.position, player.transform.position) < 1.45f)
            {
                fire2 = false;
                click2 = false;
                rightspeed = 10f;
                isRightROnce = false;
                isRightLOnce = false;
                right.transform.localPosition = rightOriginLocalPos;
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
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightPath[f], Time.deltaTime * backspeed);

            if (Vector3.Distance(right.transform.localPosition, rightPath[f]) < 1f)
            {
                rightPath.RemoveAt(f); //리스트 끝번호부터 지워주기
            }
        }
        else
        {
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
        }
    }
}
