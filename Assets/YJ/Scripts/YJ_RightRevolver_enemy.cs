using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� �̵���Ų �� ������ ������ ������� �߻��ϰ�ʹ�.

public class YJ_RightRevolver_enemy : YJ_Hand_right
{
    GameObject trigger; // ��� ��

    // ����������
    public YJ_Revolver10 revolver_10;
    public YJ_Revolver11 revolver_11;
    public YJ_Revolver12 revolver_12;

    // origin��ġ
    Transform originPos;

    // �̵��ӵ�
    float speed = 15f;
    float backspeed = 20f;

    // ����
    Vector3 dir;

    // �÷��̾���ġ ��������
    GameObject enemy;

    // ������ �߻��� bool��
    public bool isFire = false;

    // �ִϸ��̼�
    Animation anim;

    AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip shoockSound; // �ָ� ���ư��� ����

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
        // ���� ���콺 ��ư�� ������ ������ ���� �̵��ϰ�ʹ�
        if (InputManager.Instance.EnemyFire2 && !fire && !yj_trigger_enemy.grap)
        {
            audioSource.PlayOneShot(shoockSound);
            anim.Stop();
            speed = 15f;
            fire = true;
        }

        // ������ ���ư���(����)
        if (fire)
            Fire_Revolver();
    }
    void Fire_Revolver()
    {
        dir = transform.forward;
        if (Vector3.Distance(transform.position, enemy.transform.position) > 2.5f)
        {
            dir = Vector3.zero;
            // �������߻�
            isFire = true;
        }
        // ��� �������� ���ڸ��� ���ƿԴٸ�
        if (revolver_10.end && revolver_11.end && revolver_12.end)
        {
            isFire = false;
            Return();
        }
        transform.position += dir * speed * Time.deltaTime;
    }

    void Return()
    {
        // �ݴ�� ���ƿ���
        speed = backspeed;
        dir = originPos.position - transform.position;
        if (Vector3.Distance(transform.position, originPos.position) < 0.3f)
        {
            // ���߱�
            dir = Vector3.zero;
            // ����ġ ���ƿ���
            transform.position = originPos.position;
            // �������� bool�� ����
            revolver_10.end = false;
            revolver_11.end = false;
            revolver_12.end = false;
            // �ִϸ��̼� �÷���
            anim.Play();
            // ��������
            fire = false;
        }

    }
}
