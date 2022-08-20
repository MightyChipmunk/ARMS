using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JH_ButtonSound : MonoBehaviour
{
    AudioSource source;
    public AudioClip buttonSound;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene()
    {
        source.PlayOneShot(buttonSound);
        SceneManager.LoadScene(1);
    }

    public void ChangeGameScene()
    {
        SceneManager.LoadScene(2);
        Destroy(GameObject.Find("Main Camera"));
        Destroy(gameObject);
    }
}
