using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Damage : MonoBehaviour
{
    [SerializeField]
    GameObject textFactory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void FloatText(int damage)
    {
        GameObject damageText = Instantiate(textFactory);
        damageText.transform.SetParent(transform);
        damageText.transform.localScale = Vector3.one;
        damageText.transform.position = transform.position;
        damageText.transform.rotation = transform.rotation;
        damageText.GetComponent<JH_DamageText>().SetText(damage);
    }
}
