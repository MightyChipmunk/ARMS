using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_PlayerHp : MonoBehaviour
{
    int hp;
    public int maxHp = 6;
    public Slider sliderHp;

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
        if (Input.GetKey(KeyCode.F) || IsKnock)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적 팔에 닿으면 충돌 이벤트 구현
        if (other.gameObject.tag == "EnemyArms")
        {
            if (Input.GetKey(KeyCode.F) || IsKnock)
            {
                
            }
            else
            {
                // 체력 감소
                SetHP(GetHP() - 1);
                isKnock = true; //-> 추후 캐릭터 애니매시션을 통해 KnockBack 구현
            }
        }

        if (isKnock != false)
        {
            StartCoroutine(Ondamaged());
        }
        
    }

    // 넉백 후 10초동안 무적상태
    IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(5.0f);
        isKnock = false;
    }
}
