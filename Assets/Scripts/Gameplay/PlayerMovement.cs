using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public float _raycastLength;
    private bool isButtonDown;
    public bool isDead;
    private Vector2 movement;
    private Rigidbody2D RB2D;

    private float horizontal;
    private float vertical;

    public LayerMask _collisionMask;
    public Vector2 startingVector { get; set; }

    public enum MovementState
    {
        Right,
        Left,
        Up,
        Down
    };
    public MovementState moveState;

    public List<RaycastBehavior> _RightRaycasts = new List<RaycastBehavior>();
    public List<RaycastBehavior> _LeftRaycasts = new List<RaycastBehavior>();
    public List<RaycastBehavior> _DownRaycasts = new List<RaycastBehavior>();
    public List<RaycastBehavior> _UpRaycasts = new List<RaycastBehavior>();

    public AudioSource _audioSource;
    public AudioClip _moving;
    public AudioClip _poweredMoving;

    private RaycastHit2D Righthit;
    private RaycastHit2D Lefthit;
    private RaycastHit2D Downhit;
    private RaycastHit2D Uphit;

    private delegate void RaycastFunctions();
    private RaycastFunctions _raycastFunctions;

    // Use this for initialization
    private void Start ()
    {
        RB2D = GetComponent<Rigidbody2D>();
        startingVector = transform.position;
        Invoke("AddToList", 0.1f);
        //movement = Vector3.right;
        _raycastFunctions += DirectionRaycast;
        _raycastFunctions += DebugRays;
    }

    private void AddToList()
    {
        GameplayManager.instance.player = gameObject;
    }

    private void FixedUpdate()
    {
        //_raycastFunctions();
    }

    private void DirectionRaycast()
    {
        Righthit = Physics2D.Raycast(transform.position, Vector2.right, _raycastLength, _collisionMask);
        Lefthit = Physics2D.Raycast(transform.position, Vector2.left, _raycastLength, _collisionMask);
        Downhit = Physics2D.Raycast(transform.position, -Vector2.up, _raycastLength, _collisionMask);
        Uphit = Physics2D.Raycast(transform.position, Vector2.up, _raycastLength, _collisionMask);
    }

    private void DebugRays()
    {
        Debug.DrawRay(transform.position, Vector2.right * _raycastLength, Color.white);
        Debug.DrawRay(transform.position, Vector2.left * _raycastLength, Color.white);
        Debug.DrawRay(transform.position, -Vector2.up * _raycastLength, Color.white);
        Debug.DrawRay(transform.position, Vector2.up * _raycastLength, Color.white);

        if (Righthit.collider != null)
        {
            Debug.Log("Right " + Righthit.collider.name);
        }

        if (Lefthit.collider != null)
        {
            Debug.Log("Left " + Lefthit.collider.name);
        }

        if (Downhit.collider != null)
        {
            Debug.Log("Down " + Downhit.collider.name);
        }

        if (Uphit.collider != null)
        {
            Debug.Log("Up " + Uphit.collider.name);
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            MovementFunction();
        }
        else
        {
            GameplayManager.instance.ShowSmoke(transform.position);
            GameplayManager.instance.UpdateLives(-1);
            gameObject.SetActive(false);
            AudioManager.instance.MuteAllMusic();
            _audioSource.mute = true;
        }     
    }

    private void MovementFunction()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetButton("Horizontal"))
        {
            //isButtonDown = true;

            if (horizontal < 0)
            {
                if (!_LeftRaycasts[0]._isHit && !_LeftRaycasts[1]._isHit && !_LeftRaycasts[2]._isHit /*&& !_LeftRaycasts[3]._isHit && !_LeftRaycasts[4]._isHit*/)
                {
                    moveState = MovementState.Left;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else if (horizontal > 0)
            {
                if (!_RightRaycasts[0]._isHit && !_RightRaycasts[1]._isHit && !_RightRaycasts[2]._isHit /*&& !_RightRaycasts[3]._isHit && !_RightRaycasts[4]._isHit*/)
                {
                    moveState = MovementState.Right;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
        else if (Input.GetButton("Vertical"))
        {
            //isButtonDown = true;

            if (vertical < 0)
            {
                if (!_DownRaycasts[0]._isHit && !_DownRaycasts[1]._isHit && !_DownRaycasts[2]._isHit)
                {
                    moveState = MovementState.Down;
                }
            }
            else if (vertical > 0)
            {
                if (!_UpRaycasts[0]._isHit && !_UpRaycasts[1]._isHit && !_UpRaycasts[2]._isHit)
                {
                    moveState = MovementState.Up;
                }
            }
        }

        if (!Input.anyKey)
        {
            isButtonDown = false;
        }

        StateMachine();
    }

    private void StateMachine()
    {
        switch (moveState)
        {
            case MovementState.Right:
                {
                    movement = new Vector2(1, 0);
                    break;
                }
            case MovementState.Left:
                {
                    movement = new Vector2(-1, 0);
                    break;
                }
            case MovementState.Down:
                {
                    movement = new Vector2(0, -1);
                    break;
                }
            case MovementState.Up:
                {
                    movement = new Vector2(0, 1);
                    break;
                }
        }

        RB2D.velocity = movement * speed;
    }

    public void PlayNormalSFX()
    {
        _audioSource.clip = _moving;
        _audioSource.Play();
    }

    public void PlayPowerSFX()
    {
        _audioSource.clip = _poweredMoving;
        _audioSource.Play();
    }
}
