﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyBehavior : MonoBehaviour {
    
    internal EnemyInfo MyInfo;

    public float JumpingCooldown;
    private float CurJumpCooldown;

    public float JumpStrength;

    private Rigidbody2D MyRigid;

    public bool JumpWhenCharJumps;

    public Sprite NormalSprite;
    public Sprite JumpingSprite;
    private SpriteRenderer MyRenderer;

    public enum EnemyType
    {
        Slime,
        SushiBag
    }
    public EnemyType MyType;

    private void Start()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyInfo = GetComponent<EnemyInfo>();
        MyRenderer = GetComponent<SpriteRenderer>();
    }

    void Update ()
    {
        var IsGrounded = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, -0.15f, 0), 1 << LayerMask.NameToLayer("Land"));
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, -0.15f, 0), Color.blue);

        if (CurJumpCooldown>0.6f && IsGrounded) MyRenderer.sprite = NormalSprite;

        CurJumpCooldown += Time.deltaTime;

        switch (MyType)
        {
            case EnemyType.Slime:
                if (MyInfo.Floor == MyInfo.ThePlayer.Floor)
                {
                    if (LookingAtPlayer())
                    {
                        if (CurJumpCooldown > JumpingCooldown)
                        {
                            if (JumpWhenCharJumps)
                            {
                                if (MyInfo.ThePlayer.GetComponent<Rigidbody2D>().velocity.y > 0)
                                {
                                    CurJumpCooldown = 0;
                                    Jump();
                                }
                            }
                            else
                            {
                                CurJumpCooldown = 0;
                                Jump();
                            }
                        }
                    }
                }
                break;
            case EnemyType.SushiBag:
                if (CurJumpCooldown > JumpingCooldown)
                {
                    CurJumpCooldown = 0;
                    Jump();
                }
                break;
        }

        
    }

    private void Jump()
    {
        MyRigid.AddForce(new Vector2(0, JumpStrength));
        MyRenderer.sprite = JumpingSprite;

        if(!JumpWhenCharJumps && MyType!= EnemyType.SushiBag)
        JumpingCooldown = UnityEngine.Random.Range(1, 5);

    }

    private bool LookingAtPlayer()
    {
        return (MyInfo.ThePlayer.transform.position.x > transform.position.x && GetComponent<WalkingEnemyComponent>().IsFacingRight)
            || (MyInfo.ThePlayer.transform.position.x < transform.position.x && !GetComponent<WalkingEnemyComponent>().IsFacingRight);
    }
}
