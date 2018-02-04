using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupStuff : MonoBehaviour {

    List<GameObject> players;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<health>().maxHealth += 10;
            Destroy(this.gameObject);
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
