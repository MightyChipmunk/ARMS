using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_UIAnim : MonoBehaviour
{
    Vector3 originScale;
    // Start is called before the first frame update
    void Start()
    {
        originScale = transform.localScale;
    }
    float currentTime = 0;
    float moveTime = 2.5f;
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > moveTime && gameObject.transform.localScale.x >= originScale.x)
        {
            iTween.ScaleTo(gameObject, iTween.Hash("x", originScale.x * 1.3f, "y", originScale.y * 1.3f, "z", originScale.z * 1.3f, "time", 0.7f, "easetype", iTween.EaseType.easeOutBounce));
            iTween.ScaleTo(gameObject, iTween.Hash("x", originScale.x, "y", originScale.y, "z", originScale.z, "time", 0.7f, "delay", 0.7f, "easetype", iTween.EaseType.easeOutExpo));
            currentTime = 0;
        }
    }
}
