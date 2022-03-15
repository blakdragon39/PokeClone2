using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;
    private readonly int moveXId = Animator.StringToHash("moveX");
    private readonly int moveYId = Animator.StringToHash("moveY");
    private readonly int isMovingId = Animator.StringToHash("isMoving");

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(new Vector3(targetPos.x, targetPos.y)))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        Animate();
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapBox(targetPos, new Vector2(.5f, .5f), 0, solidObjectsLayer) == null;
    }

    private void Animate()
    {
        if (input != Vector2.zero)
        {
            animator.SetFloat(moveXId, input.x);
            animator.SetFloat(moveYId, input.y);
        }

        animator.SetBool(isMovingId, input != Vector2.zero);
    }
}
