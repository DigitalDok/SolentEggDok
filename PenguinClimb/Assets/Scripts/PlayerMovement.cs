using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Gameplay")]
    public float MoveSpeed;
    public float JumpStrength;

    [Header("Debugging")]
    public float GroundedRaycastLength = -0.8f;

    private Rigidbody2D MyRigid;
    private Animator MyAnim;

    private bool bIsFacingRight = true;
    private RaycastHit2D IsGrounded;

    void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
	}

    private void Update()
    {
        IsGrounded = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, GroundedRaycastLength, 0), 1 << LayerMask.NameToLayer("Land"));
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, GroundedRaycastLength, 0), Color.blue);

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded && MyRigid.velocity.y >= 0)
            {
                MyRigid.AddForce(new Vector2(0, JumpStrength));
            }
        }

        if (ShouldFlip())
            Flip();
    }

    void FixedUpdate ()
    {
        MyRigid.velocity = new Vector2(Input.GetAxis("Horizontal") * MoveSpeed, MyRigid.velocity.y);
        

        
    }

    private void Flip()
    {
        bIsFacingRight = !bIsFacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    private bool ShouldFlip()
    {
        return MyRigid.velocity.x > 0 && !bIsFacingRight || MyRigid.velocity.x < 0 && bIsFacingRight;
    }
}
