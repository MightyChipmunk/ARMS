using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SY_PlayerRoundScore : MonoBehaviour
{
    int _score = 0;
    int _roundScore = 0;
    public static SY_PlayerRoundScore Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int PlayerScore
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            if (_score > _roundScore)
            {
              _roundScore = _score;
            }
        }
    }
    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
