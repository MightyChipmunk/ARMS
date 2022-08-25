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
            Text warn = GameObject.Find("WarningText").GetComponent<Text>();
            warn.enabled = true;
            warn.gameObject.transform.localScale = Vector3.zero;
            iTween.ScaleTo(warn.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
            iTween.ScaleTo(warn.gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
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
