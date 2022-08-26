using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
// 마우스의 이동방향을 가져와서 내 주먹을 움직이게 하고싶다.
// 마우스 이동방향 (공격버튼을 눌렀을때 포지션, 그 이후 포지션)
public class YJ_LeftFight : YJ_Hand_left
{

    public GameObject trigger; // 가운데 선
    public GameObject enemyCamera;
    // 공격 속도
    float leftspeed = 15f;
    // 되돌아오는 속도
    float backspeed = 30f;

    // 타겟
    GameObject target;
    GameObject player;

    // 타겟위치
    Vector3 targetPos;

    // 왼쪽, 오른쪽버튼 눌림확인
    //bool fire = false; // 왼쪽

    bool click = false;
    bool overlap = false; // 애너미랑 닿았을때


    // 마우스 위치 (시작, 이후)
    Vector3 mouseOrigin;
    Vector3 mousePos;
    Vector3 dir;

    float leftTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> leftPath; // 위치가 들어갈 리스트
    Vector3 leftOriginLocalPos;

    YJ_Trigger yj_trigger;
    public YJ_Trigger_enemy yj_trigger_enemy;

    //콜라이더 끄고 켜기위해 불러오기
    Collider col;


    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip shoockSound; // 주먹 날아갈때 사운드

    Animation anim;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 타겟의 위치 찾기
        // 애너미의 처음위치로
        target = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        trigger = player.transform.Find("YJ_Trigger").gameObject;
        enemyCamera = GameObject.Find("EnemyAttackPos");
        // 콜라이더 가져오기
        col = GetComponent<Collider>();


        // 로컬 포지션을 저장
        leftOriginLocalPos = transform.localPosition;
        // 이동 좌표를 저장할 리스트
        leftPath = new List<Vector3>();
        mouseOrigin = Vector3.zero;

        yj_trigger = trigger.GetComponent<YJ_Trigger>();

        col.enabled = false;

        anim = GetComponent<Animation>();

        yj_KillerGage = GameObject.Find("KillerGage (2)").GetComponent<YJ_KillerGage>();
    }

    void Update()
    {
        if (yj_KillerGage.killerModeOn)
        {
            leftspeed = 60f;
            backspeed = 80f;
        }
        else
        {
            leftspeed = 15f;
            backspeed = 20f;
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
                anim.Play();
                // 상태종료
                overlap = false;
            }
        }
        #endregion
        #region 왼손공격 (왼쪽마우스클릭)
        // 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (InputManager.Instance.Fire1 && !click && !overlap && !yj_trigger.grap)// && !trigger.gameObject.activeSelf && !yj_trigger_enemy.enemyCome)
        {
            anim.Stop();
            audioSource.PlayOneShot(shoockSound);
            col.enabled = true;
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
    // 이동값 저장
    float leftDistance = 0;
    void LeftFight()
    {
        if (!click)
        {
            // 만약에 캐릭터로부터 n만큼 앞으로 갔다면 정지
            if (leftDistance > 9.2f)
            {
                //leftrg.velocity = Vector3.zero;
                leftspeed = 0f;
                click = true;
                leftDistance = 0;
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

                if (mousePos.x - mouseOrigin.x > 0)
                {
                    dir -= cross * 0.5f;
                }

                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    dir += cross * 0.5f;
                }
                transform.position += dir * leftspeed * Time.deltaTime;
                leftDistance += leftspeed * Time.deltaTime;
            }



        }
        // 캐릭터로부터 n만큼 떨어졌다면
        if (click)
        {
            col.enabled = false;
            // 다 되돌아왔으면 원점으로 만들기
            if (Vector3.Distance(transform.position, player.transform.position) < 1.7f)
            {
                // 콜라이더 켜주기
                col.enabled = true;

                leftspeed = 15f;
                transform.localPosition = leftOriginLocalPos;
                click = false;
                fire = false;
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
            anim.Play();
        }
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        // 잡기 상태가 아닐때
        if (!yj_trigger.grap)
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
        leftspeed = 15f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, leftOriginLocalPos, Time.deltaTime * backspeed);
    }
    #endregion
}
