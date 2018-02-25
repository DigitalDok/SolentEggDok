using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour {

    public int Floor;
    public int HP;

    internal PlayerMovement ThePlayer;

    private void Start()
    {
        ThePlayer = GameManager.TheGameManager.ThePlayer;
    }
}
