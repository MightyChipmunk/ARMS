using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 크기를 1초마다 키우고
// 15초가 되면
// 필살기 사용가능 상태가되고
// Q키를 누르면 3초동안 공격속도가 두배씩 증가하고싶다
public class YJ_KillerGage_enemy : MonoBehaviour
{
    // 기존 크기
    Vector3 orizinSize;
    // 흐르는시간
    float currentTime;
    // 멈출시간
    float overTime;
    // 내 이미지 스케일
    Transform imageScale;
    // 상태전환을 위한 bool값
    public bool killerModeOn_enemy = false;

    // 사이즈 줄이기
    bool scaleDown = false;
    // 색상 변경을 위한 내 색상정보
    Image imagecolor;
    // 끌때 시간
    float endTime = 0;
    // 뒤에 같이 나올 이미지
    public RectTransform blur;

    void Start()
    {
        // 시작할때 사이즈 저장
        orizinSize = new Vector3(0.1f, 0.1f, 0.1f);
        // 이미지 스케일에 내 트렌스폼 이미지 스케일 넣어주기
        imageScale = GetComponent<Transform>();
        // 이미지 컬러를 변경하기위해 이미지 가져오기
        imagecolor = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ScaleUp();
    }

    void ScaleUp()
    {
        // 1초마다 UI 크기를 키우고싶다
        // UI 크기를 키운다
        // 한... 13초 걸리나?
        currentTime += Time.deltaTime;
        overTime += Time.deltaTime;
        if( overTime < 13f )
        {
            if(overTime > 10f)
            {
                blur.localScale += new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
            }
            if (currentTime > 1f)
            {
                imageScale.localScale += new Vector3(0.3f, 0.3f, 0.3f) * Time.deltaTime;
                if (currentTime > 1.3f)
                {
                    currentTime = 0;
                }
            }
        }
        else
        {
            imagecolor.color = Color.yellow;
            if(InputManager.Instance.EnemyKiller) // 애너미는 이거 변경해야함
            {
                killerModeOn_enemy = true;
                scaleDown = true;
            }
        }

        // 다시줄여주기
        if (scaleDown)
        {
            endTime += Time.deltaTime;
            imageScale.localScale -= new Vector3(0.0007f, 0.0007f, 0.0007f);
            if (endTime > 3f)
            {
                imageScale.localScale = orizinSize;
                imagecolor.color = Color.white;
                endTime = 0;
                overTime = 0;
                currentTime = 0;
                blur.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                killerModeOn_enemy = false;
                scaleDown = false;
            }
        }
    }

}
