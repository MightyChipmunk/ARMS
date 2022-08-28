using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앞으로 조금 이동시킨 후 리볼버 세개를 순서대로 발사하고싶다.

public class YJ_LeftRevolver_enemy : YJ_Hand_left
{
    GameObject trigger; // 가운데 선

    // 리볼버세개
    public YJ_Revolver7 revolver_7;
    public YJ_Revolver8 revolver_8;
    public YJ_Revolver9 revolver_9;

    // origin위치
    Transform originPos;

    // 이동속도
    float speed = 15f;
    float backspeed = 20f;

    // 방향
    Vector3 dir;

    // 플레이어위치 가져오기
    GameObject enemy;

    // 리볼버 발사할 bool값
    public bool isFire = false;

    // 애니메이션
    //Animation anim;


    YJ_Trigger_enemy yj_trigger_enemy;

    void Start()
    {
        enemy = GameObject.Find("Enemy");

        transform.forward = enemy.transform.forward;

        yj_KillerGage_enemy = GameObject.Find("KillerGage_e (2)").GetComponent<YJ_KillerGage_enemy>();

        //anim = GetComponent<Animation>();

        originPos = GameObject.Find("leftPos_e").transform;

        trigger = enemy.transform.Find("YJ_Trigger").gameObject;

        yj_trigger_enemy = trigger.GetComponent<YJ_Trigger_enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (yj_KillerGage_enemy.killerModeOn_enemy)
        {
            speed = 30f;
            backspeed = 50f;
        }
        else
        {
            speed = 15f;
            backspeed = 20f;
        }
        // 왼쪽 마우스 버튼을 누르면 앞으로 조금 이동하고싶다
        if (InputManager.Instance.EnemyFire1 && !fire && !yj_trigger_enemy.grap)
        {
            //anim.Stop();
            speed = 15f;
            fire = true;
        }

        // 앞으로 나아가기(공격)
        if (fire)
            Fire_Revolver();
    }
    void Fire_Revolver()
    {
        dir = transform.forward;
        if (Vector3.Distance(transform.position, enemy.transform.position) > 2.5f)
        {
            dir = Vector3.zero;
            // 리볼버발사
            isFire = true;
        }
        // 모든 리볼버가 제자리로 돌아왔다면
        if (revolver_7.end && revolver_8.end && revolver_9.end)
        {
            isFire = false;
            Return();
        }
        transform.position += dir * speed * Time.deltaTime;
    }

    void Return()
    {
        // 반대로 돌아오기
        speed = backspeed;
        dir = originPos.position - transform.position;
        if (Vector3.Distance(transform.position, originPos.position) < 0.3f)
        {
            // 멈추기
            dir = Vector3.zero;
            // 원위치 돌아오기
            transform.position = originPos.position;
            // 리볼버에 bool값 변경
            revolver_7.end = false;
            revolver_8.end = false;
            revolver_9.end = false;
            // 애니메이션 플레이
            //anim.Play();
            // 공격종료
            fire = false;
        }

    }
}
