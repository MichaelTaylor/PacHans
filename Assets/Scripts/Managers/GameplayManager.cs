using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
   // public bool _areLivesSetUp { get; set; }
    public float _startingLives;
    private float _lives;
    private float score;
    public float _gainLifeThreshold;
    public Text livesText;
    public Text scoreText;
    public AudioClip _musicIntro;
    public AudioClip _gainLife;
    public AudioClip _clearGame;
    public AudioClip _gameOverSFX;
    public bool _isGameOver;

    public GameObject player;

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
    public static GameplayManager instance;

    // Use this for initialization
    private void Start()
    {
        SingletonFunction();
        //Debug.Log("Start");
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

    public void SetUpLives()
    {
        _lives = _startingLives;
        livesText.text = "Lives: " + _lives.ToString();
    }

    public void UpdateLives(float _addToLives)
    {
        Debug.Log("Lives");
        _lives += _addToLives;
        livesText.text = "Lives: " + _lives.ToString();
    }

    private void Update()
    {
        if (_maxPellets <= 0) return;

        _pelletPercentage = (pellets.Count / _maxPellets);
        CheckMusicLevels(_pelletPercentage);
        CheckScaredTimer();

        if (player == null) return;

        if (score > 0)
        {
            if (pellets.Count <= 0 && !_isGameOver)
            {
                if (!player.GetComponent<PlayerMovement>().isDead)
                {
                    _isGameOver = true;
                    StartWinGame();
                }         
            }
        } 
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
            enemies[i].SetState(EnemyBehavior.EnemyStates.Scared);
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

    public void StartWinGame()
    {
        Time.timeScale = 0.0000001f;
        StartCoroutine(WinGame(3.5f * 0.0000001f));
        AudioManager.instance.PlaySFX(_clearGame);
    }

    private IEnumerator WinGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetValues(true);
    }

    public void StartGameOver()
    {
        StartCoroutine(GameOver(3f));
        _isGameOver = true;
        AudioManager.instance.PlaySFX(_gameOverSFX);
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
            _userInterfaceController.GameplayToGameOver();
        }
    }

    public void LoadNextScene(string _nextScene)
    {
        SceneManager.LoadScene(_nextScene);
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
