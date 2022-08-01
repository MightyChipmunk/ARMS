using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SY_LeftCharge: MonoBehaviour
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

    // "F"를 누르면 차지 실행
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            currentTime += Time.deltaTime;

            if (currentTime > creatTime)
            {
                mat.color = new Color(0, 0, 1);  //-> 추후 캐릭터 애니매시션을 통해 Charging 구현
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

    // 5초 후 차지 풀림
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(5.0f);
        mat.color = new Color(1, 1, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌시 팔공격 받으면 HP 감소표시
        if (collision.gameObject.tag == "EnemyArms")
        {
            mat.color = new Color(1, 0, 0);
        }
    }
}
