using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앞으로 조금 이동시킨 후 리볼버 세개를 순서대로 발사하고싶다.

public class YJ_RightRevolver : YJ_Hand_right
{
    // 리볼버세개
    public YJ_Revolver4 revolver_4;
    public YJ_Revolver5 revolver_5;
    public YJ_Revolver6 revolver_6;

    // origin위치
    public Transform originPos;

    // 이동속도
    float speed = 15f;
    float backspeed = 20f;

    // 방향
    Vector3 dir;

    // 플레이어위치 가져오기
    GameObject player;

    // 리볼버 발사할 bool값
    public bool isFire = false;

    // 애니메이션
    Animation anim;

    void Start()
    {
        player = GameObject.Find("Player");

        transform.forward = player.transform.forward;

        yj_KillerGage = GameObject.Find("KillerGage (2)").GetComponent<YJ_KillerGage>();
        originPos = GameObject.Find("rightPos").transform;

        anim = GetComponent<Animation>();
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
            speed = 15f;
            backspeed = 20f;
        }
        // 왼쪽 마우스 버튼을 누르면 앞으로 조금 이동하고싶다
        if (InputManager.Instance.Fire2 && !fire)
        {
            anim.Stop();
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
        if (Vector3.Distance(transform.position, player.transform.position) > 2.5f)
        {
            dir = Vector3.zero;
            // 리볼버발사
            isFire = true;
        }
        // 모든 리볼버가 제자리로 돌아왔다면
        if (revolver_4.end && revolver_5.end && revolver_6.end)
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
            revolver_4.end = false;
            revolver_5.end = false;
            revolver_6.end = false;
            // 애니메이션 플레이
            anim.Play();
            // 공격종료
            fire = false;
        }

    }
}
