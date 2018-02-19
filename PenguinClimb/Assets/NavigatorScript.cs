using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorScript : MonoBehaviour {

    public GameObject ThisLeadsTo;
    internal int Floor;

    private void Start()
    {
        if (transform.position.y > 2 && transform.position.y < 2.5f)
        {
            Floor = 4;
        }
        if (transform.position.y > 0 && transform.position.y < 1)
        {
            Floor = 3;
        }
        if (transform.position.y > -2 && transform.position.y < 0)
        {
            Floor = 2;
        }
        if (transform.position.y > -4 && transform.position.y < -2)
        {
            Floor = 1;
        }
    }
}
