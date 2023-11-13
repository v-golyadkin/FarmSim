using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMove : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    Rigidbody2D body;
    public Transform moveTo;

    Animator animator;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        moveTo = GameManager.instance.player.transform;
    }

    private void FixedUpdate()
    {
        if (moveTo == null) { return; }

        if(Vector3.Distance(transform.position, moveTo.position) < 0.8f)
        {
            StopMoving();
            return;
        }

        Vector3 direction = (moveTo.position - transform.position).normalized;
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);

        direction *= speed;
        body.velocity = direction;
    }

    private void StopMoving()
    {
        moveTo = null;
        body.velocity = Vector3.zero;
    }
}
