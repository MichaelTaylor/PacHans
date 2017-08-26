using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceController : MonoBehaviour {

    public GameObject _startScreen;
    public GameObject _gameplayScreen;
    public GameObject _highScoreScreen;
    public GameObject _gameOverScreen;

    public void StartScreenToGameplay()
    {
        GameplayManager.instance.Intro(false);
        GameplayManager.instance.SetUpLives();
        GameplayManager.instance.UpdateLevelNum(1);
        GameplayManager.instance.LoadNextScene("Main Scene");
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(true);
        _highScoreScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        
    }

    public void StartScreenToHighScore()
    {
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(false);
        _highScoreScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.LoadNextScene("High Score Scene");
        StartCoroutine(StartStarScreenTimer(5f));
    }

    public void HighScoreToStartScreen()
    {
        _startScreen.SetActive(true);
        _gameplayScreen.SetActive(false);
        _highScoreScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.LoadNextScene("Start Screen Scene");
    }

    private IEnumerator StartStarScreenTimer(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        HighScoreToStartScreen();
    }

	public void GameplayToGameOver(float score)
    {
        if (score >= GameplayManager.instance._highScoreManager.TopScore5)
        {
            _startScreen.SetActive(false);
            _gameplayScreen.SetActive(false);
            _highScoreScreen.SetActive(true);
            _gameOverScreen.SetActive(false);
            GameplayManager.instance._highScoreManager.EvaluateNewScore(score);
            GameplayManager.instance.LoadNextScene("High Score Scene");
        }
        else
        {
            GameplayManager.instance.LoadNextScene("Game Over Scene");
            _startScreen.SetActive(false);
            _gameplayScreen.SetActive(false);
            _highScoreScreen.SetActive(false);
            _gameOverScreen.SetActive(true);
            StartCoroutine(StartGameOverTimer(3f));
            // GameplayManager.instance._sessionStarted = false;
        }
    }

    private IEnumerator StartGameOverTimer(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        GameOverToStartScreen();
    }

    public void GameOverToGameplay()
    {
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(true);
        _highScoreScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.Intro(false);
        GameplayManager.instance.SetUpLives();
        GameplayManager.instance.LoadNextScene("Main Scene");
    }

    public void GameOverToStartScreen()
    {
        _startScreen.SetActive(true);
        _gameplayScreen.SetActive(false);
        _highScoreScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.ResetScore();
        GameplayManager.instance.LoadNextScene("Start Screen Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
