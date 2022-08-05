using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_PlayerHp : MonoBehaviour
{
    int hp;
    public int maxHp = 6;
    public Slider sliderHp;
    public Coroutine coroutine;

    bool canUp = false;
    public bool CanUp
    {
        get { return canUp; }
        set { canUp = value; }
    }
    bool isKnock = false;
    public bool IsKnock
    {
        get { return isKnock; }
        set { isKnock = value; }
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
            coroutine = StartCoroutine(Ondamaged());
        }
        
    }

    // 넉백 후 10초동안 무적상태
    public IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(2.0f);
        canUp = true;
        yield return new WaitForSeconds(3.0f);
        isKnock = false;
        canUp = false;
    }
}
