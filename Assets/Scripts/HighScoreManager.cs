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

    public HighScoreController _controller1;
    public HighScoreController _controller2;
    public HighScoreController _controller3;
    public HighScoreController _controller4;
    public HighScoreController _controller5;

    public float TestScore;

    private void Start()
    {
        //EvaluateNewScore(TestScore);
    }

    public void EvaluateNewScore(float _newScore)
    {
        if (_newScore > TopScore1)
        {
            TopScore5 = TopScore4;
            TopScore4 = TopScore3;
            TopScore3 = TopScore2;
            TopScore2 = TopScore1;
            TopScore1 = _newScore;
            InitiateInput(1);
        }
        else if (_newScore < TopScore1 && _newScore > TopScore2)
        {
            TopScore5 = TopScore4;
            TopScore4 = TopScore3;
            TopScore3 = TopScore2;
            TopScore2 = _newScore;
            InitiateInput(2);
        }
        else if (_newScore < TopScore2 && _newScore > TopScore3)
        {
            TopScore5 = TopScore4;
            TopScore4 = TopScore3;
            TopScore3 = _newScore;
            InitiateInput(3);
        }
        else if (_newScore < TopScore3 && _newScore > TopScore4)
        {
            TopScore5 = TopScore4;
            TopScore4 = _newScore;
            InitiateInput(4);
        }
        else if (_newScore < TopScore4 && _newScore > TopScore5)
        {
            TopScore5 = _newScore;
            InitiateInput(5);
        }

        //UpdateScoreBoard();
    }

    public void UpdateScoreBoard()
    {
        _controller1._scoreText.text = TopScore1.ToString();
        _controller2._scoreText.text = TopScore2.ToString();
        _controller3._scoreText.text = TopScore3.ToString();
        _controller4._scoreText.text = TopScore4.ToString();
        _controller5._scoreText.text = TopScore5.ToString();
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
