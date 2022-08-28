using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ArmSelect : MonoBehaviour
{
    public AudioClip click;
    AudioSource source;
    public static JH_ArmSelect Instance;
    [Header("PlayerArms")]
    [SerializeField]
    GameObject rightDefault;
    [SerializeField]
    GameObject leftDefault;
    [SerializeField]
    GameObject rightRevolver;
    [SerializeField]
    GameObject leftRevolver;
    [SerializeField]
    GameObject rightFox;
    [SerializeField]
    GameObject leftFox;

    [Header("EnemyArms")]
    [SerializeField]
    GameObject enemyRightDefault;
    [SerializeField]
    GameObject enemyLeftDefault;
    [SerializeField]
    GameObject enemyRightRevolver;
    [SerializeField]
    GameObject enemyLeftRevolver;
    [SerializeField]
    GameObject enemyRightFox;
    [SerializeField]
    GameObject enemyLeftFox;

    GameObject left1;
    GameObject left2;
    GameObject left3;
    GameObject right1;
    GameObject right2;
    GameObject right3;

    public GameObject rightHand;
    public GameObject leftHand;

    public GameObject enemyRightHand;
    public GameObject enemyLeftHand;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        left1 = GameObject.Find("Rotate_Left_Basic");
        right1 = GameObject.Find("Rotate_Right_Basic");
        left2 = GameObject.Find("Left_Revolver");
        right2 = GameObject.Find("Right_Revolver");
        left3 = GameObject.Find("Left_Fox_OnlyModel");
        right3 = GameObject.Find("Right_Fox_OnlyModel");
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        int leftRan = Random.Range(1, 4);
        int rightRan = Random.Range(1, 4);

        if (leftRan == 1)
            enemyLeftHand = enemyLeftDefault;
        else
            enemyLeftHand = enemyLeftDefault;

        if (rightRan == 1)
            enemyRightHand = enemyRightDefault;
        else
            enemyRightHand = enemyRightDefault;
    }

    public void Left1()
    {
        source.PlayOneShot(click);
        leftHand = leftDefault;
        iTween.ScaleTo(left1, iTween.Hash("x", 200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left1, iTween.Hash("x", 150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(left2, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left3, iTween.Hash("x", 250, "y", 250, "z", 250, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Left2()
    {
        source.PlayOneShot(click);
        leftHand = leftRevolver;
        iTween.ScaleTo(left2, iTween.Hash("x", 200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left2, iTween.Hash("x", 150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(left1, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left3, iTween.Hash("x", 250, "y", 250, "z", 250, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Left3()
    {
        source.PlayOneShot(click);
        leftHand = leftFox;
        iTween.ScaleTo(left3, iTween.Hash("x", 500, "y", 500, "z", 500, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left3, iTween.Hash("x", 375, "y", 375, "z", 375, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(left1, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(left2, iTween.Hash("x", 100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Right1()
    {
        source.PlayOneShot(click);
        rightHand = rightDefault;
        iTween.ScaleTo(right1, iTween.Hash("x", -200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right1, iTween.Hash("x", -150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(right2, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right3, iTween.Hash("x", -250, "y", 250, "z", 250, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Right2()
    {
        source.PlayOneShot(click);
        rightHand = rightRevolver;
        iTween.ScaleTo(right2, iTween.Hash("x", -200, "y", 200, "z", 200, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right2, iTween.Hash("x", -150, "y", 150, "z", 150, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(right1, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right3, iTween.Hash("x", -250, "y", 250, "z", 250, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
    public void Right3()
    {
        source.PlayOneShot(click);
        leftHand = rightFox;
        iTween.ScaleTo(right3, iTween.Hash("x", -500, "y", 500, "z", 500, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right3, iTween.Hash("x", -375, "y", 375, "z", 375, "time", 0.3f, "delay", 0.3f, "easetype", iTween.EaseType.easeOutQuint));

        iTween.ScaleTo(right1, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.ScaleTo(right2, iTween.Hash("x", -100, "y", 100, "z", 100, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint));
    }
}
