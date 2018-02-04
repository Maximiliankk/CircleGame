﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingPot : MonoBehaviour {

    public bool isSword;
    public bool pickedUpSword;

    public void DeleteYourselfThePot()
    {
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && !pickedUpSword)
        {
            player.PlayerOnExitPot(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.PlayerOnEnterPot(this);
            pickedUpSword = isSword;

            if (isSword)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
