﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bulletPrefab;
    Weapon w;
    public float moveSpeed = 100;
    Vector3 lastFireDirection;
    PlayerIndex carbonInputId;
    static int globalId = 1;
    static System.Random random = new System.Random();
    BulletController.BulletType bulletType;

    // each player has an id
    static int s_player_id = 0;
    int player_id;

    float cooldown = 0;

	// Use this for initialization
	void Start () {
        carbonInputId = (PlayerIndex)globalId;
        globalId += 1;
        player_id = s_player_id++;

        bulletType = (BulletController.BulletType)random.Next((int)BulletController.BulletType.Max);
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCooldownTimer();
        UpdateMovement();
        UpdateShooting();
    }

    void UpdateShooting()
    {
        Vector3 v = new Vector3(GamePad.GetAxis(CAxis.RX, carbonInputId), -GamePad.GetAxis(CAxis.RY, carbonInputId), 0);

        // so first player can use wasd and space to play
        bool is_first_player = player_id == 0;
        if (Input.GetKeyDown(KeyCode.Space) && is_first_player)
        {
            v.Set(0, 0, 0);

            if (Input.GetKey(KeyCode.W))
            {
                v += new Vector3(0, 1, 0);
            }

            if (Input.GetKey(KeyCode.A))
            {
                v += new Vector3(-1, 0, 0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                v += new Vector3(0, -1, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                v += new Vector3(1, 0, 0);
            }
        }

        if (cooldown <= 0 &&
            v.magnitude > 0.1)
        {
            GameObject bul = GameObject.Instantiate(bulletPrefab);
            bul.transform.position = this.transform.position + v.normalized * 0.75f;
            bul.GetComponent<Renderer>().material.color = Color.green;
            bul.GetComponent<BulletController>().playerOwner = this.gameObject;
            bul.GetComponent<BulletController>().bulletType = bulletType;

            //bul.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            //bul.GetComponent<Collider2D>().enabled = false;
            rb.velocity = v.normalized * 3;
            cooldown += 0.25f;
            Destroy(bul,3);

            // recoil
            //var c = v * .1f;
            //this.gameObject.GetComponent<Transform>().position -= c;
        }
    }

    void UpdateMovement()
    {
        //this.transform.position += new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0) * this.moveSpeed;
        this.transform.position += new Vector3(GamePad.GetAxis(CAxis.LX, carbonInputId), -GamePad.GetAxis(CAxis.LY, carbonInputId), 0) * this.moveSpeed;
    }

    void UpdateCooldownTimer()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
}
