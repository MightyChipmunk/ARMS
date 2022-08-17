using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_EnemyHp : MonoBehaviour
{
    int hp;
    public int maxHp = 1000;
    public Slider sliderHp;
    public Coroutine coroutine;
    // Player 차징
    JH_PlayerCharge ch;

    JH_PlayerMove pm;

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
        ch = GameObject.Find("Player").GetComponent<JH_PlayerCharge>();
        pm = GetComponent<JH_PlayerMove>();
        sliderHp.maxValue = maxHp;
        SetHP(maxHp);
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        // PlayerArms에 닿고 charging된 플레이어 팔에 맞으면
        if (other.gameObject.CompareTag("PlayerArms"))
        {

            if (InputManager.Instance.EnemyGuard || IsKnock)
            {

            }
            // 차징일 때, HP -2 / 넉백 실행
            else if (ch.IsCharging)
            {
                SetHP(GetHP() - 120);
                isKnock = true;

            }
            // 차징아닐 때, HP -1
            else
            {
                SetHP(GetHP() - 90);
                pm.Hitted();
            }

        }

        if (isKnock != false)
        {
            coroutine = StartCoroutine(Ondamaged());
        }

    }

    // 넉백 후 5초동안 무적상태
    public IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(2.0f);
        canUp = true;
        yield return new WaitForSeconds(3.0f);
        isKnock = false;
        canUp = false;
    }
}
