using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_PlayerHp : MonoBehaviour
{
    int hp;
    public int maxHp = 6;
    public Slider sliderHp;

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
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;

        sliderHp.maxValue = maxHp;
        SetHP(maxHp);
    }

    void Update()
    {
      
    }

    MeshRenderer mesh;
    Material mat;

    private void OnCollisionEnter(Collision collision)
    {
        // �� �ȿ� ������ �浹 �̺�Ʈ ����
        if (collision.gameObject.tag == "EnemyArms")
        {
            if (Input.GetKey(KeyCode.F))
            {

            }
            else
            {
                // ü�� ����
                SetHP(GetHP() - 1);
                mat.color = new Color(1, 0, 0);  //-> ���� ĳ���� �ִϸŽü��� ���� KnockBack ����
            }
        }
    }
}
