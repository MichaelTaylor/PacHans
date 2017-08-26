using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehavior : MonoBehaviour {

    public float _transitionSpeed;
    public float _speedMultiplyer { get; set; }
    public bool CanMove { get; set; }


    private void Start()
    {
        GameplayManager.instance._transitionBehavior = this;
    }

    // Update is called once per frame
    private void Update ()
    {
        if (!CanMove) return;
        transform.position += new Vector3(0f, -1f, 0f) * (_transitionSpeed) * Time.deltaTime;
	}
}
