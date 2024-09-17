using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum PlayerDirection
{
    Up, Down, Left, Right
}
public class TopDownPlayerController : MonoBehaviour
{
    #region Serializable Values

    public float _playerSpeed = 2f;
    #endregion
    #region Private Variables

    private Vector2 _movementDirection;
    private Vector2 _movementVector;
    private Vector2 _inputDirection;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Animation _animation;
    private PlayerDirection _playerDirection;
    #endregion
    #region Animation Names

    private static readonly int up = Animator.StringToHash("player_up");
    private static readonly int left = Animator.StringToHash("player_left");
    private static readonly int right = Animator.StringToHash("player_right");
    private static readonly int down = Animator.StringToHash("player_down");
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void ComputePlayerDirection(float x, float y)
    {
        if (x < -0.9)
        {
            _playerDirection = PlayerDirection.Left;

            //_animation.clip = _animation.GetClip("player_left");
            //Animator.(left);
            //_animation.Play()
            if(_animator!= null)
                _animator?.Play("player_left");
            _movementDirection = new Vector2(1f, 0f);
        }
        else if (x > .9)
        {
            _playerDirection = PlayerDirection.Right;
            if(_animator!= null)
                _animator?.Play("player_right");
            _movementDirection = new Vector2(1f, 0f);
        }
            
        else if (y < -0.9)
        {
            _playerDirection = PlayerDirection.Down;
            if(_animator!= null)
                _animator?.Play("player_down");
            _movementDirection = new Vector2(0, 1f);
        }
        else if (y > 0.9)
        {
            _playerDirection = PlayerDirection.Up;
            if(_animator!= null)
                _animator?.Play("player_up");
            _movementDirection = new Vector2(0, 1f);
        }
    }

    Vector2 ComputeMovementDirection()
    {
        switch (_playerDirection)
        {
            case PlayerDirection.Up:
                return new Vector2(0, 1);
            case PlayerDirection.Down:
                return new Vector2(0, -1);
            case PlayerDirection.Left:
                return new Vector2(-1, 0);
            case PlayerDirection.Right:
                return new Vector2(1, 0);
            default:
                return new Vector2(0, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if the movement direction has changed from what it was in either x or y, then we need to compute a new player direction
        Vector2 newInputDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        if( Mathf.Abs(newInputDirection.x - _inputDirection.x) > 0.001f && Mathf.Abs(newInputDirection.x) > Mathf.Abs(_inputDirection.x))
            ComputePlayerDirection(newInputDirection.x, 0f);
        if (Mathf.Abs(newInputDirection.y - _inputDirection.y)> 0.001f && Mathf.Abs(newInputDirection.y) > Mathf.Abs(_inputDirection.y))
            ComputePlayerDirection(0, newInputDirection.y);
        _movementVector = new Vector2(_movementDirection.x * newInputDirection.x, _movementDirection.y * newInputDirection.y);
        
        
        _inputDirection = newInputDirection;
    }
    
    //always move on a fixed update when using a rigid body
    private void FixedUpdate()
    {
        _rb.velocity = _movementVector * _playerSpeed;
    }
}
