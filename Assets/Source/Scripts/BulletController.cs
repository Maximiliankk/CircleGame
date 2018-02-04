using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public GameObject playerOwner;
    int damage = 10;
    float size = 1;
    short sizeDir = 1;
    public BulletType bulletType;
    System.Random random;

	// Use this for initialization
	void Start () {
        random = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
		switch (bulletType)
        {
            case BulletType.SizeChange:
                ChangeSize();
                break;
            case BulletType.Fast:
                FastBullet();
                break;
            case BulletType.Random:
                RandomBullet();
                break;
            default:
                break;
        }
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

    private void FastBullet()
    {
        GetComponent<Rigidbody2D>().velocity *= 1.01f;
    }

    private void RandomBullet()
    {
        if (random.Next(0, 13) % 3 != 0)
        {
            return;
        }
        var size = random.Next(7, 14) / 10.0f;
        var speed = random.Next(30, 70) / 50.0f;
        var directonX = random.Next(-10, 10) / 100.0f;
        var directonY = random.Next(-10, 10) / 100.0f;

        var trans = GetComponent<Transform>();
        var rigid = GetComponent<Rigidbody2D>();

        if (trans.localScale.x < .5f)
        {
            size += 1;
        }
        trans.localScale *= size;//new Vector3(size, size);
        rigid.velocity *= speed;// new Vector2(speed, speed);
        rigid.velocity += new Vector2(directonX, directonY);
    }

    private void ChangeSize()
    {
        if (size >= 3)
        {
            sizeDir = -1;
        }
        else if (size <= .5)
        {
            sizeDir = 1;
        }

        size += .05f * sizeDir;
        var trans = GetComponent<Transform>();
        trans.localScale = new Vector3(size, size, size);
    }

    public enum BulletType
    {
        SizeChange,
        Fast,
        Random,
        Wave,

        // don't add values after this
        Max
    }
}
