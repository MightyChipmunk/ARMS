using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_Count : MonoBehaviour
{
    public AudioClip three;
    public AudioClip two;
    public AudioClip one;
    public AudioClip bell;

    AudioSource source;

    public static JH_Count Instance { get; private set; }

    Text count;
    bool isStart = false;
    public bool IsStart { get { return isStart; } set { isStart = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
        count = gameObject.GetComponent<Text>();
        StartCoroutine("CountDown");
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            
        }

    }

    IEnumerator CountDown()
    {
        count.transform.localScale = Vector3.zero;
        count.text = "3";
        source.PlayOneShot(three);
        iTween.ScaleTo(count.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.9f, "easetype", iTween.EaseType.easeOutCirc));
        yield return new WaitForSeconds(1.0f);
        count.transform.localScale = Vector3.zero;
        count.text = "2";
        source.PlayOneShot(two);
        iTween.ScaleTo(count.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.9f, "easetype", iTween.EaseType.easeOutCirc));
        yield return new WaitForSeconds(1.0f);
        count.transform.localScale = Vector3.zero;
        count.text = "1";
        source.PlayOneShot(one);
        iTween.ScaleTo(count.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.9f, "easetype", iTween.EaseType.easeOutCirc));
        yield return new WaitForSeconds(1.0f);
        source.PlayOneShot(bell);
        count.transform.localScale = Vector3.zero;
        count.text = "A R M S !";
        iTween.ScaleTo(count.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.9f, "easetype", iTween.EaseType.easeOutCirc));
        isStart = true;
        yield return new WaitForSeconds(1.0f);
        count.enabled = false;
    }
}
