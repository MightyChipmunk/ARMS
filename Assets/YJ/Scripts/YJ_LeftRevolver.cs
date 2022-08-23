using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앞으로 조금 이동시킨 후 리볼버 세개를 순서대로 발사하고싶다.

public class YJ_LeftRevolver : YJ_Hand_left
{
    // 리볼버세개
    public YJ_Revolver1 revolver_1;
    public YJ_Revolver2 revolver_2;
    public YJ_Revolver3 revolver_3;

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
        if (InputManager.Instance.Fire1 && !fire)
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
        if (revolver_1.end && revolver_2.end && revolver_3.end)
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
            revolver_1.end = false;
            revolver_2.end = false;
            revolver_3.end = false;
            // 애니메이션 플레이
            anim.Play();
            // 공격종료
            fire = false;
        }

    }
}
