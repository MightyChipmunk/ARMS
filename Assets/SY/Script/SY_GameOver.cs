using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SY_GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] Text countdownText;
    [SerializeField] float setTime = 99.0f;
    // Start is called before the first frame update
    void Start()
    {
        countdownText.text = setTime.ToString();
        gameOverText.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (setTime > 0)
            setTime -= Time.deltaTime;
        else if (setTime <= 0)
        {
            Time.timeScale = 0.0f;
            gameOverText.SetActive(true);
        }

        countdownText.text = Mathf.Round(setTime).ToString();
    }

}
