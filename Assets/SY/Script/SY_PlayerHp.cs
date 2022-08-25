using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_PlayerHp : MonoBehaviour
{
    AudioSource source;
    public AudioClip hitted;

    int hp;
    public int maxHp = 1000;
    public Slider sliderHp;
    public Coroutine coroutine;
    public Text hpDc;
    JH_EnemyCharge ech;
    JH_PlayerMove pm;

    SY_GameOver1 koText;

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
        if (value <= 0)
        {
            pm.State = JH_PlayerMove.PlayerState.Die;
            pm.Died();
            koText.GameOverText(true);
        }

        if (hp != value && value != maxHp)
        {
            transform.Find("Damage").GetComponent<JH_Damage>().FloatText(hp - value);
            source.PlayOneShot(hitted); 
        }
        hp = value;
        sliderHp.value = value;
    }

    public int GetHP()
    {
        return hp;
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
        ech = GameObject.Find("Enemy").GetComponent<JH_EnemyCharge>();
        pm = GetComponent<JH_PlayerMove>();
        sliderHp.maxValue = maxHp;
        SetHP(maxHp);
        koText = GameObject.Find("InputManager").GetComponent<SY_GameOver1>();
    }

    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        // "EnemyArms"에 닿고 charging 된 에너미 팔에 맞으면
        if (other.gameObject.CompareTag("EnemyArms"))  // =>
        {
            if (InputManager.Instance.Guard || IsKnock || pm.State == JH_PlayerMove.PlayerState.Die || pm.State == JH_PlayerMove.PlayerState.Win)
            {

            }
            // 차징일 때, HP -2 / 넉백 실행
            else if (ech.IsCharging)
            {
                SetHP(GetHP() - other.gameObject.GetComponent<JH_ArmDamage>().ChargeDamage);
                isKnock = true;
            }
            // 차징아닐 때, HP -1
            else
            {
                SetHP(GetHP() - other.gameObject.GetComponent<JH_ArmDamage>().Damage);
                if (pm.State != JH_PlayerMove.PlayerState.Die)
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
