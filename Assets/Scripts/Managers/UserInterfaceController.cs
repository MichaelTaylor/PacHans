using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceController : MonoBehaviour {

    public GameObject _startScreen;
    public GameObject _gameplayScreen;
    public GameObject _highScoreScreen;
    public GameObject _gameOverScreen;

    public void StartScreenToGameplay(float _seconds)
    {
        StartCoroutine(TransitionStartToGameplay(_seconds));
        GameplayManager.instance.ShowCoorsLogo();
    }

    public IEnumerator TransitionStartToGameplay(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        GameplayManager.instance.Intro(false);
        GameplayManager.instance.SetUpLives();
        GameplayManager.instance.UpdateLevelNum(1);
        GameplayManager.instance.LoadNextScene("Main Scene");
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(true);
        _highScoreScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.SetGameState(GameplayManager.GameState.Gameplay);
        GameplayManager.instance.ResetTransition();
    }

    public void StartScreenToHighScore()
    {
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(false);
        _highScoreScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.LoadNextScene("High Score Scene");
        StartCoroutine(StartStarScreenTimer(4f));
        GameplayManager.instance.SetGameState(GameplayManager.GameState.HighScore);
    }

    public IEnumerator HighScoreToStartScreen(float _seconds, bool _showLogo)
    {
        yield return new WaitForSeconds(_seconds);
        _startScreen.SetActive(true);
        _gameplayScreen.SetActive(false);
        _highScoreScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.LoadNextScene("Start Screen Scene");
        GameplayManager.instance.SetGameState(GameplayManager.GameState.StartMenu);
        GameplayManager.instance.ResetTransition();
    }

    private IEnumerator StartStarScreenTimer(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        StartCoroutine(HighScoreToStartScreen(0f,false));
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
        GameOverToStartScreen(0f,false);
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
        GameplayManager.instance.ResetTransition();
    }

    public void GameOverToStartScreen(float _seconds, bool _showLogo)
    {
        StartCoroutine(TransitionGameOverToStart(_seconds));
        if (!_showLogo) return;
        GameplayManager.instance.ShowPPU512Logo();
        Destroy(GameObject.Find("Filler background"));
    }

    public IEnumerator TransitionGameOverToStart(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _startScreen.SetActive(true);
        _gameplayScreen.SetActive(false);
        _highScoreScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.ResetScore();
        GameplayManager.instance.LoadNextScene("Start Screen Scene");
        GameplayManager.instance.SetGameState(GameplayManager.GameState.StartMenu);
        GameplayManager.instance.ResetTransition();
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
