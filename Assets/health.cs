using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour {

    public int currentHealth = 100;
    public int maxHealth = 100;

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
