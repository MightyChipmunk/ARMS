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
        print("��ư Ŭ��");

        // ��� Ŭ���� ���� ������Ʈ�� �����ͼ� ����
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        // ��� Ŭ���� ���� ������Ʈ�� �̸��� ��ư �� ���� ���
        print(clickObject.name + ", " + clickObject.GetComponentInChildren<Text>().text);

        //GameObject goImage = GameObject.Find("StartSet/Left1/Image");
        Color color = clickObject.GetComponent<Image>().color;
        color.a = 0.5f;
        clickObject.GetComponent<Image>().color = color;

    }
}

