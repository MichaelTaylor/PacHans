using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyNavAgent))]
public class EnemyBehavior : MonoBehaviour {

    private bool isDead;
    public Transform Destination;
    public enum EnemyStates
    {
        Wandering,
        Chasing,
        Scared,
        Dead
    };
    public EnemyStates _enemyStates;
	
    // Use this for initialization
	private void Start ()
    {
        GetComponent<PolyNavAgent>().SetDestination(Destination.position);
	}
	
	// Update is called once per frame
	private void Update ()
    {
		
	}

    private void Behave()
    {
        switch(_enemyStates)
        {
            case EnemyStates.Wandering:
                {
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
}
