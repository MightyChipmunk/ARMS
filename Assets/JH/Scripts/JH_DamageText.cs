using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(int damage)
    {
        GetComponent<Text>().text = damage.ToString();
        iTween.MoveTo(gameObject, iTween.Hash(
            "islocal", true, 
            "x", transform.localPosition.x + (float)Random.Range(-499, 500) / 3, 
            "y", transform.localPosition.y + (float)Random.Range(-499, 500) / 3, 
            "time", 0.9f, 
            "easetype", iTween.EaseType.easeOutCirc));
        Destroy(gameObject, 1.0f);
    }
}
