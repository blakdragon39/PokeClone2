using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public event Action OnEncounter;
    
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask encounterLayer;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;
    private readonly int moveXId = Animator.StringToHash("moveX");
    private readonly int moveYId = Animator.StringToHash("moveY");
    private readonly int isMovingId = Animator.StringToHash("isMoving");

    public void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    public void HandleUpdate() {
        if (!isMoving) {
            SetInput();

            if (input != Vector2.zero) {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (!OnTileInLayer(targetPos, solidObjectsLayer)) {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        Animate();
    }

    private void SetInput() {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0) input.y = 0;
    }

    private IEnumerator Move(Vector3 targetPos) {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        CheckForEncounters();
    }

    private void Animate() {
        if (input != Vector2.zero) {
            animator.SetFloat(moveXId, input.x);
            animator.SetFloat(moveYId, input.y);
        }

        animator.SetBool(isMovingId, input != Vector2.zero);
    }

    private void CheckForEncounters() {
        if (OnTileInLayer(transform.position, encounterLayer)) {
            OnEncounter();
        }
    }

    private bool OnTileInLayer(Vector2 position, LayerMask layer) {
        return Physics2D.OverlapBox(position, new Vector2(0.5f, 0.5f), 0, layer) != null;
    }
}