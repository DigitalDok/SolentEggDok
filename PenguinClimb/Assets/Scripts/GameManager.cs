using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager TheGameManager;

    public int Score;
    private Text ScoreText;

    [Header("Door Sprites")]
    public Sprite OpenDoorSprite;
    public Sprite ClosedDoorSprite;

    [Header("Enemies")]
    public GameObject TomatoEnemy;

    [Header("Doors")]
    public List<GameObject> Doors = new List<GameObject>();
    public List<GameObject> Doors_Floor_4 = new List<GameObject>();
    public List<GameObject> Doors_Floor_3 = new List<GameObject>();
    public List<GameObject> Doors_Floor_2 = new List<GameObject>();
    public List<GameObject> Doors_Floor_1 = new List<GameObject>();

    [Header("Spawning")]
    public int EnemiesToSpawnInit;

    private void Awake()
    {
        TheGameManager = this;
    }

    void Start ()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Door"))
        {
            Doors.Add(item);
            if (item.transform.position.y > 2 && item.transform.position.y < 2.5f)
            {
                Doors_Floor_4.Add(item);
            }
            if (item.transform.position.y > 0 && item.transform.position.y < 1)
            {
                Doors_Floor_3.Add(item);
            }
            if (item.transform.position.y > -2 && item.transform.position.y < 0)
            {
                Doors_Floor_2.Add(item);
            }
            if (item.transform.position.y > -4 && item.transform.position.y < -2)
            {
                Doors_Floor_1.Add(item);
            }
        }
        ScoreText = transform.Find("Score").gameObject.GetComponent<Text>();

        List<GameObject> DoorsChecked = new List<GameObject>();
        for (int i = 0; i < EnemiesToSpawnInit; i++)
        {
            int R = UnityEngine.Random.Range(0, Doors.Count-1);
            while (true)
            {
                if (Doors[R].GetComponent<DoorScript>().IsOpen && !DoorsChecked.Contains(Doors[R]))
                {
                    DoorsChecked.Add(Doors[R]);
                    GameObject Tomatello = Instantiate(TomatoEnemy, Doors[R].transform.position, Quaternion.identity);
                    break;
                }

                R = UnityEngine.Random.Range(0, Doors.Count - 1);
            }
            
        }
	}
	
	
	void Update ()
    {
        ScoreText.text = "SCORE: " + Score.ToString("0000");
	}
}
