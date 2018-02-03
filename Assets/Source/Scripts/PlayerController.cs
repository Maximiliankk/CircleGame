using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bulletPrefab;
    Weapon w;
    public float moveSpeed = 1;
    Vector3 lastFireDirection;

    float cooldown = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCooldownTimer();
        UpdateMovement();
        //UpdateShooting();
    }

    void UpdateShooting()
    {
        Vector3 v = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0);

        if (cooldown <= 0 &&
            v.magnitude > 0.001)
        {
            GameObject bul = GameObject.Instantiate(bulletPrefab);
            bul.transform.position = this.transform.position + v * 3;
            bul.GetComponent<Renderer>().material.color = Color.green;
            //bul.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = v.normalized * 3;
            cooldown += 0.25f;
            Destroy(bul,3);
        }
    }

    void UpdateMovement()
    {
        //this.transform.position += new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0) * this.moveSpeed;
        this.transform.position += new Vector3(GamePad.GetAxis(CAxis.LX), GamePad.GetAxis(CAxis.LY), 0) * this.moveSpeed;
    }

    void UpdateCooldownTimer()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
}
