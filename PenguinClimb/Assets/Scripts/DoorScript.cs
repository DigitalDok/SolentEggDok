using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Sprite OpenDoorSprite;
    private Sprite ClosedDoorSprite;

    private SpriteRenderer MyRenderer;

    public bool IsOpen;
	
	void Start ()
    {
        MyRenderer = GetComponent<SpriteRenderer>();
        OpenDoorSprite = GameManager.TheGameManager.OpenDoorSprite;
        ClosedDoorSprite = GameManager.TheGameManager.ClosedDoorSprite;
	}
	

	public void OpenDoor()
    {
        MyRenderer.sprite = OpenDoorSprite;
        IsOpen = true;
    }

    public void CloseDoor()
    {
        MyRenderer.sprite = ClosedDoorSprite;
        IsOpen = false;
    }
}
