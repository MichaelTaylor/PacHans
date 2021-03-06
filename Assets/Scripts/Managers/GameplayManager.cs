﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    // public bool _areLivesSetUp { get; set; }
    public float _startingLives;
    private float _lives;
    public float _levelNum;
    private float score;
    public float _gainLifeThreshold;
    public Text livesText;
    public Text levelText;
    public Text scoreText;
    public AudioClip _musicIntro;
    public AudioClip _gainLife;
    public AudioClip _clearGame;
    public AudioClip _gameOverSFX;
    public bool _isGameOver;

    public GameObject player;

    public TransitionBehavior _transitionBehavior;

    public List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    public List<GameObject> pellets = new List<GameObject>();
    public List<Transform> powerUpTransform = new List<Transform>();

    public float _scaredTimer;

    public float _maxPellets;
    public float _pelletPercentage;

    public Transform _enemyReturnPoint;

    public bool poweredUp;
    public float powerUpDuration;

    public GameObject _smokeObject;
    public int _smokeAmount;
    private List<GameObject> _listOfSmoke = new List<GameObject>();

    public UserInterfaceController _userInterfaceController;
    public HighScoreManager _highScoreManager;
    public static GameplayManager instance;

    [Header("Menu control variables")]
    

    //UI VARIABLES
    private int _startScreenIndex;
    public Button _startButton;
    public Sprite _startDefault;
    public Sprite _startHighlight;
    public Button _highScoreButton;
    public Sprite _highscoreDefault;
    public Sprite _highScoreHighlight;

    public enum GameState
    {
        StartMenu,
        Gameplay,
        HighScore
    }
    public GameState _gameState;

    [Header("Transition Variables")]
    public GameObject _background;
    public GameObject _ppu512Logo;
    public GameObject _coorsLogo;
    public Color _coorsColor;
    public Color _logoColor;

    // Use this for initialization
    private void Start()
    {
        SingletonFunction();
        _background.SetActive(true);
    }

    private void SingletonFunction()
    {
        if (GameplayManager.instance == null)
        {
            GameplayManager.instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    private void PoolSmoke()
    {
        for (int i = 0; i < _smokeAmount; i++)
        {
            GameObject _newSmokeObject = Instantiate(_smokeObject, transform.position, Quaternion.identity);
            _newSmokeObject.SetActive(false);
            _listOfSmoke.Add(_newSmokeObject);
        }
    }

    public void ShowSmoke(Vector2 _smokeTransform)
    {
        int _smokeIndex = Random.Range(0, _listOfSmoke.Count);
        _listOfSmoke[_smokeIndex].transform.position = _smokeTransform;
        _listOfSmoke[_smokeIndex].SetActive(true);
        
    }

    public void Intro(bool _areLivesSetUp)
    {
        Time.timeScale = 0.0000001f;
        AudioManager.instance.PlaySFX(_musicIntro);
        scoreText.text = "Score: " + score.ToString();
        StartCoroutine(TimeResume(0.0000001f * 4.5f));
        _isGameOver = false;
        if (_areLivesSetUp) return;
        SetUpLives();
    }

    private IEnumerator TimeResume(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PoolSmoke();
        Time.timeScale = 1f;
        Invoke("GetMaxPellets",0.1f);
    }

    private void GetMaxPellets()
    {
        _maxPellets = pellets.Count;
        player.GetComponent<PlayerMovement>().PlayNormalSFX();
        player.GetComponent<PlayerMovement>()._audioSource.mute = false;
    }

    public void UpdateScore(float addScore)
    {
        score += addScore;
        scoreText.text = "Score: " + score.ToString();

        if (score % _gainLifeThreshold == 0)
        {
            UpdateLives(1);
            AudioManager.instance.PlaySFX(_gainLife);
        }
    }

    public void UpdateLevelNum(float _addToLevel)
    {
        _levelNum += _addToLevel;
        levelText.text = "Level: " + _levelNum.ToString();
    }

    public void SetUpLives()
    {
        _lives = _startingLives;
        livesText.text = "Lives: " + _lives.ToString();
    }

    public void UpdateLives(float _addToLives)
    {
        _lives += _addToLives;
        livesText.text = "Lives: " + _lives.ToString();
    }

    private void Update()
    {
        CheckGameState();
    }

    public void SetGameState(GameState _newState)
    {
        _gameState = _newState;
    }

    private void CheckGameState()
    {
        switch(_gameState)
        {
            case GameState.StartMenu:
                {
                    StartMenuBehavior();
                    break;
                }
            case GameState.Gameplay:
                {
                    GameplayBehavior();
                    break;
                }
            case GameState.HighScore:
                {
                    break;
                }
        }
    }

    private void StartMenuBehavior()
    {
        if (Input.GetAxis("Vertical") > 0 && Input.GetButtonDown("Vertical"))
        {
            _startScreenIndex -= 1;
        }
        else if (Input.GetAxis("Vertical") < 0 && Input.GetButtonDown("Vertical"))
        {
            _startScreenIndex += 1;
        }

        if (_startScreenIndex > 1)
        {
            _startScreenIndex = 0;
        }
        else if (_startScreenIndex < 0)
        {
            _startScreenIndex = 1;
        }

        if (Input.GetButtonDown("Submit"))
        {
            if (_startScreenIndex == 0)
            {
                Debug.Log("Go to Gameplay");
                //_userInterfaceController.StartScreenToGameplay(3f);
                _startButton.onClick.Invoke();
                //ShowCoorsLogo();
            }
            else
            {
                Debug.Log("Go to High Score");
                _highScoreButton.onClick.Invoke();
                //_userInterfaceController.StartScreenToHighScore();
            }
        }

        if (_startScreenIndex == 0)
        {
            //_startButton.Select();
            _startButton.GetComponent<Image>().sprite = _startHighlight;
            _highScoreButton.GetComponent<Image>().sprite = _highscoreDefault;
        }
        else
        {
            //_highScoreButton.Select();
            _startButton.GetComponent<Image>().sprite = _startDefault;
            _highScoreButton.GetComponent<Image>().sprite = _highScoreHighlight;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            //_userInterfaceController.QuitGame();
        }
    }

    private void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("[enter]")))
        {
            Debug.Log("Enter");
        }
    }

    private void GameplayBehavior()
    {
        if (_isGameOver) return;

        _pelletPercentage = (pellets.Count / _maxPellets);
        CheckMusicLevels(_pelletPercentage);
        CheckScaredTimer();
        // Debug.Log(pellets.Count);

        if (player == null) return;
        TriggerWinConditions();
    }

    public void ShowCoorsLogo()
    {
        _coorsLogo.SetActive(true);
        _background.SetActive(false);
        Camera.main.backgroundColor = _coorsColor;
    }

    public void ShowPPU512Logo()
    {
        _ppu512Logo.SetActive(true);
        _background.SetActive(false);
        Camera.main.backgroundColor = _logoColor;
    }

    public void ResetTransition()
    {
        _ppu512Logo.SetActive(false);
        _coorsLogo.SetActive(false);
        _background.SetActive(true);
    }

    private void AddToScaredTimer(float _additionalTime)
    {
        _scaredTimer += _additionalTime;
    }

    private void CheckScaredTimer()
    {
        if (!poweredUp) return;
        if (_scaredTimer > 0)
        {
            _scaredTimer -= Time.deltaTime;
        }
        else
        {
            BackToNormal();
        }
    }

    public void CheckMusicLevels(float percentage)
    {
        if (_isGameOver) return;
        if (percentage > 0.8f)
        {
            AudioManager.instance.Lv1Music();
        }
        else if (percentage < 0.8f && percentage > 0.6f)
        {
            AudioManager.instance.Lv2Music();
        }
        else if (percentage < 0.6f && percentage > 0.4f)
        {
            AudioManager.instance.Lv3Music();
        }
        else if (percentage < 0.4f && percentage > 0.2f)
        {
            AudioManager.instance.Lv4Music();
        }
        else if (percentage < 0.2f && percentage > 0.01f)
        {
            AudioManager.instance.Lv5Music();
        }
        else
        {
            AudioManager.instance.MuteAllMusic();
        }    
    }

    public void PowerUp()
    {
        poweredUp = true;
        //StartCoroutine(BackToNormal(powerUpDuration));
        AddToScaredTimer(powerUpDuration);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].CheckIfScared();
        }

        player.GetComponent<PlayerMovement>().PlayPowerSFX();
    }

    private void BackToNormal()
    {
        poweredUp = false;
        player.GetComponent<PlayerMovement>().PlayNormalSFX();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ReturnToLife();
        }
    }

    public void TriggerWinConditions()
    {
        if (score > 0)
        {
            if (pellets.Count < 1 && !_isGameOver)
            {
                if (!player.GetComponent<PlayerMovement>().isDead)
                {
                    _isGameOver = true;
                    StartWinGame();
                    AudioManager.instance.MuteAllMusic();
                    player.GetComponent<PlayerMovement>()._audioSource.mute = true;
                }
            }
        }
    }

    public void StartWinGame()
    {
        Time.timeScale = 0.0000001f;
        StartCoroutine(WinGame(4f * 0.0000001f));
        AudioManager.instance.PlaySFX(_clearGame);
        //_transitionBehavior.CanMove = true;
        //_transitionBehavior._transitionSpeed = 50000000;
    }

    private IEnumerator WinGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        UpdateLevelNum(1);
        ResetValues(true);
    }

    public void StartGameOver()
    {
        StartCoroutine(GameOver(3f));
        _isGameOver = true;
        AudioManager.instance.PlaySFX(_gameOverSFX);
        _transitionBehavior.CanMove = true;
        _transitionBehavior._speedMultiplyer = 1;
    }

    private IEnumerator GameOver(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (_lives > 0)
        {
            ResetValues(true);
        }
        else
        {
            ResetValues(false);
            _levelNum = 0;
            UpdateLevelNum(0);
            _userInterfaceController.GameplayToGameOver(score);
        }
    }

    public void LoadNextScene(string _nextScene)
    {
        SceneManager.LoadScene(_nextScene);
    }

    public void ResetScore()
    {
        //Intro(true);
        _isGameOver = false;
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    private void ResetValues(bool _gameNotDone)
    {
        enemies.Clear();
        pellets.Clear();
        powerUpTransform.Clear();
        _listOfSmoke.Clear();

        if (!_gameNotDone) return;
        Intro(true);
        _isGameOver = false;
        GameplayManager.instance.LoadNextScene("Main Scene");
    }
}
