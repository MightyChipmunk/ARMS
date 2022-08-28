using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JH_ButtonSound : MonoBehaviour
{
    AudioSource source;
    public AudioClip buttonSound;
    GameObject cam1;
    GameObject cam2;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        cam1 = GameObject.Find("Main Camera");
        cam2 = GameObject.Find("Main Camera (1)");

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene()
    {
        source.PlayOneShot(buttonSound);
        GameObject.Find("Start Button").SetActive(false);

        iTween.MoveTo(cam1, iTween.Hash("z", 0.8f, "time", 1.5f, "easetype", iTween.EaseType.easeInCirc));
        iTween.MoveTo(cam2, iTween.Hash("z", 0.8f, "time", 1.5f, "easetype", iTween.EaseType.easeInCirc));

        StartCoroutine("ILoadScene");
    }

    public void ChangeGameScene()
    {
        SceneManager.LoadScene(2);
        Destroy(GameObject.Find("Main Camera"));
        Destroy(gameObject);
    }

    IEnumerator ILoadScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }
}
