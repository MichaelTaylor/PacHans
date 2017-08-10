using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyNavAgent))]
public class EnemyBehavior : MonoBehaviour {

    private int _newDestinationIndex;
    private int _oldDestinationIndex;
    private bool isDead;
    private Transform Destination;
    private float _distance;
    public float _distanceThreshold;
    public float _BehaviourTime;
    public float _maxBehaviourTime;
    public enum EnemyStates
    {
        Idle,
        Wandering,
        Chasing,
        Scared,
        Dead
    };
    public EnemyStates _enemyStates;

    private PolyNavAgent _polyNavAgent;

    // Use this for initialization
	private void Start()
    {     
        Invoke("AddToList", 0.1f);
        _polyNavAgent = GetComponent<PolyNavAgent>();

	}

    private void AddToList()
    {
		if (GameplayManager.instance != null)
		{
			GameplayManager.instance.enemies.Add(this);
            SetState(EnemyStates.Wandering);
		}  
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
					break;
				}
			case EnemyStates.Scared:
				{
					break;
				}
			case EnemyStates.Dead:
				{
					break;
				}
		}
    }

    private void Enemy_Wandering()
    {
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
    }

    private void Enemy_Chasing()
    {
        //TODO:RUN TO PLAYER
    }

    private void Enemy_Scared()
    {
        //TODO: RUN AWAY FROM PLAYER
    }

    private void Enemy_Dead()
    {
        //TODO: GO BACK TO STARTING SQUARE
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
}
