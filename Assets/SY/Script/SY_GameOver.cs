using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SY_GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject gameOverText;
    [SerializeField] Text countdownText;
    [SerializeField] float setTime = 99.0f;


    bool gameEnd = true;

    // Start is called before the first frame update
    void Start()
    {
        countdownText.text = setTime.ToString();
        gameOverText.SetActive(false);
        gameOver.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (setTime > 0 && JH_Count.Instance.IsStart)
            setTime -= Time.deltaTime;
        else if (setTime <= 0 && gameEnd)
        {
            gameEnd = false;
            GameOverText();
        }

        countdownText.text = Mathf.Round(setTime).ToString();
    }

    public void GameOverText()
    {
        //Time.timeScale = 0.0f;
        //gameOverText.SetActive(true);
        StartCoroutine("DelayText");
    }

    IEnumerator DelayText()
    {
        // 씬 딜레이
        while (Time.timeScale > 0.1f)
        {

            Time.timeScale -= 0.1f;

            yield return null;
        }

        //KO텍스트 딜레이 표시
        yield return new WaitForSeconds(0.3f);
        gameOverText.SetActive(true);

        //게임 오버 UI 출력
        yield return new WaitForSeconds(0.4f);
        gameOver.SetActive(true);
    }
    
}
