using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public GameObject playerOwner;
    int damage = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" &&
            collision.gameObject != playerOwner)
        {
            collision.gameObject.GetComponent<health>().TakeDamage(damage);
            Destroy(this.gameObject);
            Debug.Log("damaged, health left: " + collision.gameObject.GetComponent<health>().currentHealth);
        }
    }
}
