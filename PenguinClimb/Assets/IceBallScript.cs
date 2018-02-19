using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallScript : MonoBehaviour {

    public GameObject IceBlockPref;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            GameObject IceBlock = Instantiate(IceBlockPref, collision.gameObject.transform.position, Quaternion.identity);
        }
    }
}
