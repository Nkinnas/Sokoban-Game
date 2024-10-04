using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 500f;  
    private Vector3 endPosition;
    private bool moveToPosition = false;
    private float playerMoveDistance = 1f;  
    public LayerMask wallLayer; 
    public LayerMask boxLayer;   
    private Animator _animator;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _speed = "Speed";

    void Start()
    {
        endPosition = transform.position;  
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!moveToPosition)
        {
            HandleMovement(); 
        }
    }

    void FixedUpdate()
    {
        if (moveToPosition)
        {
            MoveTowardsTarget();
        }
    }

    void HandleMovement()
    {
        Vector3 direction = GetInputDirection();

        if (direction != Vector3.zero)
        {
            if (!CheckCollision(transform.position, direction, playerMoveDistance, wallLayer) &&
                !CheckForMultipleBoxes(direction))
            {
                MovePlayer(direction);
            }
        }
        else
        {
            _animator.SetFloat(_speed, 0f);
        }
    }

    Vector3 GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.A)) return Vector3.left;
        if (Input.GetKeyDown(KeyCode.D)) return Vector3.right;
        if (Input.GetKeyDown(KeyCode.W)) return Vector3.up;
        if (Input.GetKeyDown(KeyCode.S)) return Vector3.down;
        return Vector3.zero;
    }

    void MovePlayer(Vector3 direction)
    {
        endPosition = transform.position + direction * playerMoveDistance;
        moveToPosition = true;
        float speed = direction.magnitude * moveSpeed;

        if (_animator != null)
        {
            _animator.SetFloat(_horizontal, direction.x);
            _animator.SetFloat(_vertical, direction.y);
            _animator.SetFloat(_speed, speed);
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, endPosition) < 0.0001f)
        {
            transform.position = endPosition;
            moveToPosition = false;
        }
    }

    bool CheckCollision(Vector3 origin, Vector3 direction, float distance, LayerMask layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);
        return hit.collider != null;
    }

    bool CheckForMultipleBoxes(Vector3 direction)
    {
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, direction, playerMoveDistance, boxLayer);
        if (hitBox.collider != null)
        {
            Vector3 secondBoxCheckPosition = hitBox.collider.transform.position + direction * playerMoveDistance;
            RaycastHit2D secondBoxHit = Physics2D.Raycast(secondBoxCheckPosition, Vector2.zero, 0f, boxLayer);
            if (secondBoxHit.collider != null)
            {
                return true;
            }
        }

        return false; 
    }
}
