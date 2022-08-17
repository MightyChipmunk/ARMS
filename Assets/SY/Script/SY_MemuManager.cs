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

    public void GameExit()
    {
        SceneManager.LoadScene("Main");
    }
}
