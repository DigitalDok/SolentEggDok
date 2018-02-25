using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Gameplay")]
    public float MoveSpeed;
    public float JumpStrength;
    public float JumpCD;
    public float JumpHoldMultiplier;
    private float CurHoldJump;
    public float HoldJumpCD;

    public GameObject IceBallProjectile;
    public float ProjectileSpeed;
    private GameObject IceBall;

    private Transform PlayerSpawn;

    public int Floor = 1;

    [Header("Debugging")]
    public float GroundedRaycastLength = -0.8f;

    private Rigidbody2D MyRigid;
    private Animator MyAnim;

    internal bool IsFacingRight = true;
    private RaycastHit2D IsGrounded;

    private DoorScript CurrentDoor;
    private NavigatorScript CurrentNavigator;

    private Transform ShootingOrigin;
    private bool HasJumped = true;
    private float CurJumpCD;
    private bool StopAllMovement;
    private bool WillNowJump;
    private bool WillNowAttack;
    private bool CanHoldForMore;
    

    void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        ShootingOrigin = transform.Find("ShootingOrigin");
        PlayerSpawn = GameObject.Find("PlayerSpawn").transform;
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
                Floor = CurrentNavigator.ThisLeadsTo.gameObject.GetComponent<NavigatorScript>().Floor;
                

                transform.position = CurrentNavigator.ThisLeadsTo.transform.position;

            }
        }

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded && MyRigid.velocity.y >= 0)
            {
                MyRigid.gravityScale = 1;
                MyRigid.velocity = new Vector2(0, 0);
                MyRigid.AddForce(new Vector2(0, JumpStrength));
                CanHoldForMore = true;
                CurHoldJump = 0;

            }
        }
        if(Input.GetButton("Jump") && CanHoldForMore)
        {
            CurHoldJump += Time.deltaTime;
            if(CurHoldJump > HoldJumpCD)
            {
                CanHoldForMore = false;
                CurHoldJump = 0;
                return;
            }

            if(MyRigid.velocity.y > 0 )
            {
                MyRigid.AddRelativeForce(new Vector2(0, JumpStrength * JumpHoldMultiplier * Time.deltaTime));
                MyRigid.gravityScale += Time.deltaTime;
            }
        }

        // Shooting Projectiles
        if (Input.GetButtonDown("Fire1"))
        {
            if (IsGrounded && MyRigid.velocity.y >= 0)
            {
                if (IceBall)
                {
                    if (IceBall.activeInHierarchy)
                    {
                        StopAllMovement = false;
                        return;
                    }
                }

                IceBall = Instantiate(IceBallProjectile, ShootingOrigin.position, Quaternion.identity);
                IceBall.GetComponent<Rigidbody2D>().velocity = new Vector2(ProjectileSpeed * (IsFacingRight ? 1 : -1), 0);
                IceBall.GetComponent<IceBallScript>().Floor = Floor;
               
            }
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
            if(!StopAllMovement)
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
        if (collision.GetComponent<Pickup>())
        {
            print("Picked up a " + collision.name);
            GameManager.TheGameManager.Score += collision.GetComponent<Pickup>().ScorePts;
            collision.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<DoorScript>())
        {
            CurrentDoor = collision.GetComponent<DoorScript>();

            if (CurrentDoor.IsOpen) CurrentDoor.CloseDoor();
        }
        else if (collision.CompareTag("Monster"))
        {
            if (Floor == collision.GetComponent<EnemyInfo>().Floor)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(RespawnPlayer());
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2);
        GetComponent<BoxCollider2D>().enabled = true;
        MyRigid.velocity = Vector2.zero;
        transform.position = PlayerSpawn.position;
        Floor = 1;
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
