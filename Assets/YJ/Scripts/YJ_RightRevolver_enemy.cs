using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앞으로 조금 이동시킨 후 리볼버 세개를 순서대로 발사하고싶다.

public class YJ_RightRevolver_enemy : YJ_Hand_right
{
    GameObject trigger; // 가운데 선

    // 리볼버세개
    public YJ_Revolver10 revolver_10;
    public YJ_Revolver11 revolver_11;
    public YJ_Revolver12 revolver_12;

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
    Animation anim;

    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip shoockSound; // 주먹 날아갈때 사운드

    YJ_Trigger_enemy yj_trigger_enemy;

    void Start()
    {
        enemy = GameObject.Find("Enemy");

        transform.forward = enemy.transform.forward;

        yj_KillerGage = GameObject.Find("KillerGage_e (2)").GetComponent<YJ_KillerGage>();
        
        originPos = GameObject.Find("rightPos_e").transform;

        anim = GetComponent<Animation>();

        audioSource = GetComponent<AudioSource>();

        trigger = enemy.transform.Find("YJ_trigger").gameObject;

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
        if (InputManager.Instance.EnemyFire2 && !fire && !yj_trigger_enemy.grap)
        {
            audioSource.PlayOneShot(shoockSound);
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
        if (Vector3.Distance(transform.position, enemy.transform.position) > 2.5f)
        {
            dir = Vector3.zero;
            // 리볼버발사
            isFire = true;
        }
        // 모든 리볼버가 제자리로 돌아왔다면
        if (revolver_10.end && revolver_11.end && revolver_12.end)
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
            revolver_10.end = false;
            revolver_11.end = false;
            revolver_12.end = false;
            // 애니메이션 플레이
            anim.Play();
            // 공격종료
            fire = false;
        }

    }
}
