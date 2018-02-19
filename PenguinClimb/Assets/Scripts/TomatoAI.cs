using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoAI : MonoBehaviour {

    public bool IsFacingRight;
    public float WalkSpeed;
    private Rigidbody2D MyRigid;


	void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyRigid.velocity = new Vector2(WalkSpeed * ((IsFacingRight)?1:-1), 0);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DirChanger"))
        {
            Flip();
        }
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
        MyRigid.velocity = new Vector2(WalkSpeed * ((IsFacingRight) ? 1 : -1), 0);
    }
}
