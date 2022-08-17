using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_PlayerHp : MonoBehaviour
{
    int hp;
    public int maxHp = 1000;
    public Slider sliderHp;
    public Coroutine coroutine;
    public Text hpDc;
    SY_EnemyRightCharge erc;
    SY_EnemyLeftCharge elc;



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

        pm = GetComponent<JH_PlayerMove>();
        sliderHp.maxValue = maxHp;
        SetHP(maxHp);
    }

    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        // "EnemyArms"에 닿고 charging 된 에너미 팔에 맞으면
        if (other.gameObject.CompareTag("EnemyArms") && other.TryGetComponent<SY_EnemyRightCharge>(out erc))  // =>
        {
            if (InputManager.Instance.Guard || IsKnock)
            {

            }
            // 차징일 때, HP -2 / 넉백 실행
            else if (erc.IsCharging)
            {

                SetHP(GetHP() - 2);
                isKnock = true;
            }
            // 차징아닐 때, HP -1
            else
            {

                SetHP(GetHP() - 1);
                //isKnock = false;
                pm.Hitted();
            }
        }
        else if (other.gameObject.CompareTag("EnemyArms") && other.TryGetComponent<SY_EnemyLeftCharge>(out elc))
        {

            if (InputManager.Instance.Guard || IsKnock)
            {

            }
            // 차징일 때, HP -2 / 넉백 실행
            else if (elc.IsCharging)
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

    // 넉백 시간
    public IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(2.0f);
        canUp = true;
        yield return new WaitForSeconds(3.0f);
        isKnock = false;
        canUp = false;
    }
}
