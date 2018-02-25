using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallScript : MonoBehaviour {

    public GameObject IceBlockPref;
    internal int Floor;
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (GameManager.TheGameManager.ThePlayer.Floor == collision.GetComponent<EnemyInfo>().Floor)
            {
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
                GameObject IceBlock = Instantiate(IceBlockPref, collision.gameObject.transform.position, Quaternion.identity);
                GameManager.TheGameManager.EnemiesOnLevel--;
                IceBlock.GetComponent<IceBlockScript>().Floor = Floor;
            }
        }
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
           
        }
    }
}
