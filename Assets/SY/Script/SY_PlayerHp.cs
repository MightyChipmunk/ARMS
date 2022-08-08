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
        // 적 팔에 닿으면 충돌 이벤트 구현
        if (other.gameObject.CompareTag("EnemyArms") && other.TryGetComponent<SY_LeftCharge>(out lc))
        {
            if (Input.GetKey(KeyCode.F) || IsKnock)
            {
               
            }
            // 차징일 때는, KnockBack실행
            else if (lc.IsCharging)
            {
                // 체력 감소
                SetHP(GetHP() - 2);
                isKnock = true; //-> 추후 캐릭터 애니매시션을 통해 KnockBack 구현
                Debug.Log("Isknock " + isKnock);
            }
            // 차징 아닐 때는, HP감소
            else
            {
                // 체력 감소ww
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
            // 차징일 때는, KnockBack실행
            else if (rc.IsCharging)
            {
                // 체력 감소
                SetHP(GetHP() - 2);
                isKnock = true; //-> 추후 캐릭터 애니매시션을 통해 KnockBack 구현
                Debug.Log("Isknock " + isKnock);
            }
            // 차징 아닐 때는, HP감소
            else
            {
                // 체력 감소
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

    // 넉백 후 5초동안 무적상태
    IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(5.0f);
        isKnock = false;
    }
}
