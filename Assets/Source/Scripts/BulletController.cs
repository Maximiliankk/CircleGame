using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public GameObject playerOwner;
    int damage = 10;
    float size = 1;
    short sizeDir = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (size >= 3)
        {
            sizeDir = -1;
        }
        else if (size <= .5)
        {
            sizeDir = 1;
        }

        size += .05f * sizeDir;
        var trans = this.gameObject.GetComponent<Transform>();
        trans.localScale = new Vector3(size, size, size);
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
