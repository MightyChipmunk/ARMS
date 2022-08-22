using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SY_MemuManager : MonoBehaviour
{
    public GameObject startSet;
    public GameObject menuSet;
    
    // Start is called before the first frame update
    void Start()
    {
        menuSet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 서브메뉴
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false); 
            }
            else
            {
                menuSet.SetActive(true);
            }
        }
    }

    // 캐릭터창으로 이동
    public void ChangeScene()
    {
        SceneManager.LoadScene(0);
    }

    // 장갑선택창으로 이동
    public void ChangeScene1()
    {
        SceneManager.LoadScene(1);
    }


    // 게임신으로 이동
    public void ChangeScene2()
    {
        SceneManager.LoadScene(2);
    }
}
