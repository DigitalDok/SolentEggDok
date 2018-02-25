using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBoyAI : MonoBehaviour {

    public float HoverSpeed;
    private Rigidbody2D MyRigid;

    public bool IsFacingRight;

    public enum Direction
    {
        UpRight,
        DownRight,
        UpLeft,
        DownLeft
    }
    public Direction StartingDir;

	// Use this for initialization
	void Start ()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        switch (StartingDir)
        {
            case Direction.UpRight:
                MyRigid.velocity = new Vector2(1, 1) * HoverSpeed;
                break;
            case Direction.DownRight:
                MyRigid.velocity = new Vector2(1, -1) * HoverSpeed;
                break;
            case Direction.UpLeft:
                MyRigid.velocity = new Vector2(-1, 1) * HoverSpeed;
                break;
            case Direction.DownLeft:
                MyRigid.velocity = new Vector2(-1, -1) * HoverSpeed;
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "VerticalBound")
        {
            MyRigid.velocity = new Vector2(MyRigid.velocity.x, -MyRigid.velocity.y);
        }
        if (collision.name == "HorizontalBound")
        {
            MyRigid.velocity = new Vector2(-MyRigid.velocity.x, MyRigid.velocity.y);

            IsFacingRight = !IsFacingRight;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }
    }

}
