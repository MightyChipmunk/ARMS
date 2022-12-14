using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 왼쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
// 필요요소 : 방향 (애너미 위치) , 속도
// 마우스의 이동방향을 가져와서 내 주먹을 움직이게 하고싶다.
// 마우스 이동방향 (공격버튼을 눌렀을때 포지션, 그 이후 포지션)
public class YJ_RightFight : YJ_Hand_right
{
    //public GameObject left;
    public GameObject enemyCamera;
    public GameObject trigger;
    //YJ_LeftFight leftFight;
    public YJ_Trigger yj_trigger;

    // 공격 속도
    float rightspeed = 15f;
    // 되돌아오는 속도
    float backspeed = 20f;

    // 타겟
    GameObject target;
    GameObject player;


    // 타겟위치
    Vector3 targetPos;

    Transform originPos;
    // 버튼 눌림확인
    //bool fire = false; // 오른쪽

    bool click = false;

    // 애너미랑 닿았을때
    bool overlap = false;

    // 마우스 위치 (시작, 이후)
    Vector3 mouseOrigin;
    Vector3 mousePos;
    Vector3 dir;


    float rightTime = 0.5f; // 좌표저장 카운터
    [SerializeField] private List<Vector3> rightPath; // 위치가 들어갈 리스트
    Vector3 rightOriginLocalPos;

    // 필살기 사용가능

    public YJ_Trigger_enemy yj_trigger_enemy;

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
        originPos = player.transform;
        trigger = player.transform.Find("YJ_Trigger").gameObject;
        enemyCamera = GameObject.Find("EnemyAttackPos");
        yj_trigger = trigger.GetComponent<YJ_Trigger>();

        // 로컬좌표의 값을 저장
        rightOriginLocalPos = transform.localPosition;
        // 이동 좌표를 저장할 리스트
        rightPath = new List<Vector3>();
        //mouseOrigin = Vector3.zero;

        //leftFight = left.GetComponent<YJ_LeftFight>();
        col = GetComponent<Collider>();

        col.enabled = false;

        anim = GetComponent<Animation>();

        yj_KillerGage = GameObject.Find("KillerGage (2)").GetComponent<YJ_KillerGage>();
    }

    
    void Update()
    {
       
        if( yj_KillerGage.killerModeOn )
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
            //print("애너미닿음");
            Return();
            if(Vector3.Distance(transform.position, player.transform.position) < 1.7f)
            {
                transform.localPosition = rightOriginLocalPos;
                rightPath.Clear();
                anim.Play();
                overlap = false;
            }
        }

        // 오른쪽 마우스를 누르면 일정거리만큼 애너미의 처음위치에 이동하고싶다.
        if (InputManager.Instance.Fire2 && !click && !overlap && !yj_trigger.grap)// && !trigger.gameObject.activeSelf && !yj_trigger_enemy.enemyCome)
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

                Vector3 cross = Vector3.Cross(dir, transform.up);
                mousePos = Input.mousePosition;

                // 마우스가 오른쪽을 향하면
                if (mousePos.x - mouseOrigin.x > 0)
                {
                    dir -= cross * 0.5f;
                }

                // 마우스가 왼쪽을 향하면
                else if (mousePos.x - mouseOrigin.x < 0)
                {
                    dir += cross * 0.5f;
                }
                transform.position += dir * rightspeed * Time.deltaTime;
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

                rightspeed = 15f;
                transform.localPosition = rightOriginLocalPos;
                click = false;
                fire = false;
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
            anim.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 잡기 상태가 아닐때
        if (!yj_trigger.grap)
        {
            // 애너미레이어와 닿았을 때
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //audioSource.PlayOneShot(hitSound);
                overlap = true;
            }
        }
    }

    void Return()
    {
        fire = false;
        click = false;
        rightspeed = 15f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, rightOriginLocalPos, Time.deltaTime * backspeed);
        
    }
}
