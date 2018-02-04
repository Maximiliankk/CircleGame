using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSword : MonoBehaviour {

    public int swordDamage;
    public float playerKnockForce;
    public float bulletKnockForce;
    public Rigidbody2D playerRigidbody;
    public PlayerController player;

    BoxCollider2D swordCollider;

    void Awake()
    {
        swordCollider = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<PlayerController>();
    }

	void Update () {

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject != player.gameObject)
        {
            other.GetComponent<health>().TakeDamage(swordDamage);
            DeflectObject(other.gameObject, playerKnockForce);
        }
        else if (other.tag == "Bullet")
        {
            DeflectObject(other.gameObject, bulletKnockForce);
        }
    }

    void DeflectObject(GameObject other, float knockForce)
    {
        Vector2 dir = other.transform.position - transform.position;
        other.GetComponent<Rigidbody2D>().AddForce(dir * knockForce, ForceMode2D.Impulse);
    }

    public void DisableSwordCollider()
    {
        swordCollider.enabled = false;
    }
}
