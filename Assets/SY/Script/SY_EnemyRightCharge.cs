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

    // "F"를 누르면 차지 실행
    void Update()
    {
        Charging();
    }

    void Charging()
    {
        
        if (InputManager.Instance.EnemyGuard)
        {
            // "F"키를 누르면 가드를 한다.
            currentTime += Time.deltaTime;
            isGuard = true;

            // "F"키를 2초 이상 누르면 차징 상태이고 싶다. 
            if (currentTime > creatTime)
            {
                //mat.color = new Color(0, 0, 1);  //-> 추후 캐릭터 애니매시션을 통해 Charging 구현
                isCharging = true;
                currentTime = 0;
                StopCoroutine("WaitForIt");
                Debug.Log("EnemyRightCharging: " + isCharging);
            }
        }

        // "F"키를 누르면 가드를 해제한다.
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

    // 5초 후 차지 풀림
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
                //mat.color = new Color(1,1,0); // 노랑
                color.material.color = Color.yellow; // 데미지 경고
            }
            else
            {
                color.material.color = Color.red; // 팔 멈춤
            }
        }
    }
}
