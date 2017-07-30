﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float Speed;
    Vector2 Dest = Vector2.zero;

	// Use this for initialization
	private void Start ()
    {
        Dest = transform.position;
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 p = Vector2.MoveTowards(transform.position, Dest, Speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        // Check for Input if not moving
        if ((Vector2)transform.position == Dest)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                Dest = (Vector2)transform.position + Vector2.up;
            if (Input.GetKey(KeyCode.RightArrow))
                Dest = (Vector2)transform.position + Vector2.right;
            if (Input.GetKey(KeyCode.DownArrow))
                Dest = (Vector2)transform.position - Vector2.up;
            if (Input.GetKey(KeyCode.LeftArrow))
                Dest = (Vector2)transform.position - Vector2.right;
        }
    }

    private bool IsValid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }
}
