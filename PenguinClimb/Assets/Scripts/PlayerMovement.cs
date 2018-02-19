using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Gameplay")]
    public float MoveSpeed;
    public float JumpStrength;
    public float TrajectoryStrength;
    public float JumpCD;

    public GameObject IceBallProjectile;
    public float ProjectileSpeed;

    [Header("Debugging")]
    public float GroundedRaycastLength = -0.8f;

    private Rigidbody2D MyRigid;
    private Animator MyAnim;

    private bool IsFacingRight = true;
    private RaycastHit2D IsGrounded;

    private DoorScript CurrentDoor;
    private NavigatorScript CurrentNavigator;

    private Transform ShootingOrigin;
    private bool HasJumped = true;
    private float CurJumpCD;
    

    void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        ShootingOrigin = transform.Find("ShootingOrigin");
	}

    private void Update()
    {
        IsGrounded = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, GroundedRaycastLength, 0), 1 << LayerMask.NameToLayer("Land"));

        Debug.DrawLine(transform.position, transform.position + new Vector3(0, GroundedRaycastLength, 0), Color.blue);

        // Doors
        if(Input.GetButtonDown("Vertical"))
        {
            if (CurrentDoor)
            {
                if (CurrentDoor.IsOpen) CurrentDoor.CloseDoor();
                else CurrentDoor.OpenDoor();
            }
            else if (CurrentNavigator)
            {
                transform.position = CurrentNavigator.ThisLeadsTo.transform.position;
            }
        }

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded && MyRigid.velocity.y >= 0)
            {
                MyRigid.velocity = new Vector2(0, 0);
                MyRigid.AddForce(new Vector2((IsFacingRight?TrajectoryStrength:-TrajectoryStrength), JumpStrength));
                HasJumped = true;
            }
        }


        // Shooting Projectiles
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject IceBall = Instantiate(IceBallProjectile, ShootingOrigin.position, Quaternion.identity);
            IceBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ProjectileSpeed * (IsFacingRight?1:-1), 0);
        }


        // Flipping
        if (ShouldFlip())
            Flip();
    }

    void FixedUpdate ()
    {
        if (HasJumped)
        {
            CurJumpCD += Time.deltaTime;
            if(CurJumpCD>JumpCD)
            {
                HasJumped = false;
                CurJumpCD = 0;
            }
        }
        else
        {
            MyRigid.velocity = new Vector2(Input.GetAxis("Horizontal") * MoveSpeed, MyRigid.velocity.y);
        }
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    private bool ShouldFlip()
    {
        return MyRigid.velocity.x > 0 && !IsFacingRight || MyRigid.velocity.x < 0 && IsFacingRight;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Pickup>())
        {
            print("Picked up a " + collision.name);
            GameManager.TheGameManager.Score += collision.GetComponent<Pickup>().ScorePts;
            collision.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<DoorScript>())
        {
            CurrentDoor = collision.GetComponent<DoorScript>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<NavigatorScript>())
        {
            CurrentNavigator = collision.GetComponent<NavigatorScript>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<DoorScript>())
        {
            CurrentDoor = null;
        }
        else if (collision.GetComponent<NavigatorScript>())
        {
            CurrentNavigator = null;
        }
    }
}
