using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_PlayerCharge : MonoBehaviour
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

    // "F"�� ������ ���� ����
    void Update()
    {
        Charging();
    }

    void Charging()
    {
        if (InputManager.Instance.Guard)
        {
            // "F"Ű�� ������ ���带 �Ѵ�.
            currentTime += Time.deltaTime;
            isGuard = true;

            // "F"Ű�� 2�� �̻� ������ ��¡ �����̰� �ʹ�. 
            if (currentTime > creatTime)
            {
                isCharging = true;
                currentTime = 0;
                StopCoroutine("WaitForIt");
            }
        }
        // "F"Ű�� ������ ���带 �����Ѵ�.
        else
        {
            if (InputManager.Instance.GuardUp || GetComponent<JH_PlayerMove>().IsGrapped())
            {
                StartCoroutine("WaitForIt");
                isGuard = false;
                currentTime = 0;
            }
        }
    }

    // 5�� �� ���� Ǯ��
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
        if (other.gameObject.tag == "EnemyArms")
        {
            // �� ������ ���� ����
        }

        if (other.gameObject.layer == 11)
        {
            if (IsCharging)
                UnCharge();
            if (GameObject.Find("Enemy").GetComponent<JH_EnemyCharge>().IsCharging)
                GameObject.Find("Enemy").GetComponent<JH_EnemyCharge>().UnCharge();
        }
    }
}
