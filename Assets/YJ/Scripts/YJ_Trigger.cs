using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Trigger : MonoBehaviour
{
    Vector3 localpos;
    GameObject enemy;
    CharacterController cc;
    public bool enemyCome;

    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy");
        cc = enemy.GetComponent<CharacterController>();
        localpos = transform.localPosition;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(enemyCome)
        {
            enemy.transform.position = transform.position;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Enemy"))
        {
            cc.enabled = false;
            enemyCome = true;
        }
    }

}
