using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // "F"�� ������ ���� ����
    void Update()
    {
        Charging();
    }

    void Charging()
    {
        if (InputManager.Instance.EnemyGuard)
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
            if (InputManager.Instance.EnemyGuardUp)
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

    IEnumerator UnCharge()
    {
        yield return new WaitForSeconds(0.1f);
        isCharging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerArms")
        {
            // �� ������ ���� ����
        }

        if (other.gameObject.layer == 10 && isCharging)
        {
            StopCoroutine("WaitForIt");
            StartCoroutine("UnCharge");
        }
    }
}
