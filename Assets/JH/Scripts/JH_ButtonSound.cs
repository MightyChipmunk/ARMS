using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JH_ButtonSound : MonoBehaviour
{
    AudioSource source;
    public AudioClip buttonSound;
    public AudioClip starSound;
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
        GameObject.Find("Start Button").GetComponent<Button>().enabled = false;

        GameObject.Find("Player").GetComponent<Animator>().SetTrigger("Selected");
        GameObject.Find("Enemy").GetComponent<Animator>().SetTrigger("Selected");

        //iTween.ScaleTo(GameObject.Find("Main"), iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f, "easetype", iTween.EaseType.easeInCirc));
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
        yield return new WaitForSeconds(2f);
        GameObject.Find("Player").transform.Find("Star").GetComponent<ParticleSystem>().Play();
        GameObject.Find("Enemy").transform.Find("Star").GetComponent<ParticleSystem>().Play();
        source.PlayOneShot(starSound);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}
