using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject reticule;
    Weapon w;
    public float moveSpeed = 100;
    Vector3 lastFireDirection;
    PlayerIndex carbonInputId;
    static int globalId = 1;

    // each player has an id
    static int s_player_id = 0;
    int player_id;

    float cooldown = 0;

	// Use this for initialization
	void Start () {
        carbonInputId = (PlayerIndex)globalId;
        globalId += 1;
        player_id = s_player_id++;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCooldownTimer();
        UpdateMovement();
        UpdateShooting();
    }

    static Vector3 Skew(Vector3 axis)
    {
        return new Vector3(-axis.y, axis.x, 0);
    }

    static Quaternion QuatFromBasis(Vector3 x, Vector3 y)
    {
        Vector3 z = new Vector3(0, 0, 1);
        Matrix4x4 m = new Matrix4x4(
            new Vector4(x.x, x.y, x.z, 0),
            new Vector4(y.x, y.y, y.z, 0),
            new Vector4(z.x, z.y, z.z, 0),
            new Vector4(0, 0, 0, 0)
        );
        return m.rotation;
    }

    void UpdateShooting()
    {
        Vector3 v = new Vector3(GamePad.GetAxis(CAxis.RX, carbonInputId), -GamePad.GetAxis(CAxis.RY, carbonInputId), 0);

        // so first player can use wasd and space to play
        bool is_first_player = player_id == 0;
        if (Input.GetKey(KeyCode.Space) && is_first_player)
        {
            v = Vector3.zero;

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

            if (v.magnitude < 0.1)
            {
                v = lastFireDirection;
            }
        }

        if (cooldown <= 0 &&
            v.magnitude > 0.1)
        {
            lastFireDirection = v;
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
