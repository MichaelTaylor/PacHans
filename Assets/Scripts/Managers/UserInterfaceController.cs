using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceController : MonoBehaviour {

    public GameObject _startScreen;
    public GameObject _gameplayScreen;
    public GameObject _gameOverScreen;

    public void StartScreenToGameplay()
    {
        GameplayManager.instance.Intro(false);
        GameplayManager.instance.SetUpLives();
        GameplayManager.instance.LoadNextScene("Main Scene");
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
        
    }

	public void GameplayToGameOver()
    {
        GameplayManager.instance.LoadNextScene("Game Over Scene");
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(false);
        _gameOverScreen.SetActive(true);
       // GameplayManager.instance._sessionStarted = false;
    }

    public void GameOverToGameplay()
    {
        _startScreen.SetActive(false);
        _gameplayScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.Intro(false);
        GameplayManager.instance.SetUpLives();
        GameplayManager.instance.LoadNextScene("Main Scene");
    }

    public void GameOverToStartScreen()
    {
        _startScreen.SetActive(true);
        _gameplayScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        GameplayManager.instance.LoadNextScene("Start Screen Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
