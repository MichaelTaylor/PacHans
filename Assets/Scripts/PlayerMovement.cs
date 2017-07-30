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

        isButtonDown = IsButtonHeldDown();
        //transform.position += new Vector3(horizontal, vertical) * speed * Time.deltaTime;
        movement = new Vector2(horizontal, vertical);
        RB2D.velocity = movement * speed;

        //velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        //previous = transform.position

        /*if (isButtonDown)
        {
			if (RB2D.velocity.x > 0.5)
			{
                moveState = MovementState.Right;
			}
			else if (RB2D.velocity.x < -0.5)
			{
                moveState = MovementState.Left;
			}
			else if (RB2D.velocity.y > 0.5)
			{
                moveState = MovementState.Up;
			}
			else if (RB2D.velocity.y < -0.5)
			{
                moveState = MovementState.Down;
			} 
        }*/

        MoveDirection();
    }

    private bool IsButtonHeldDown()
    {
        if (horizontal != 0 || vertical != 0)
        {
            return true;   
        }
        else
        {
            return false;
        }

    }

    private void MoveDirection()
    {
        
        //if (NewState == moveState) return;

        switch(moveState)
        {
            case MovementState.Right:
            {
				movement = Vector2.right;
				break;
            }
            case MovementState.Left:
            {
                movement = Vector2.left;
                break;
            }
            case MovementState.Up:
			{
                movement = Vector2.up;
				break;
			}
            case MovementState.Down:
			{
                movement = Vector2.down;
				break;
			}
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
    }
}
