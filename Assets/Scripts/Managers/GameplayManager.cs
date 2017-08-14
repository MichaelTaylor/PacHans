﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    private float score;
    public Text scoreText;

    public GameObject player;

    public List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    public List<GameObject> pellets = new List<GameObject>();
    public List<Transform> powerUpTransform = new List<Transform>();

    public Transform _enemyReturnPoint;
    
    public bool poweredUp;
    public float powerUpDuration;

    public static GameplayManager instance;

    // Use this for initialization
    private void Start()
    {
        SingletonFunction();
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
}