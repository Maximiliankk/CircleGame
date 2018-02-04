using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Vector3 mySpawn;

    public GameObject bulletPrefab;
    public GameObject reticule;
    Weapon w;
    public float moveSpeed = 100;
    Vector3 lastFireDirection;
    Vector3 lastDirection;
    PlayerIndex carbonInputId;
    static int globalId = 1;
    static System.Random random = new System.Random();
    BulletController.BulletType bulletType;
    Color bulletColor;

    static Color[] colors = new Color[] { Color.red, Color.green, Color.black, Color.magenta, Color.yellow, Color.white, Color.gray, Color.cyan };

    SittingPot last_pot_touched = null;
    bool holding_pot = false;

    // each player has an id
    static int s_player_id = 0;
    int player_id;

    float cooldown = 0;
    private Rigidbody2D rb;
    public float bulletGeneralSpeed = 1;

    // Use this for initialization
    void Start () {
        mySpawn = transform.position;

        rb = GetComponent<Rigidbody2D>();
        carbonInputId = (PlayerIndex)globalId;
        globalId += 1;
        player_id = s_player_id++;

        bulletType = (BulletController.BulletType)random.Next((int)BulletController.BulletType.Max);
    }

    public void GoToRespawn()
    {
        transform.position = mySpawn;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCooldownTimer();
        UpdateMovement();
        UpdateShooting();
        UpdatePot();
    }

    public void PlayerOnEnterPot(SittingPot pot)
    {
        last_pot_touched = pot;
    }

    public void PlayerOnExitPot(SittingPot pot)
    {
        if (last_pot_touched == pot)
        {
            last_pot_touched = null;
        }
    }

    void UpdatePot()
    {
        if (!holding_pot && last_pot_touched != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("player picked up the fucking pot");
            last_pot_touched.DeleteYourselfThePot();
            holding_pot = true;
        }

        else if (holding_pot && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("player throws the fucking pot");
            holding_pot = false;
        }
    }

    static Vector3 Skew(Vector3 axis)
    {
        return new Vector3(-axis.y, axis.x, 0);
    }

    static Quaternion QuatFromBasis(Vector3 x, Vector3 y)
    {
        Vector3 z = new Vector3(0, 0, 1);
        Matrix4x4 m = Matrix4x4.identity;
        m.SetColumn(0, new Vector4(x.x, x.y, x.z, 0));
        m.SetColumn(1, new Vector4(y.x, y.y, y.z, 0));
        m.SetColumn(2, new Vector4(z.x, z.y, z.z, 0));
        return m.rotation;
    }

    static Color ColorFromBytes(int r, int g, int b)
    {
        Color c;
        c.r = (float)r / 255.0f;
        c.g = (float)g / 255.0f;
        c.b = (float)b / 255.0f;
        c.a = 1.0f;
        return c;
    }

    static Color ColorFromBytes(int r, int g, int b, int a)
    {
        Color c;
        c.r = (float)r / 255.0f;
        c.g = (float)g / 255.0f;
        c.b = (float)b / 255.0f;
        c.a = (float)a / 255.0f;
        return c;
    }

    void UpdateShooting()
    {
        Vector3 v = new Vector3(GamePad.GetAxis(CAxis.RX, carbonInputId), -GamePad.GetAxis(CAxis.RY, carbonInputId), 0);

        Vector3 keyDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            keyDirection += new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            keyDirection += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            keyDirection += new Vector3(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            keyDirection += new Vector3(1, 0, 0);
        }

        if (keyDirection.magnitude > 0.1) lastDirection = keyDirection;

        // for first player, override the controller directional input with the keyboard
        bool is_first_player = player_id == 0;
        if (is_first_player && Input.GetKey(KeyCode.Space)) v = lastDirection;

        bool controller_axis_is_being_used = v.magnitude > 0.1;
        bool do_shoot = cooldown <= 0 && controller_axis_is_being_used;
        if (do_shoot)
        {
            lastFireDirection = v;
            GameObject bul = GameObject.Instantiate(bulletPrefab);
            bul.transform.position = this.transform.position + v.normalized * 0.75f;
            bul.GetComponent<Renderer>().material.color = colors[(int)carbonInputId - 1];
            bul.GetComponent<BulletController>().playerOwner = this.gameObject;
            bul.GetComponent<BulletController>().bulletType = bulletType;

            //bul.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            //bul.GetComponent<Collider2D>().enabled = false;
            rb.velocity = v.normalized * bulletGeneralSpeed;
            cooldown += 0.25f;
            Destroy(bul,3);

            // recoil
            //var c = v * .1f;
            //this.gameObject.GetComponent<Transform>().position -= c;
        }

        float inverse_parent_scale = 1.0f / transform.localScale.x;
        float default_reticule_scale = 5.25f * inverse_parent_scale;
        float firing_reticule_scale = 10.25f * inverse_parent_scale;

        Vector3 aim_direction = lastDirection.normalized;
        reticule.transform.localPosition = aim_direction * 5.75f * inverse_parent_scale;
        reticule.transform.localRotation = QuatFromBasis(aim_direction, Skew(aim_direction));

        SpriteRenderer sprite = reticule.GetComponent<SpriteRenderer>();

        if (do_shoot)
        {
            reticule.transform.localScale = new Vector3(firing_reticule_scale, firing_reticule_scale, firing_reticule_scale);
            sprite.color = ColorFromBytes(239, 42, 11);
        }

        else
        {
            reticule.transform.localScale = new Vector3(default_reticule_scale, default_reticule_scale, default_reticule_scale);
            sprite.color = ColorFromBytes(142, 142, 142, 127);
        }
    }

    void UpdateMovement()
    {
        //this.transform.position += new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0) * this.moveSpeed;
        //this.transform.position += new Vector3(GamePad.GetAxis(CAxis.LX, carbonInputId), -GamePad.GetAxis(CAxis.LY, carbonInputId), 0) * this.moveSpeed;

        rb.AddForce(new Vector3(GamePad.GetAxis(CAxis.LX, carbonInputId), -GamePad.GetAxis(CAxis.LY, carbonInputId), 0) * this.moveSpeed);
        //rb.AddForce(-Vector3.right * moveSpeed * GamePad.GetAxis(CAxis.LX));
        //rb.AddForce(-Vector3.up * moveSpeed * GamePad.GetAxis(CAxis.LY));
        //rb.AddForce(Vector3.right * moveSpeed * GamePad.GetAxis(CAxis.LX));

        if (player_id == 0)
        {
            if (Input.GetKey(KeyCode.W)) rb.AddForce(Vector3.up * moveSpeed);
            if (Input.GetKey(KeyCode.A)) rb.AddForce(-Vector3.right * moveSpeed);
            if (Input.GetKey(KeyCode.S)) rb.AddForce(-Vector3.up * moveSpeed);
            if (Input.GetKey(KeyCode.D)) rb.AddForce(Vector3.right * moveSpeed);
        }
    }

    void UpdateCooldownTimer()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
}
