using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SY_MemuManager : MonoBehaviour
{
    public GameObject startSet;
    public GameObject menuSet;
    public GameObject okButton;
    
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
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeScene1()
    {
        SceneManager.LoadScene(2);
    }

    public void ChangeScene2()
    {
        SceneManager.LoadScene(0);
    }
}
