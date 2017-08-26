using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour {

    public int Rank;
    public float _numScore;
    public string _score;
    public string _fullname;
    private string[] Alphabet =
    {"A","B","C","D","E","F","G",
     "H","I","J","K","L","M","N",
     "O","P","Q","R","S","T","U",
     "V","W","X","Y","Z" }; //Holds the whole alphabet

    public int _currentLetterFirst;// save
    public int _currentLetterMiddle;// save
    public int _currentLetterLast;// save

    private int _initialIndex; //sets up the 3 initials
    public int _letterIndex; //meant to reference the current letter of the alphabet
    public Text _scoreText;
    public Text[] _initials; //gets the 3 initial components

    private float vertical;

    public bool CanInput { get; set; }

    public AudioClip _confirmSFX;

    private void Start()
    {
        //_score = PlayerPrefs.GetFloat("Score" + Rank.ToString()).ToString();
        //_fullname = PlayerPrefs.GetString("Name" + Rank.ToString());
        GetScore();
        CheckScore();
        if (!CanInput)
        {
            CheckScoreText();
            CheckFullName(_fullname);
        }
        else
        {
            ResetName();
        }
        //Debug.Log(PlayerPrefs.GetString("Name" + Rank.ToString())[0]);
    }

    private void GetScore()
    {
        switch(Rank)
        {
            case 1:
                {
                    _numScore = GameplayManager.instance._highScoreManager.TopScore1;
                    break;
                }
            case 2:
                {
                    _numScore = GameplayManager.instance._highScoreManager.TopScore2;
                    break;
                }
            case 3:
                {
                    _numScore = GameplayManager.instance._highScoreManager.TopScore3;
                    break;
                }
            case 4:
                {
                    _numScore = GameplayManager.instance._highScoreManager.TopScore4;
                    break;
                }
            case 5:
                {
                    _numScore = GameplayManager.instance._highScoreManager.TopScore5;
                    break;
                }
        }
    }

    private void Update()
    {
        if (!CanInput) return;
        vertical = Input.GetAxis("Vertical");
        CheckInitialIndex();
        SwitchInitial();

        if (Input.GetButtonDown("Vertical"))
        {
            ChangeLetter(vertical);
        }
    }

    private void CheckInitialIndex()
    {
        //Debug.Log("Check");
        switch(_initialIndex)
        {
            case 0:
                {
                    _initials[0].text = Alphabet[_currentLetterFirst];
                    _initials[0].GetComponent<Flicker>().enabled = true;
                    _initials[1].GetComponent<Flicker>().enabled = false;
                    _initials[2].GetComponent<Flicker>().enabled = false;
                    break;
                }
            case 1:
                {
                    _initials[1].text = Alphabet[_currentLetterMiddle];
                    _initials[0].GetComponent<Flicker>().enabled = false;
                    _initials[1].GetComponent<Flicker>().enabled = true;
                    _initials[2].GetComponent<Flicker>().enabled = false;
                    break;
                }
            case 2:
                {
                    _initials[2].text = Alphabet[_currentLetterLast];
                    _initials[0].GetComponent<Flicker>().enabled = false;
                    _initials[1].GetComponent<Flicker>().enabled = false;
                    _initials[2].GetComponent<Flicker>().enabled = true;
                    break;
                }
        }
    }

    private void ResetName()
    {
        _initials[0].text = "A";
        _initials[1].text = "A";
        _initials[2].text = "A";

        _currentLetterFirst = 0;
        _currentLetterMiddle = 0;
        _currentLetterLast = 0;
    }

    private void CheckScore()
    {
        //_numScore = Score;
        CheckScoreText();
    }

    private void CheckScoreText()
    {
        _scoreText.text = _numScore.ToString();
    }

    private void CheckFullName(string name)
    {
        _initials[0].text = name[0].ToString();
        _initials[1].text = name[1].ToString();
        _initials[2].text = name[2].ToString();
    }

    private void SwitchInitial()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.PlaySFX(_confirmSFX);
            _initialIndex++;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _initialIndex--;
        }

        if (_initialIndex > 2)
        {
            _initialIndex = 2;
            _fullname = _initials[0].text.ToString() + _initials[1].text.ToString() + _initials[2].text.ToString();
            _initials[0].GetComponent<Flicker>().enabled = false;
            _initials[1].GetComponent<Flicker>().enabled = false;
            _initials[2].GetComponent<Flicker>().enabled = false;
            GameplayManager.instance._highScoreManager.UpdateNames();
            SaveRank();
            GameplayManager.instance._userInterfaceController.GameOverToStartScreen();
        }
        else if (_initialIndex < 0)
        {
            _initialIndex = 0;
        }


        /*if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (_initialIndex == 0)
            {
                _initials[0].text = Alphabet[_letterIndex];
            }
            else if (_initialIndex == 1)
            {
                _initials[1].text = Alphabet[_letterIndex];
            }
            else if (_initialIndex == 2)
            {
                _initials[2].text = Alphabet[_letterIndex];
            }
        }0*/
        
    }

    private void ChangeLetter(float _value)
    {
        if (_value > 0)
        {
            if (_initialIndex == 0)
            {
                _currentLetterFirst++;
            }
            else if (_initialIndex == 1)
            {
                _currentLetterMiddle++;
            }
            else if (_initialIndex == 2)
            {
                _currentLetterLast++;
            }
        }
        else if (_value < 0)
        {
            if (_initialIndex == 0)
            {
                _currentLetterFirst--;
            }
            else if (_initialIndex == 1)
            {
                _currentLetterMiddle--;
            }
            else if (_initialIndex == 2)
            {
                _currentLetterLast--;
            }
        }

        if (_currentLetterFirst > Alphabet.Length -1)
        {
            _currentLetterFirst = 0;
        }
        else if (_currentLetterFirst < 0)
        {
            _currentLetterFirst = Alphabet.Length - 1;
        }

        if (_currentLetterMiddle > Alphabet.Length - 1)
        {
            _currentLetterMiddle = 0;
        }
        else if (_currentLetterMiddle < 0)
        {
            _currentLetterMiddle = Alphabet.Length - 1;
        }

        if (_currentLetterLast > Alphabet.Length - 1)
        {
            _currentLetterLast = 0;
        }
        else if (_currentLetterLast < 0)
        {
            _currentLetterLast = Alphabet.Length - 1;
        }
    }

    private void SaveRank()
    {
        //PlayerPrefs.SetFloat("Score" + Rank.ToString(), GameplayManager.instance._highScoreManager.TestScore);
        //PlayerPrefs.SetString("Name" + Rank.ToString(), _initials[0].text + _initials[1].text + _initials[2].text);
        GameplayManager.instance._highScoreManager.Save();
        Debug.Log("Save");
    }
}
