using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ArmSelect : MonoBehaviour
{
    public static JH_ArmSelect Instance;
    [SerializeField]
    GameObject rightDefault;
    [SerializeField]
    GameObject leftDefault;
    [SerializeField]
    GameObject rightRevolver;
    [SerializeField]
    GameObject leftRevolver;

    public GameObject rightHand;
    public GameObject leftHand;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Left1()
    {
        leftHand = leftDefault;
        Debug.Log("adsf");
    }
    public void Left2()
    {
        leftHand = leftRevolver;
    }
    public void Left3()
    {

    }
    public void Right1()
    {
        rightHand = rightDefault;
        Debug.Log("adsf");
    }
    public void Right2()
    {
        rightHand = rightRevolver;
    }
    public void Right3()
    {

    }
}
