using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_RightCharge: MonoBehaviour
{
    MeshRenderer mesh;
    Material mat;
    Rigidbody rigid;

    float currentTime;
    float creatTime = 1f;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
    }

    // "F"�� ������ ���� ����
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            
            currentTime += Time.deltaTime;
           

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
                currentTime = 0;
            }
        }
    }

    // 5�� �� ���� Ǯ��
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(5.0f);
        mat.color = new Color(1, 1, 1);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� �Ȱ��� ������ HP ����ǥ��
        if (collision.gameObject.tag == "EnemyArms")
        {
            mat.color = new Color(1,0,0);
        }
    }
}