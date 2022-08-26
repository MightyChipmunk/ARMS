using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SY_RoundNumber : MonoBehaviour
{
    Text roundNumber;
    // Start is called before the first frame update
    void Start()
    {
        roundNumber = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        roundNumber.text = "Round " + (SY_EnemyRoundScore.Instance.EnemyScore + SY_PlayerRoundScore.Instance.PlayerScore + 1);
    }
}
