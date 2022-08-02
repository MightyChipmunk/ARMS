using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
// 마우스의 이동방향을 가져와서 내 주먹을 움직이게 하고싶다.
// 마우스 이동방향 (공격버튼을 눌렀을때 포지션, 그 이후 포지션)
public class YJ_LeftFight : MonoBehaviour
{
    
    public GameObject right; // 오른손
    public GameObject trigger; // 가운데 선
    // 공격 속도
    float leftspeed = 10f;
    // 되돌아오는 속도
    float backspeed = 15f;

    // 타겟
    GameObject target;
    GameObject player;


    // 타겟위치
    Vector3 targetPos;

    Transform originPos;
    // 왼쪽, 오른쪽버튼 눌림확인
    bool fire = false; // 왼쪽
    public bool Fire
    {
        get { return fire; }
    }
    bool click = false;
    bool isLeftROnce; // 왼손이 오른쪽으로 휘었는지
    bool isLeftLOnce; // 왼손이 왼쪽으로 휘었는지
    bool overlap = false; // 애너미랑 닿았을때
    public bool grap = false; // 잡기 하고있는지
    public bool Grapp
    {
        get { return grap; }
    }

    // 마우스 위치 (시작, 이후)
    Vector3 mouseOrigin;
    Vector3 mousePos;

    // Rigidbody 불러오기
    Rigidbody leftrg;


    float leftTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> leftPath; // 위치가 들어갈 리스트
    Vector3 leftOriginLocalPos;
    Vector3 rightOriginLocalPos;

    YJ_Trigger yj_trigger;

    void Start()
    {
        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        originPos = player.transform;


        //Rigidbody 불러오기
        leftrg = GetComponent<Rigidbody>();


        // 로컬좌표의 값을 저장
        leftOriginLocalPos = transform.localPosition;
        // 이동 좌표를 저장할 리스트
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        yj_trigger = trigger.GetComponent<YJ_Trigger>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            grap = true;
            leftOriginLocalPos = transform.localPosition;
            rightOriginLocalPos = right.transform.localPosition;
            targetPos = target.transform.position + new Vector3(-1.23f, 0f, 0f);
            trigger.gameObject.SetActive(true);
            
        }
        if (grap)
        {
            Grap();
        }

        if (overlap)
        {
            Return();
            if (Vector3.Distance(transform.position, player.transform.position) < 1.45f)
            {
                transform.localPosition = leftOriginLocalPos;
                leftPath.Clear();
                overlap = false;
            }
        }
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (Input.GetMouseButtonDown(0) && !click && !overlap && !grap)
        {
            fire = true;
            mouseOrigin = Input.mousePosition;
            targetPos = target.transform.position;

        }
        if (fire)
        {
            LeftFight();
        }
    }
    void LeftFight()
    {
        if (!click)
        {
            // 만약에 캐릭터로부터 n만큼 앞으로 갔다면 정지
            if (Vector3.Distance(transform.position, player.transform.position) > 10f)
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
                    leftPath.Add(transform.localPosition);
                    leftTime = 0f;
                }

                // 이동
                Vector3 dir = targetPos - transform.position;
                dir.Normalize();
                transform.position += dir * leftspeed * Time.deltaTime;

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
            if (Vector3.Distance(transform.position, player.transform.position) < 1.45f)
            {
                print("리셋");
                fire = false;
                click = false;
                leftspeed = 10f;
                isLeftROnce = false;
                isLeftLOnce = false;
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
    }

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
            print(click);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Enemy") && !grap)
        {
            overlap = true;
        }
    }

    void Return()
    {
        leftrg.velocity = Vector3.zero;
        fire = false;
        click = false;
        leftspeed = 10f;
        isLeftROnce = false;
        isLeftLOnce = false;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);

    }

    float timer;
    bool turn = false;
    void Grap()
    {
        Vector3 dir = targetPos - transform.position;
        transform.position += dir * leftspeed * Time.deltaTime;
        right.transform.position += dir * leftspeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) > 10f && Vector3.Distance(right.transform.position, player.transform.position) > 10f || yj_trigger.enemyCome)
        {
            timer += Time.deltaTime;
            leftspeed = 0f;
            if (timer > 0.3f)
            {
                turn = true;
            }
        }
        if (turn)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
            right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);

            if(Vector3.Distance(transform.position, player.transform.position) < 1.45f && Vector3.Distance(right.transform.position, player.transform.position) < 1.45f)
            {
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
                turn = false;
                grap = false;
                leftspeed = 10f;
                //trigger.gameObject.SetActive(false);
            }
        }
    }

}

