using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager TheGameManager;

    public int Score;
    private Text ScoreText;

    public Sprite OpenDoorSprite;
    public Sprite ClosedDoorSprite;

    private void Awake()
    {
        TheGameManager = this;
    }

    void Start ()
    {
        ScoreText = transform.Find("Score").gameObject.GetComponent<Text>();
	}
	
	
	void Update ()
    {
        ScoreText.text = "SCORE: " + Score.ToString("0000");
	}
}
