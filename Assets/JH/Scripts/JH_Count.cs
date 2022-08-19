using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_Count : MonoBehaviour
{
    Text count;
    bool isStart = false;
    public bool IsStart { get { return isStart; } set { isStart = value; } }

    // Start is called before the first frame update
    void Start()
    {
        count = gameObject.GetComponent<Text>();
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
        iTween.ScaleTo(count.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.9f, "easetype", iTween.EaseType.easeOutCirc));
        yield return new WaitForSeconds(1.0f);
        count.transform.localScale = Vector3.zero;
        count.text = "2";
        iTween.ScaleTo(count.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.9f, "easetype", iTween.EaseType.easeOutCirc));
        yield return new WaitForSeconds(1.0f);
        count.transform.localScale = Vector3.zero;
        count.text = "1";
        iTween.ScaleTo(count.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.9f, "easetype", iTween.EaseType.easeOutCirc));
        yield return new WaitForSeconds(1.0f);
        count.text = "Go!";
        isStart = true;
        yield return new WaitForSeconds(1.0f);
        count.enabled = false;
    }
}
