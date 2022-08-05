using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_LeftCharge: MonoBehaviour
{
    MeshRenderer mesh;
    Material mat;
    Rigidbody rigid;
    Renderer color;

    float currentTime;
    float creatTime = 2f;

    bool isGuard = false;
    public bool IsGuard
    {
        get { return isGuard; }
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
        color = GetComponent<Renderer>();
    }

    // "F"�� ������ ���� ����
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            currentTime += Time.deltaTime;
            isGuard = true;
            if (currentTime > creatTime)
            {
                mat.color = new Color(0, 0, 1);  //-> ���� ĳ���� �ִϸŽü��� ���� Charging ����
                currentTime = 0;
                StopCoroutine("WaitForIt");
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                StartCoroutine("WaitForIt");
                isGuard = false;
                currentTime = 0;
            }
        }
        //Debug.Log("LeftGuard: " + isGuard);
    }

    // 5�� �� ���� Ǯ��
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(5.0f);
        mat.color = new Color(1, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyArms")
        {
            if (color.material.color == Color.white)
            {
                //mat.color = new Color(1,1,0); // ���
                color.material.color = Color.yellow; // ������ ���
            }
            else
            {
                color.material.color = Color.red; // �� ����
            }
        }
    }
}
