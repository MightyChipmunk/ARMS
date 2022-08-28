using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 왼쪽 버튼을 누르면 바깥쪽 대각선 방향으로 향하고 타겟포지션을 쳐다보고싶다.
public class YJ_LeftFox_enemy : YJ_Hand_left
{
    GameObject trigger; // 가운데 선

    Vector3 dir;
    float speed = 7f;
    public float distance = 0f;

    public GameObject originPos;
    GameObject enemy;
    GameObject player;
    public GameObject lazer;
    Vector3 target_pos;

    bool go;
    bool back;
    public bool lazerOn; //protected 자식만 상속가능

    //Animation anim;

    // 레이저가 애너미에 닿았는지 확인할 것
    YJ_LeftFox_lazer_e yj_leftfox_lazer;

    YJ_Trigger_enemy yj_trigger_enemy;

    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip lazerSound;

    //bool goRay;
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        
        // 레이저 안보이게 할 것
        cylinder.GetComponent<MeshRenderer>().enabled = false;

        // 애니메이션
        //anim = GetComponent<Animation>();

        //anim.Play("idleee");
        originPos = GameObject.Find("leftPos_e");

        yj_leftfox_lazer = cylinder.GetComponent<YJ_LeftFox_lazer_e>();

        yj_KillerGage_enemy = GameObject.Find("KillerGage_e (2)").GetComponent<YJ_KillerGage_enemy>();

        trigger = enemy.transform.Find("YJ_Trigger").gameObject;

        yj_trigger_enemy = trigger.GetComponent<YJ_Trigger_enemy>();

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (InputManager.Instance.EnemyFire1 && !fire && !yj_trigger_enemy.grap)
        {
            // 필살기 감지
            if (yj_KillerGage_enemy.killerModeOn_enemy)
            {
                lazer.transform.localScale = new Vector3(0.25f, 0.25f, 0.03f);
            }
            if (!yj_KillerGage_enemy.killerModeOn_enemy)
            {
                lazer.transform.localScale = new Vector3(0.1f, 0.1f, 0.03f);
            }

            
            //anim.Stop("idleee");
            fire = true;
            // 눌렀을때 애너미 처음 위치
            target_pos = player.transform.position;

            //방향
            dir = -enemy.transform.position + transform.position;
            dir.Normalize();
            go = true;
        }
        if (go)
        {
            // 대각선으로 이동
            distance += speed * Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
        }


        GoRay();

        if(lazerOn)
        {
            if (openMouseTime > 0.25f && !yj_leftfox_lazer.triggerOn)
            {
                // 레이저 발사
                lazer.transform.localScale += new Vector3(0, 0, 1f) * 10 * Time.deltaTime;
            }

            if (openMouseTime > 0.25 && openMouseTime < 0.26)
            {
                audioSource.PlayOneShot(lazerSound);
            }
        }

        ScaleDown();

        if (back)
            GoBack();

    }


    float currentTime = 0;
    public bool scaleDown;
    private void GoRay()
    {
        // 손이 2.5만큼 이동하면
        if (distance > 2.5f)
        {
            
            go = false;

            // 보이게 켜두고
            cylinder.GetComponent<MeshRenderer>().enabled = true;

            // 발사할 곳 쳐다보기
            transform.LookAt(target_pos);

            // 입을 벌리고
            OpenMouse();



            // 레이저발사
            lazerOn = true;

            // 0.5초 후에 사이즈 줄일것
            currentTime += Time.deltaTime;

            if (currentTime > 1f )
            {
                //anim.Stop("Attack 1");
                //anim.Play("Attack 2");
                scaleDown = true;
                lazerOn = false;
                distance = 0;
            }

        }

    }

    public GameObject cylinder;
    private void ScaleDown()
    {
        if (scaleDown)
        {
            
            openMouseTime = 0;
            distance = 0;
            currentTime = 0;
            
            if(lazer.transform.localScale.x > 0.1f)
            {
                // 레이저 크기 줄여주기
                lazer.transform.localScale -= new Vector3(0.5f, 0.5f, 0) * 1 * Time.deltaTime;
            }
            // 많이 줄어 들었으면
            else
            {
                // 안보이게 꺼주고
                cylinder.GetComponent<MeshRenderer>().enabled = false;

                // 준비상태 크기로 변경
                lazer.transform.localScale = new Vector3(0.1f, 0.1f, 0.03f);

                // 입닫기 애니메이션
                CloseMouse();

            }

            // 입 다 닫고 나서
            if (closeMouseTime > 0.25f)
            { 
                back = true;
                scaleDown = false;
            }

        }
    }

    private void GoBack()
    {
        currentTime += Time.deltaTime;
        
        if(currentTime > 0.2f)
        {
            // 원래있던 자리로 방향지정
            dir = originPos.transform.position - transform.position;
            dir.Normalize();

            // 갑시다
            transform.position += dir * speed * Time.deltaTime;

            // 거의 다왔으면 완전히 되돌리기
            if (Vector3.Distance(transform.position, originPos.transform.position) < 0.2f)
            {
                transform.position = originPos.transform.position;
                yj_leftfox_lazer.triggerOn = false;
                // LookAt 풀기
                transform.forward = -Camera.main.transform.forward;
                currentTime = 0;
                closeMouseTime = 0;
                //anim.Play("idleee");
                fire = false;
                back = false;
            }

        }
    }

    public GameObject helm_visor;
    public GameObject ear_main_L;
    public GameObject ear_main_R;
    public GameObject sidewing_L;
    public GameObject sidewing_R;
    float openMouseTime = 0;
    float closeMouseTime = 0;
    void OpenMouse()
    {
        openMouseTime += Time.deltaTime;
        if(openMouseTime < 0.25f)
        {
            helm_visor.transform.eulerAngles += new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_L.transform.eulerAngles += new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_R.transform.eulerAngles += new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_L.transform.eulerAngles += new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_R.transform.eulerAngles += new Vector3(2, 0, 0) * 60 * Time.deltaTime;
        }
    }

    void CloseMouse()
    {
        closeMouseTime += Time.deltaTime;
        if (closeMouseTime < 0.25f)
        {
            helm_visor.transform.eulerAngles -= new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_L.transform.eulerAngles -= new Vector3(-2, 0, 0) * 60 * Time.deltaTime;
            ear_main_R.transform.eulerAngles -= new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_L.transform.eulerAngles -= new Vector3(2, 0, 0) * 60 * Time.deltaTime;
            sidewing_R.transform.eulerAngles -= new Vector3(2, 0, 0) * 60 * Time.deltaTime;
        }
    }
}
