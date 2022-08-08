using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_PlayerHp : MonoBehaviour
{
    int hp;
    public int maxHp = 6;
    public Slider sliderHp;
    SY_LeftCharge lc;
    SY_RightCharge rc;

    bool isKnock;
    public bool IsKnock
    {
        get { return isKnock; }
    }

    public void SetHP(int value)
    {
        hp = value;
        sliderHp.value = value;
    }
    public int GetHP()
    {
        return hp;
    }
    void Start()
    {

        sliderHp.maxValue = maxHp;
        SetHP(maxHp);
    }

    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        // �� �ȿ� ������ �浹 �̺�Ʈ ����
        if (other.gameObject.CompareTag("EnemyArms") && other.TryGetComponent<SY_LeftCharge>(out lc))
        {
            if (Input.GetKey(KeyCode.F) || IsKnock)
            {
               
            }
            // ��¡�� ����, KnockBack����
            else if (lc.IsCharging)
            {
                // ü�� ����
                SetHP(GetHP() - 2);
                isKnock = true; //-> ���� ĳ���� �ִϸŽü��� ���� KnockBack ����
                Debug.Log("Isknock " + isKnock);
            }
            // ��¡ �ƴ� ����, HP����
            else
            {
                // ü�� ����ww
                SetHP(GetHP() - 1);
                //isKnock = false;
                Debug.Log("Isknock " + isKnock);

            }
        }
        else if (other.gameObject.CompareTag("EnemyArms") && other.TryGetComponent<SY_RightCharge>(out rc))
        {

            if (Input.GetKey(KeyCode.F) || IsKnock)
            {
                
            }
            // ��¡�� ����, KnockBack����
            else if (rc.IsCharging)
            {
                // ü�� ����
                SetHP(GetHP() - 2);
                isKnock = true; //-> ���� ĳ���� �ִϸŽü��� ���� KnockBack ����
                Debug.Log("Isknock " + isKnock);
            }
            // ��¡ �ƴ� ����, HP����
            else
            {
                // ü�� ����
                SetHP(GetHP() - 1);
                //isKnock = false;
                Debug.Log("Isknock " + isKnock);
            }
        }

        if (isKnock != false)
        {
            StartCoroutine(Ondamaged());
        }
        
    }

    // �˹� �� 5�ʵ��� ��������
    IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(5.0f);
        isKnock = false;
    }
}
