using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SY_MemuManager : MonoBehaviour
{
    AudioSource source;
    public AudioClip buttonSound;
    GameObject buttonSoundPlayer;
    public GameObject startSet;
    public GameObject menuSet;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonSoundPlayer = GameObject.Find("ButtonSound");
        source = GetComponent<AudioSource>();
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
        buttonSoundPlayer.GetComponent<JH_ButtonSound>().ChangeScene();
    }

    public void ChangeScene1()
    {
        buttonSoundPlayer.GetComponent<JH_ButtonSound>().ChangeGameScene();
    }

    public void ChangeScene2()
    {
        SceneManager.LoadScene(0);
    }


}
