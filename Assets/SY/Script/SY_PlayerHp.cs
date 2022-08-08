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
    SY_LeftCharge lc;
    SY_RightCharge rc;
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
                isKnock = true;
            }
            // ��¡ �ƴ� ����, HP����
            else
            {
                // ü�� ����ww
                SetHP(GetHP() - 1);
                //isKnock = false;
                pm.Hitted();
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
                isKnock = true;
            }
            // ��¡ �ƴ� ����, HP����
            else
            {
                // ü�� ����
                SetHP(GetHP() - 1);
                pm.Hitted();
            }
        }

        if (isKnock != false)
        {
            coroutine = StartCoroutine(Ondamaged());
        }

    }

    // �˹� �� 10�ʵ��� ��������
    public IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(2.0f);
        canUp = true;
        yield return new WaitForSeconds(3.0f);
        isKnock = false;
        canUp = false;
    }
}
