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

    GameObject left1;
    GameObject left2;
    GameObject left3;
    GameObject right1;
    GameObject right2;
    GameObject right3;

    public GameObject rightHand;
    public GameObject leftHand;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        left1 = GameObject.Find("Rotate_Left_Basic");
        right1 = GameObject.Find("Rotate_Right_Basic");
        left2 = GameObject.Find("Left_Revolver");
        right2 = GameObject.Find("Right_Revolver");
        DontDestroyOnLoad(gameObject);
    }

    public void Left1()
    {
        leftHand = leftDefault;
        iTween.ScaleTo(left1, iTween.Hash("x", 200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left1, iTween.Hash("x", 150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(left2, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        //iTween.ScaleTo(left3, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Left2()
    {
        leftHand = leftRevolver;
        iTween.ScaleTo(left2, iTween.Hash("x", 200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left2, iTween.Hash("x", 150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(left1, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        //iTween.ScaleTo(left3, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Left3()
    {

    }
    public void Right1()
    {
        rightHand = rightDefault;
        iTween.ScaleTo(right1, iTween.Hash("x", -200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right1, iTween.Hash("x", -150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(right2, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        //iTween.ScaleTo(right3, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Right2()
    {
        rightHand = rightRevolver;
        iTween.ScaleTo(right2, iTween.Hash("x", -200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right2, iTween.Hash("x", -150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(right1, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        //iTween.ScaleTo(right3, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Right3()
    {

    }
}
