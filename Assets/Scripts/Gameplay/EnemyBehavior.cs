﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyNavAgent))]
public class EnemyBehavior : MonoBehaviour {

    private int _newDestinationIndex;
    private int _oldDestinationIndex;
    private bool isDead;
    private Transform Destination;
    public float _score;
    public float _normalSpeed;
    public float _scaredSpeed;
    public float _deadSpeed;
    private float _distance;
    public float _distanceThreshold;
    public float _BehaviourTime;
    public float _maxBehaviourTime;
    public float _runMultiplier;
    public AudioClip _deathSFX;
    public enum EnemyStates
    {
        Idle,
        Wandering,
        Chasing,
        Scared,
        Dead
    };
    public EnemyStates _enemyStates;

    private Transform _runTo;

    private PolyNavAgent _polyNavAgent;
    private Animator _anim;

    // Use this for initialization
	private void Start()
    {     
        Invoke("AddToList", 0.1f);
        GetProperties();
        _polyNavAgent.maxSpeed = _normalSpeed;
    }

    private void GetProperties()
    {
        _polyNavAgent = GetComponent<PolyNavAgent>();
        _anim = GetComponent<Animator>();
    }

    private void AddToList()
    {
		if (GameplayManager.instance != null)
		{
			GameplayManager.instance.enemies.Add(this);
            StartCoroutine(BeginBehavior(3f));
        }  
    }

    private IEnumerator BeginBehavior(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetState(EnemyStates.Wandering);
    }

	// Update is called once per frame
	private void Update ()
    {
        Behave();

		if (Destination == null) return;
		_distance = Vector2.Distance(transform.position, Destination.position);
	}

    public void SetState(EnemyStates newState)
    {
        _enemyStates = newState;
        Destination = null;
    }

    private void Behave()
    {
		switch (_enemyStates)
		{
            case EnemyStates.Idle:
				{
					
					break;
				}
			case EnemyStates.Wandering:
				{
                    Enemy_Wandering();
					break;
				}
			case EnemyStates.Chasing:
				{
                    Enemy_Chasing();
					break;
				}
			case EnemyStates.Scared:
				{
                    Enemy_Scared();
					break;
				}
			case EnemyStates.Dead:
				{
                    Enemy_Dead();
					break;
				}
		}
    }

    private void Enemy_Wandering()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        if (Destination == null)
        {
            _newDestinationIndex = SetNewIndex();
            SetNewDestination(GameplayManager.instance.powerUpTransform[_newDestinationIndex]);
        }

        if (_distance < _distanceThreshold)
        {
            _newDestinationIndex = SetNewIndex();
			SetNewDestination(GameplayManager.instance.powerUpTransform[_newDestinationIndex]);
        }

        _BehaviourTime += Time.deltaTime;

        if (_BehaviourTime > _maxBehaviourTime)
        {
            if (_enemyStates != EnemyStates.Scared || _enemyStates != EnemyStates.Dead)
            {
                _BehaviourTime = 0f;
                SetState(EnemyStates.Chasing);
            }
        }
    }
    
    private void Enemy_Chasing()
    {
        //TODO:RUN TO PLAYER
        GetComponent<BoxCollider2D>().enabled = true;
        _anim.SetBool("IsScared", false);
        _polyNavAgent.maxSpeed = _normalSpeed;
        SetNewDestination(GameplayManager.instance.player.transform);
    }

    private void Enemy_Scared()
    {
        //TODO: RUN AWAY FROM PLAYER
       // _polyNavAgent.maxSpeed = _scaredSpeed;
        _anim.SetBool("IsScared", true);

        if (Destination == null)
        {
            if (GameplayManager.instance.player.transform.position.y < GameplayManager.instance.player.GetComponent<PlayerMovement>().startingVector.y)
            {
                if (_distance > _distanceThreshold)
                {
                    SetNewDestination(GameplayManager.instance.powerUpTransform[Random.Range(0, 2)]);
                }
            }
            else
            {
                if (_distance > _distanceThreshold)
                {
                    SetNewDestination(GameplayManager.instance.powerUpTransform[Random.Range(2, 4)]);
                }
            }
        }

        if (_distance < _distanceThreshold)
        {
            SetNewDestination(GameplayManager.instance.powerUpTransform[Random.Range(2, 4)]);
        }
    }

    private void Enemy_Dead()
    {
        _polyNavAgent.maxSpeed = _deadSpeed;
        isDead = true;
        GetComponent<BoxCollider2D>().enabled = false;
        SetNewDestination(GameplayManager.instance._enemyReturnPoint);

        if (_distance < _distanceThreshold)
        {
            SetState(EnemyStates.Chasing);
            isDead = false;
            _anim.SetBool("IsDead", false);
        }
    }

    private int SetNewIndex()
    {
        int _newIndex = Random.Range(0, 4);
        _oldDestinationIndex = _newIndex;

        return _newIndex;
    }

    private void SetNewDestination(Transform _newDestination)
    {
        Destination = _newDestination;
		_polyNavAgent.SetDestination(Destination.position);
    }

    public void ReturnToLife()
    {
        if (_enemyStates != EnemyStates.Dead)
        {
            SetState(EnemyStates.Chasing);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (_enemyStates == EnemyStates.Scared)
            {
                SetState(EnemyStates.Dead);
                _anim.SetBool("IsDead", true);
                AudioManager.instance.PlaySFX(_deathSFX);
                GameplayManager.instance.ShowSmoke(transform.position);
                GameplayManager.instance.UpdateScore(_score);
            }
            else if (_enemyStates != EnemyStates.Dead && _enemyStates != EnemyStates.Scared)
            {
                SetState(EnemyStates.Idle);
                col.gameObject.GetComponent<PlayerMovement>().isDead = true;
                GameplayManager.instance.StartGameOver();
                _polyNavAgent.maxSpeed = 0f;
            }
        }
    }
}
