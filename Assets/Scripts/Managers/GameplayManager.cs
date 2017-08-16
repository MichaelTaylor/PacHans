using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    private float score;
    public Text scoreText;
    public AudioClip _musicIntro;
    public AudioClip _clearGame;
    public AudioClip _gameOverSFX;
    public bool _isGameOver;

    public GameObject player;

    public List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    public List<GameObject> pellets = new List<GameObject>();
    public List<Transform> powerUpTransform = new List<Transform>();

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
        Debug.Log("Start");
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

    public void Intro()
    {
        Time.timeScale = 0.0000001f;
        AudioManager.instance.PlaySFX(_musicIntro);
        scoreText.text = "Score: " + score.ToString();
        StartCoroutine(TimeResume(0.0000001f * 4.5f));
    }

    private IEnumerator TimeResume(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PoolSmoke();
        Time.timeScale = 1f;
    }

    public void UpdateScore(float addScore)
    {
        score += addScore;
        scoreText.text = "Score: " + score.ToString();
    }

    private void Update()
    {
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

    public void PowerUp()
    {
        poweredUp = true;
        StartCoroutine(BackToNormal(powerUpDuration));
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetState(EnemyBehavior.EnemyStates.Scared);
        }
    }

    private IEnumerator BackToNormal(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ReturnToLife();
        }
    }

    public void StartWinGame()
    {
        StartCoroutine(WinGame(3f));
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
        AudioManager.instance.PlaySFX(_gameOverSFX);
    }

    private IEnumerator GameOver(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetValues(false);
        _userInterfaceController.GameplayToGameOver();
    }

    public void LoadNextScene(string _nextScene)
    {
        SceneManager.LoadScene(_nextScene);
    }

    private void ResetValues(bool _didWin)
    {
        enemies.Clear();
        pellets.Clear();
        powerUpTransform.Clear();
        _listOfSmoke.Clear();

        if (!_didWin) return;
        Time.timeScale = 0.0000001f;
        StartCoroutine(TimeResume(0.0000001f * 4.5f));
        GameplayManager.instance.LoadNextScene("Main Scene");
    }
}
