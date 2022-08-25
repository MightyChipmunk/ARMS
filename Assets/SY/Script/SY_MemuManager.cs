using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
                Time.timeScale = 1;
            }
            else
            {
                menuSet.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void ChangeScene()
    {
        buttonSoundPlayer.GetComponent<JH_ButtonSound>().ChangeScene();
    }

    public void ChangeGameScene()
    {
        if (JH_ArmSelect.Instance.leftHand != null && JH_ArmSelect.Instance.rightHand != null)
        {
            if (buttonSoundPlayer)
            {
                buttonSoundPlayer.GetComponent<JH_ButtonSound>().ChangeGameScene();
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
        else
        {
            GameObject.Find("WarningText").GetComponent<Text>().enabled = true;
        }
    }

    public void ChangeCharacterScene()
    {
        if (GameObject.Find("ArmSelect"))
        {
            Destroy(GameObject.Find("ArmSelect"));
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1;
    }
}
