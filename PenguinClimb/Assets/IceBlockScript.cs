using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockScript : MonoBehaviour {

    private Rigidbody2D MyRigid;

    private int Bounces;
    public float SlideSpeed;


    void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Bounces++;
            print("wall");
            if (Bounces == 2)
            {
                gameObject.SetActive(false);
            }

            MyRigid.velocity = new Vector2(-MyRigid.velocity.x, MyRigid.velocity.y);
        }
        else if (collision.gameObject.CompareTag("Monster"))
        {
            if (Mathf.Abs(MyRigid.velocity.x) > 0)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                GameManager.TheGameManager.EnemiesOnLevel--;
            }
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            if (Mathf.Abs(MyRigid.velocity.x) > 0)
            {
                GameManager.TheGameManager.ThePlayer.Die();
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            
            MyRigid.velocity = new Vector2(SlideSpeed * (collision.gameObject.GetComponent<PlayerMovement>().IsFacingRight ? 1 : -1), 0);
            
        }
        else if(collision.gameObject.CompareTag("IceBlock"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
