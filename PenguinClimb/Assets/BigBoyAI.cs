using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoyAI : MonoBehaviour {

    internal EnemyInfo MyInfo;
    
    private Rigidbody2D MyRigid;
    
    public Sprite NormalSprite;
    public Sprite JumpingSprite;
    private SpriteRenderer MyRenderer;

    public float JumpStrength;

    public enum States
    {
        Walking,
        Stomping,
        DroppingDown
    }

    public States MyStates = States.Walking;


    public float StompingCD;
    public float CurStompCD;
    private int Stomps;

    void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyInfo = GetComponent<EnemyInfo>();
        MyRenderer = GetComponent<SpriteRenderer>();
    }
	
	
	void Update ()
    {
        switch (MyStates)
        {
            case States.Walking:

                if (!GetComponent<BoxCollider2D>().enabled)
                    GetComponent<BoxCollider2D>().enabled = true;

                break;
            case States.Stomping:
                CurStompCD += Time.deltaTime;
                if (CurStompCD > StompingCD)
                {
                    CurStompCD = 0;
                    JumpAndStomp();
                    Stomps++;
                    if (Stomps == 3)
                    {
                        CurStompCD = 0;
                        MyStates = States.DroppingDown;
                        GetComponent<CircleCollider2D>().enabled = false;
                    }
                }
                break;
            case States.DroppingDown:
                CurStompCD += Time.deltaTime;
                if (CurStompCD > 0.76f)
                {
                    MyStates = States.Walking;
                    Stomps = 0;
                    GetComponent<CircleCollider2D>().enabled = true;
                    MyRenderer.sprite = NormalSprite;
                    MyInfo.Floor--;
                    MyRigid.velocity = 
                        new Vector2(GetComponent<WalkingEnemyComponent>().WalkSpeed * (GetComponent<WalkingEnemyComponent>().IsFacingRight ? 1 : -1), 0);
                }

                break;
        }
    }

    private void JumpAndStomp()
    {
        MyRigid.AddForce(new Vector2(0, JumpStrength));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && MyStates == States.Walking)
        {
            if(MyInfo.Floor -1 == MyInfo.ThePlayer.Floor)
            {
                MyRigid.velocity = Vector2.zero;
                MyRenderer.sprite = JumpingSprite;
                MyStates = States.Stomping;
            }
        }
    }
}
