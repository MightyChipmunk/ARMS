using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// isFire가 true면 targetPos로 가고싶다
public class YJ_Revolver3 : MonoBehaviour
{
    // 내부 bool 값
    public bool end = false;
    bool trigger = false; // 닿았을때

    // 애너미를 맞출곳
    public GameObject targetPos;
    // 애너미 위치저장
    Vector3 target;

    // 방향
    Vector3 dir;

    //거리
    float distance = 0;

    // 속도
    float speed = 3f;

    // 되돌아오는속도
    float backspeed = 5f;
    bool go = false;

    // 돌아올 곳
    public Transform originPos;

    // isFire를 가져올 곳
    public YJ_LeftRevolver leftRevolver;

    // 출발시간
    float currnetTime = 0;
    public float creatTime = 0;

    // 콜라이더 켜고 끄기
    Collider col;

    // 필살기 사용
    public YJ_KillerGage yj_KillerGage;

    // 트레일
    TrailRenderer trail;


    void Start()
    {
        col = GetComponent<Collider>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;

        yj_KillerGage = GameObject.Find("KillerGage (2)").GetComponent<YJ_KillerGage>();
        targetPos = GameObject.Find("EnemyAttackPos");
    }

    // Update is called once per frame
    void Update()
    {
        if (yj_KillerGage.killerModeOn)
        {
            speed = 30f;
            backspeed = 50f;
        }
        else
        {
            speed = 5f;
            backspeed = 10f;
        }
        // isFire가 true일때 0.2초 후 날아가기
        if (leftRevolver.isFire && !end)
        {
            // 갈때 콜라이더 켜기
            col.enabled = true;
            // 트레일 켜기
            trail.enabled = true;
            speed = 3f;
            currnetTime += Time.deltaTime;
            if (currnetTime < 0.2f)
            {
                target = targetPos.transform.position;
            }
            if (currnetTime > creatTime)
            {
                go = true;
                Go();
            }
            if (!trigger)
                dir = target - transform.position;
            if (trigger)
                dir = originPos.position - transform.position;
        }
        if (go)
            distance += speed * Time.deltaTime;
    }
    void Go()
    {
        // 거리가 1.7이상일때 되돌아오기
        if (distance > 1.7f)
        {
            // 트레일 끄기
            trail.enabled = false;
            // 되돌아오는함수
            Back();
        }
        // 앞으로 나아가기
        transform.position += dir * speed * Time.deltaTime;
    }

    void Back()
    {
        // 올때 콜라이더 끄기
        col.enabled = false;
        // 트레일 끄기
        trail.enabled = false;
        // 방향바꿔주기
        dir = originPos.position - transform.position;
        // 올때 스피드는 빠르게
        speed = backspeed;
        // 완전히 가까워지면 제자리로 돌리기
        if (Vector3.Distance(transform.position, originPos.position) < 0.3f)
        {
            dir = Vector3.zero;
            transform.position = originPos.position;
            go = false;
            distance = 0;
            currnetTime = 0;
            trigger = false;
            end = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 애너미레이어와 닿았을 때
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            trigger = true;
            Back();
        }
    }
}
