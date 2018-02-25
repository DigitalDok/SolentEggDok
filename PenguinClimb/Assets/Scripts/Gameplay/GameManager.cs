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

    internal PlayerMovement ThePlayer;

    [Header("Spawning")]
    public int EnemiesToSpawnInit;

    public float EnemySpawnCooldown;
    private float CurEnemySpawnCooldown;
    internal int EnemiesOnLevel;
    public int MaxEnemiesOnLevel;
    private int PrevLevels=4;

    private void Awake()
    {
        TheGameManager = this;
        ThePlayer = GameObject.Find("Penguin").GetComponent<PlayerMovement>();
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
                    EnemiesOnLevel++;

                    if(Doors_Floor_1.Contains(Doors[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 1;
                    if (Doors_Floor_2.Contains(Doors[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 2;
                    if (Doors_Floor_3.Contains(Doors[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 3;
                    if (Doors_Floor_4.Contains(Doors[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 4;
                    
                    break;
                }

                R = UnityEngine.Random.Range(0, Doors.Count - 1);
            }
            
        }

        
	}
	
	void Update ()
    {
        ScoreText.text = "SCORE: " + Score.ToString("0000");

        if (EnemiesOnLevel < MaxEnemiesOnLevel)
        {
            CurEnemySpawnCooldown += Time.deltaTime;
            if (CurEnemySpawnCooldown > EnemySpawnCooldown)
            {
                CurEnemySpawnCooldown = 0;
                
                List<GameObject> DoorsToWorkWith = new List<GameObject>();

                if (ThePlayer.Floor == 1)
                {
                    foreach (var item in Doors_Floor_2)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_3)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_4)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                }
                if (ThePlayer.Floor == 2)
                {
                    foreach (var item in Doors_Floor_1)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_3)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_4)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                }
                if (ThePlayer.Floor == 3)
                {
                    foreach (var item in Doors_Floor_2)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_1)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_4)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                }
                if (ThePlayer.Floor == 4)
                {
                    foreach (var item in Doors_Floor_2)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_3)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                    foreach (var item in Doors_Floor_1)
                    {
                        DoorsToWorkWith.Add(item);
                    }
                }

                int R = UnityEngine.Random.Range(0, DoorsToWorkWith.Count - 1);

                if (!DoorsToWorkWith[R].GetComponent<DoorScript>().IsOpen)
                    DoorsToWorkWith[R].GetComponent<DoorScript>().OpenDoor();
                
                GameObject Tomatello = Instantiate(TomatoEnemy, DoorsToWorkWith[R].transform.position, Quaternion.identity);
                EnemiesOnLevel++;

                if (Doors_Floor_1.Contains(DoorsToWorkWith[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 1;
                if (Doors_Floor_2.Contains(DoorsToWorkWith[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 2;
                if (Doors_Floor_3.Contains(DoorsToWorkWith[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 3;
                if (Doors_Floor_4.Contains(DoorsToWorkWith[R])) Tomatello.GetComponent<EnemyInfo>().Floor = 4;

                if (PrevLevels == 4) PrevLevels--;
                if (PrevLevels == 3) PrevLevels--;
                if (PrevLevels == 2) PrevLevels = 4;

                

            }
        }
	}
}
