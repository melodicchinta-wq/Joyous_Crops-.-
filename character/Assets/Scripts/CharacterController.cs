using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    [SerializeField] float speed = 2f;
    Vector2 motionVector;
    Vector2 lastMotionVector;
    Animator animator;
    public bool moving;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x,
                (int)transform.position.y, 0);

            if (GameManager.instance.tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is interactable");
                GameManager.instance.tileManager.SetInteracted(position);
            }
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        motionVector = new Vector2
            (
            horizontal,
            vertical
            );
        animator.SetFloat("horizontal",horizontal);
        animator.SetFloat("vertical",vertical);

        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2
                (
                horizontal,
                vertical
                ).normalized;

            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody2D.velocity = motionVector * speed;
    }
}
