using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyComponent : MonoBehaviour {

    public bool IsFacingRight;
    public float WalkSpeed;
    private Rigidbody2D MyRigid;
    private float CurFlippingCD;

    void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyRigid.velocity = new Vector2(WalkSpeed * ((IsFacingRight)?1:-1), 0);
	}

    private void Update()
    {
        CurFlippingCD += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DirChanger"))
        {
            if(CurFlippingCD>1)
            Flip();
        }
    }

    private void Flip()
    {
        CurFlippingCD = 0;
        IsFacingRight = !IsFacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
        MyRigid.velocity = new Vector2(WalkSpeed * ((IsFacingRight) ? 1 : -1), 0);
    }
}
