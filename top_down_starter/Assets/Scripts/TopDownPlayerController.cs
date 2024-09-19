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
    private PlayerDirection _playerDirection;
    #endregion

    #region Animation Names
    private static readonly int up = Animator.StringToHash("player_up");
    private static readonly int left = Animator.StringToHash("player_left");
    private static readonly int right = Animator.StringToHash("player_right");
    private static readonly int down = Animator.StringToHash("player_down");
    private static readonly int isMoving = Animator.StringToHash("isMoving");
    #endregion

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
            _animator?.Play("player_left");
            _movementDirection = new Vector2(1f, 0f);
        }
        else if (x > 0.9)
        {
            _playerDirection = PlayerDirection.Right;
            _animator?.Play("player_right");
            _movementDirection = new Vector2(1f, 0f);
        }
        else if (y < -0.9)
        {
            _playerDirection = PlayerDirection.Down;
            _animator?.Play("player_down");
            _movementDirection = new Vector2(0, 1f);
        }
        else if (y > 0.9)
        {
            _playerDirection = PlayerDirection.Up;
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

    void Update()
    {
        Vector2 newInputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Mathf.Abs(newInputDirection.x - _inputDirection.x) > 0.001f && Mathf.Abs(newInputDirection.x) > Mathf.Abs(_inputDirection.x))
            ComputePlayerDirection(newInputDirection.x, 0f);
        if (Mathf.Abs(newInputDirection.y - _inputDirection.y) > 0.001f && Mathf.Abs(newInputDirection.y) > Mathf.Abs(_inputDirection.y))
            ComputePlayerDirection(0, newInputDirection.y);

        _movementVector = new Vector2(_movementDirection.x * newInputDirection.x, _movementDirection.y * newInputDirection.y);
        _inputDirection = newInputDirection;

        // Check if the player is moving
        bool isMoving = _inputDirection != Vector2.zero;
        _animator.SetBool("isMoving", isMoving);
    }

    private void FixedUpdate()
    {
        _rb.velocity = _movementVector * _playerSpeed;
    }
}
