using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBehavior : MonoBehaviour {

    public RaycastHit2D _rayCast { get; set; }
    public Vector2 _rayDirection;
    public float _rayLength;
    public LayerMask _rayMask;
    public bool _isHit;

    private void FixedUpdate()
    {
        _rayCast = Physics2D.Raycast(transform.position, _rayDirection, _rayLength, _rayMask);
        Debug.DrawRay(transform.position, _rayDirection * _rayLength, Color.white);
        _isHit = _rayCast.collider != null ? true : false;
    }
}
