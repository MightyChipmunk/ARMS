using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SY_GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] Text countdownText;
    [SerializeField] float setTime = 99.0f;
    GameObject player;
    GameObject enemy;
    [SerializeField]
    GameObject playerM;
    [SerializeField]
    GameObject enemyM;
    [SerializeField]
    GameObject podium;
    [SerializeField]
    GameObject podiumCam;
    [SerializeField]
    GameObject gameUI;
    [SerializeField]
    GameObject podiumUI;

    public AudioClip ko;
    public AudioClip end;

    AudioSource source;

    bool gameEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        podium.SetActive(false);
        podiumUI.SetActive(false);
        countdownText.text = setTime.ToString();
        gameOverText.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (setTime > 0 && JH_Count.Instance.IsStart && !gameEnd)
            setTime -= Time.deltaTime;
        else if (setTime <= 0 && !gameEnd)
        {
            GameOverText(true);
        }

        countdownText.text = Mathf.Round(setTime).ToString();
    }

    public void GameOverText(bool isEnemy)
    {
        gameEnd = true;
        //Time.timeScale = 0.0f;
        //gameOverText.SetActive(true);
        StartCoroutine(DelayText(isEnemy));

    }

    IEnumerator DelayText(bool isEnemy)
    {
        source.PlayOneShot(end);
        // 게임 종료되면 시간 느려짐
        while (Time.timeScale > 0.1f)
        {

            Time.timeScale -= 0.03f;

            yield return null;
        }
        // 약 1초 후에 K.O 텍스트 출력
        yield return new WaitForSeconds(0.1f);
        source.PlayOneShot(ko);
        gameOverText.SetActive(true);
        gameOverText.transform.localScale = Vector3.one * 10;
        iTween.ScaleTo(gameOverText, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.05f, "easetype", iTween.EaseType.easeOutQuint));
        // 1초 후에 게임 속도 원복 및 승리 모션 재생
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;
        yield return new WaitForSeconds(1.5f);
        if (!isEnemy)
        {
            player.GetComponent<JH_PlayerMove>().State = JH_PlayerMove.PlayerState.Win;
            player.GetComponent<Animator>().SetTrigger("Win");
        }
        else
        {
            enemy.GetComponent<JH_PlayerMove>().State = JH_PlayerMove.PlayerState.Win;
            enemy.GetComponent<Animator>().SetTrigger("Win");
        }
        // 3초 후에 포디움 활성화 및 카메라 이동
        yield return new WaitForSeconds(3f);
        podium.SetActive(true);
        podiumUI.SetActive(true);
        if (isEnemy)
        {
            GameObject winner = Instantiate(enemyM);
            winner.transform.position = podium.transform.position;
            winner.transform.rotation = Quaternion.Euler(0, 90, 0);
            podiumCam.GetComponent<JH_LookRotation>().lookRotation = winner.transform;
        }
        else
        {
            GameObject winner = Instantiate(playerM);
            winner.transform.position = podium.transform.position;
            winner.transform.rotation = Quaternion.Euler(0, 90, 0);
            podiumCam.GetComponent<JH_LookRotation>().lookRotation = winner.transform;
        }
        gameUI.SetActive(false);
        player.SetActive(false);
        enemy.SetActive(false);
        iTween.MoveTo(podiumCam, iTween.Hash("x", 1.8, "y", 1.2, "z", -1.4, "time", 2f, "easetype", iTween.EaseType.easeInQuint, "islocal", true));
        iTween.MoveTo(podiumUI.transform.GetChild(0).gameObject, iTween.Hash("x", 460, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuint, "islocal", true));
        yield return new WaitForSeconds(5);
        podiumUI.transform.Find("GameOver").gameObject.SetActive(true);
    }
}
