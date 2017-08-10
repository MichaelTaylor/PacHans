using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    private float previousVel;
    private bool isButtonDown;
    private Vector2 movement;
    private Rigidbody2D RB2D;

    private float horizontal;
    private float vertical;

    public enum MovementState
    {
        Right,
        Left,
        Up,
        Down
    };
    public MovementState moveState;

    // Use this for initialization
    private void Start ()
    {
        RB2D = GetComponent<Rigidbody2D>();
        //movement = Vector3.right;
    }

    private void Update()
    {
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

        movement = new Vector2(horizontal, vertical);
        RB2D.velocity = movement * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
