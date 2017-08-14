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

    public static GameplayManager instance;

    // Use this for initialization
    private void Start()
    {
        SingletonFunction();
        PoolSmoke();
        Intro();
        scoreText.text = "Score: " + score.ToString();
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
        _listOfSmoke[Random.Range(0, _listOfSmoke.Count)].SetActive(false);
    }

    private void Intro()
    {
        Time.timeScale = 0f;
        AudioManager.instance.PlaySFX(_musicIntro);
        StartCoroutine(TimeResume(4.5f));
    }

    private IEnumerator TimeResume(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1f;
    }

    public void UpdateScore(float addScore)
    {
        score += addScore;
        scoreText.text = "Score: " + score.ToString();
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

    public void StartGameOver()
    {
        if (!_isGameOver) return;
        StartCoroutine(GameOver(3f));
        AudioManager.instance.PlaySFX(_gameOverSFX);
    }

    private IEnumerator GameOver(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Game Over Scene");
    }
}
