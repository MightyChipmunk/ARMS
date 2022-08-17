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
    public GameObject enemyCamera;
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
    //bool isLeftROnce; // 왼손이 오른쪽으로 휘었는지
    //bool isLeftLOnce; // 왼손이 왼쪽으로 휘었는지
    bool overlap = false; // 애너미랑 닿았을때
    public bool grap = false; // 잡기 하고있는지
    public bool Grapp
    {
        get { return grap; }
    }

    // 마우스 위치 (시작, 이후)
    Vector3 mouseOrigin;
    Vector3 mousePos;
    Vector3 dir;



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

        //print(Vector3.Distance(transform.position, player.transform.position));


        // 로컬 포지션을 저장
        leftOriginLocalPos = transform.localPosition;
        rightOriginLocalPos = right.transform.localPosition;
        // 이동 좌표를 저장할 리스트
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        yj_trigger = trigger.GetComponent<YJ_Trigger>();
    }


    void Update()
    {
        // 내 회전값은 카메라를 따라감
        transform.localRotation = Camera.main.transform.localRotation;
        #region 잡기공격 (휠버튼클릭)
        // 휠버튼을 누르면
        if (Input.GetMouseButtonDown(2))
        {
            // 그랩을 켜고
            grap = true;
            // 타겟의 위치는 Trigger가 가운데 갈 수 있도록 x값 수정하여 지정
            targetPos = enemyCamera.transform.position + new Vector3(-1.23f, 0f, 0f);
            // Trigger 활성화
            trigger.gameObject.SetActive(true);
            yj_trigger.mr.enabled = true;
        }
        #endregion
        // 휠버튼을 눌렀다면
        if (grap)
        {
            Grap();
        }
        #region 주먹이 애너미에 닿았을때
        // 잡기상태가 아닌데 애너미랑 닿았을경우
        if (overlap)
        {
            // 되돌아오기
            Return();
            // 거의 다 돌아왔다면
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
            {
                // 오리진포스로 옮기기
                transform.localPosition = leftOriginLocalPos;
                // 저장된 Vector3 리스트 지우기
                leftPath.Clear();
                // 상태종료
                overlap = false;
            }
        }
        #endregion
        #region 왼손공격 (왼쪽마우스클릭)
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (Input.GetMouseButtonDown(0) && !click && !overlap && !grap && !trigger.gameObject.activeSelf)
        {
            fire = true;
            mouseOrigin = Input.mousePosition;
            targetPos = enemyCamera.transform.position;

        }
        if (fire)
        {
            LeftFight();
        }
        #endregion
    }
    #region 왼손공격
    void LeftFight()
    {        
        if (!click)
        {
            // 만약에 캐릭터로부터 n만큼 앞으로 갔다면 정지
            if (Vector3.Distance(transform.position, player.transform.position) > 10f)
            {
                //leftrg.velocity = Vector3.zero;
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
                dir = enemyCamera.transform.position - transform.position;
                dir.Normalize();

                mousePos = Input.mousePosition;
                // 외각
                Vector3 cross = Vector3.Cross(dir, transform.up);
                print(mousePos.x - mouseOrigin.x);

                if (mousePos.x - mouseOrigin.x > 0)
                {
                    //dir.x += 0.5f;
                    dir -= cross * 0.5f;
                }

                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    dir += cross * 0.5f;
                    //dir.x -= 0.5f;
                    //if (!isLeftLOnce)
                    //{
                    //leftrg.velocity = Vector3.zero; // addforce정지시켜주기 ( 초기화 )
                    //leftrg.AddForce(Vector3.left * 5, ForceMode.Impulse); // 초기화 이후 addforce
                    //isLeftLOnce = true; // 왼손 왼쪽으로 휨
                    //isLeftROnce = false; // 왼손 오른쪽으로 휘지 않음.
                    //}
                }
                transform.position += dir * leftspeed * Time.deltaTime;
            }



        }
        // 캐릭터로부터 n만큼 떨어졌다면
        if (click)
        {
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
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
    }
    #endregion
    #region 왼손공격 후 돌아오기
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
            //print(click);
        }
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        // 잡기 상태가 아닐때
        if(!trigger.gameObject.activeSelf)
        {
            // 애너미레이어와 닿았을 때
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                overlap = true;
            }
        }
    }
    #region 트리거 후 돌아오기
    void Return()
    {
        fire = false;
        click = false;
        leftspeed = 10f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }
    #endregion
    #region 잡기실행
    float timer;
    bool turn = false;
    void Grap()
    {
        // 방향은 타겟방향으로
        Vector3 dir = targetPos - transform.position;
        // 왼손과 오른손을 움직인다
        transform.position += dir * leftspeed * Time.deltaTime;
        right.transform.position += dir * leftspeed * Time.deltaTime;
        // 양손이 플레이어에서 10만큼 떨어지거나 가운데 고리에 애너미가 닿으면 0.3초동안 멈추기
        if (Vector3.Distance(transform.position, player.transform.position) > 10f && Vector3.Distance(right.transform.position, player.transform.position) > 10f || yj_trigger.enemyCome)
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
            if (Vector3.Distance(transform.position, player.transform.position) > 2f && Vector3.Distance(right.transform.position, player.transform.position) > 2f)
            {
                // 양손 불러오기 ( 바로앞까지말고 조금 더 앞쪽으로 부르기 )
                transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
                right.transform.localPosition = Vector3.Lerp(right.transform.localPosition, rightOriginLocalPos + new Vector3(0, 0, 0.5f), Time.deltaTime * 5f);
            }
            // 좀 더 가까워졌을때 아예 로컬로 가져오기
            if (Vector3.Distance(transform.position, player.transform.position) < 2.1f && Vector3.Distance(right.transform.position, player.transform.position) < 2.1f
                && Vector3.Distance(transform.position, player.transform.position) > 1.7f && Vector3.Distance(right.transform.position, player.transform.position) > 1.7f)
            {
                transform.localPosition = leftOriginLocalPos;
                right.transform.localPosition = rightOriginLocalPos;
                timer = 0;
            }
            // 완전히 가까워지면 끄기
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f && Vector3.Distance(right.transform.position, player.transform.position) < 1.7f)
            {
                turn = false;
                grap = false;
                leftspeed = 10f;
            }
        }
    }
    #endregion
}

