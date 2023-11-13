using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float runSpeed = 5f;

    private Rigidbody2D body;
    private Animator animator;
    private Vector2 motionVector;
    public Vector2 lastMotionVector;
    public bool moving;
    bool running;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) )
        {
            running = false;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector.x = horizontal;
        motionVector.y = vertical;

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        moving = horizontal !=0 || vertical !=0;
        animator.SetBool("moving", moving);

        if( horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(horizontal, vertical).normalized;

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
        body.velocity = motionVector * (running == true ? runSpeed : speed);
    }

    private void OnDisable()
    {
        body.velocity = Vector2.zero;
    }
}
