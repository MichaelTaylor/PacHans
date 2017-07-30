using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
	private float score;
    public Text scoreText;

    public GameObject player;
    public List<GameObject> enemies = new List<GameObject>();

    public bool poweredUp;

    public static GameplayManager instance;

	private void SingletonFunction()
	{
		if (GameplayManager.instance == null)
		{
			GameplayManager.instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	private void Start()
	{
		SingletonFunction();
        scoreText.text = "Score: " + score.ToString();
	}

	public void UpdateScore(float addScore)
	{
		score += addScore;
        scoreText.text = "Score: " + score.ToString();
	}

    public void PowerUp()
    {
        poweredUp = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            //TODO: make them scared cats
        }
    }
}
