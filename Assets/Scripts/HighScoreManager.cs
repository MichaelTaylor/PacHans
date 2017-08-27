using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {

    public float TopScore1;
    public float TopScore2;
    public float TopScore3;
    public float TopScore4;
    public float TopScore5;

    public string TopScoreName1;
    public string TopScoreName2;
    public string TopScoreName3;
    public string TopScoreName4;
    public string TopScoreName5;

    public HighScoreController _controller1;
    public HighScoreController _controller2;
    public HighScoreController _controller3;
    public HighScoreController _controller4;
    public HighScoreController _controller5;

    public float TestScore;

    private void Start()
    {
        Load();   
        //EvaluateNewScore(TestScore);
    }

    public void Save()
    {
        SaveManager.SaveData(this);
    }

    private void Load()
    {
        float[] scores = SaveManager.LoadScores();
        string [] names = SaveManager.LoadNames();

        GetScores(scores);
        GetNames(names);
    }

    private void GetScores(float [] scores)
    {
        TopScore1 = scores[0];
        _controller1._numScore = TopScore1;

        TopScore2 = scores[1];
        _controller2._numScore = TopScore2;

        TopScore3 = scores[2];
        _controller3._numScore = TopScore3;

        TopScore4 = scores[3];
        _controller4._numScore = TopScore4;

        TopScore5 = scores[4];
        _controller5._numScore = TopScore5;
    }

    private void GetNames(string [] names)
    {
        TopScoreName1 = names[0];
        _controller1._fullname = TopScoreName1;

        TopScoreName2 = names[1];
        _controller2._fullname = TopScoreName2;

        TopScoreName3 = names[2];
        _controller3._fullname = TopScoreName3;

        TopScoreName4 = names[3];
        _controller4._fullname = TopScoreName4;

        TopScoreName5 = names[4];
        _controller5._fullname = TopScoreName5;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            SaveManager.SaveData(this);
            Debug.Log("Save");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.LoadScores();
            SaveManager.LoadNames();
            Debug.Log("Load");
        }
    }

    public void UpdateScoreBoard()
    {
        _controller1._scoreText.text = TopScore1.ToString();
        _controller2._scoreText.text = TopScore2.ToString();
        _controller3._scoreText.text = TopScore3.ToString();
        _controller4._scoreText.text = TopScore4.ToString();
        _controller5._scoreText.text = TopScore5.ToString();
    }

    public void UpdateNames()
    {
        TopScoreName1 = _controller1._fullname;
        TopScoreName2 = _controller2._fullname;
        TopScoreName3 = _controller3._fullname;
        TopScoreName4 = _controller4._fullname;
        TopScoreName5 = _controller5._fullname;

        /* _controller1.CheckFullName(_controller1._fullname);
         _controller2.CheckFullName(_controller2._fullname);
         _controller3.CheckFullName(_controller3._fullname);
         _controller4.CheckFullName(_controller4._fullname);
         _controller5.CheckFullName(_controller5._fullname);*/
    }

    public void EvaluateNewScore(float _newScore)
    {
        if (_newScore >= TopScore1)
        {
            TopScore5 = TopScore4;
            TopScoreName5 = TopScoreName4;

            TopScore4 = TopScore3;
            TopScoreName4 = TopScoreName3;

            TopScore3 = TopScore2;
            TopScoreName3 = TopScoreName2;

            TopScore2 = TopScore1;
            TopScoreName2 = TopScoreName1;

            TopScore1 = _newScore;
            InitiateInput(1);
        }
        else if (_newScore < TopScore1 && _newScore >= TopScore2)
        {
            TopScore5 = TopScore4;
            TopScoreName5 = TopScoreName4;

            TopScore4 = TopScore3;
            TopScoreName4 = TopScoreName3;

            TopScore3 = TopScore2;
            TopScoreName3 = TopScoreName2;

            TopScore2 = _newScore;
            InitiateInput(2);
        }
        else if (_newScore < TopScore2 && _newScore >= TopScore3)
        {
            TopScore5 = TopScore4;
            TopScoreName5 = TopScoreName4;

            TopScore4 = TopScore3;
            TopScoreName4 = TopScoreName3;

            TopScore3 = _newScore;
            InitiateInput(3);
        }
        else if (_newScore < TopScore3 && _newScore >= TopScore4)
        {
            TopScore5 = TopScore4;
            TopScoreName5 = TopScoreName4;

            TopScore4 = _newScore;
            InitiateInput(4);
        }
        else if (_newScore < TopScore4 && _newScore >= TopScore5)
        {
            TopScore5 = _newScore;
            InitiateInput(5);
        }

        UpdateScoreBoard();
    }

    private void InitiateInput(int _rankIndex)
    {
        switch(_rankIndex)
        {
            case 1:
                {
                    _controller1.CanInput = true;
                    break;
                }
            case 2:
                {
                    _controller2.CanInput = true;
                    break;
                }
            case 3:
                {
                    _controller3.CanInput = true;
                    break;
                }
            case 4:
                {
                    _controller4.CanInput = true;
                    break;
                }
            case 5:
                {
                    _controller5.CanInput = true;
                    break;
                }
        }
    }
}
