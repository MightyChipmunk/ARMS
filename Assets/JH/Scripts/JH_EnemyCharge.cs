using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_EnemyCharge : MonoBehaviour
{
    float currentTime;
    float creatTime = 2f;

    bool isGuard = false;
    public bool IsGuard
    {
        get { return isGuard; }
    }

    bool isCharging;
    public bool IsCharging
    {
        get { return isCharging; }
    }

    void Start()
    {
        
    }

    // "F"를 누르면 차지 실행
    void Update()
    {
        Charging();
    }

    void Charging()
    {
        if (InputManager.Instance.EnemyGuard)
        {
            // "F"키를 누르면 가드를 한다.
            currentTime += Time.deltaTime;
            isGuard = true;

            // "F"키를 2초 이상 누르면 차징 상태이고 싶다. 
            if (currentTime > creatTime)
            {
                isCharging = true;
                currentTime = 0;
                StopCoroutine("WaitForIt");
            }
        }

        // "F"키를 누르면 가드를 해제한다.
        else
        {
            if (InputManager.Instance.EnemyGuardUp || GetComponent<JH_PlayerMove>().IsGrapped(true))
            {
                StartCoroutine("WaitForIt");
                isGuard = false;
                currentTime = 0;
            }
        }
    }

    // 5초 후 차지 풀림
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(5.0f);
        isCharging = false;
    }

    IEnumerator UnCharge_C()
    {
        yield return new WaitForSeconds(0.1f);
        isCharging=false;
    }
    public void UnCharge()
    {
        StopCoroutine("WaitForIt");
        StartCoroutine("UnCharge_C");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerArms")
        {
            // 팔 맞을때 추후 구현
        }

        if (other.gameObject.layer == 10)
        {
            if (IsCharging)
                UnCharge();
            if (GameObject.Find("Player").GetComponent<JH_PlayerCharge>().IsCharging)
                GameObject.Find("Player").GetComponent<JH_PlayerCharge>().UnCharge();
        }
    }
}
