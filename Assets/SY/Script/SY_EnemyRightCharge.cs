using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_EnemyRightCharge : MonoBehaviour
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

    bool isCharging;
    public bool IsCharging
    {
        get { return isCharging; }
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
        Charging();
    }

    void Charging()
    {
        
        if (InputManager.Instance.EnemyGuard)
        {
            // "F"Ű�� ������ ���带 �Ѵ�.
            currentTime += Time.deltaTime;
            isGuard = true;

            // "F"Ű�� 2�� �̻� ������ ��¡ �����̰� �ʹ�. 
            if (currentTime > creatTime)
            {
                //mat.color = new Color(0, 0, 1);  //-> ���� ĳ���� �ִϸŽü��� ���� Charging ����
                isCharging = true;
                currentTime = 0;
                StopCoroutine("WaitForIt");
                Debug.Log("EnemyRightCharging: " + isCharging);
            }
        }

        // "F"Ű�� ������ ���带 �����Ѵ�.
        else
        {
            if (InputManager.Instance.EnemyGuardUp)
            {
                StartCoroutine("WaitForIt");
                isGuard = false;
                currentTime = 0;
            }
        }
        //Debug.Log("RightGuard: " + isGuard);
    }

    // 5�� �� ���� Ǯ��
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(5.0f);
        //mat.color = new Color(1, 1, 1);
        isCharging = false;
        Debug.Log("EnemyRightCharging: " + isCharging);
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
