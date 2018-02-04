using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bulletPrefab;
    Weapon w;
    public float moveSpeed = 100;
    Vector3 lastFireDirection;
    PlayerIndex carbonInputId;
    static int globalId = 1;

    float cooldown = 0;

	// Use this for initialization
	void Start () {
        carbonInputId = (PlayerIndex)globalId;
        globalId += 1;
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

        if (cooldown <= 0 &&
            v.magnitude > 0.1)
        {
            GameObject bul = GameObject.Instantiate(bulletPrefab);
            bul.transform.position = this.transform.position + v.normalized * 0.75f;
            bul.GetComponent<Renderer>().material.color = Color.green;
            bul.GetComponent<BulletController>().playerOwner = this.gameObject;

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
