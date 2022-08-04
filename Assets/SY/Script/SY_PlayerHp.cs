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
        // �� �ȿ� ������ �浹 �̺�Ʈ ����
        if (other.gameObject.tag == "EnemyArms")
        {
            if (Input.GetKey(KeyCode.F) || IsKnock)
            {
                
            }
            else
            {
                // ü�� ����
                SetHP(GetHP() - 1);
                isKnock = true; //-> ���� ĳ���� �ִϸŽü��� ���� KnockBack ����
            }
        }

        if (isKnock != false)
        {
            StartCoroutine(Ondamaged());
        }
        
    }

    // �˹� �� 10�ʵ��� ��������
    IEnumerator Ondamaged()
    {
        yield return new WaitForSeconds(5.0f);
        isKnock = false;
    }
}
