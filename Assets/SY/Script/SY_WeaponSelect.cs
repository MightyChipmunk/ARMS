using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SY_WeaponSelect : MonoBehaviour
{
    public GameObject left1;
    public void Start()
    {
       
    }

    public void ClickBtn()
    {
        print("버튼 클릭");

        // 방금 클릭한 게임 오브젝트를 가져와서 저장
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        // 방금 클릭한 게임 오브젝트의 이름과 버튼 속 문자 출력
        print(clickObject.name + ", " + clickObject.GetComponentInChildren<Text>().text);

        //GameObject goImage = GameObject.Find("StartSet/Left1/Image");
        Color color = clickObject.GetComponent<Image>().color;
        color.a = 0.5f;
        clickObject.GetComponent<Image>().color = color;

    }
}

