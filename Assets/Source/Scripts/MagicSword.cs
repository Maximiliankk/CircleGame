using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSword : MonoBehaviour {

    public int swordDamage;
    public float playerKnockForce;
    public float bulletKnockForce;
    public Rigidbody2D playerRigidbody;
    public PlayerController player;
    public float secondsBetweenDamage;

    BoxCollider2D swordCollider;
    GameObject hurtPlayer;

    void Awake()
    {
        swordCollider = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject != player.gameObject)
        {
            hurtPlayer = other.gameObject;
            InvokeRepeating("HurtPlayer", 0, secondsBetweenDamage);
        }
        else if (other.tag == "Bullet")
        {
            DeflectObject(other.gameObject, bulletKnockForce);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject == hurtPlayer)
        {
            CancelInvoke("HurtPlayer");
            hurtPlayer = null;
        }
    }

    void HurtPlayer()
    {
        hurtPlayer.GetComponent<health>().TakeDamage(swordDamage);
        hurtPlayer.GetComponent<health>().TakeDamage(swordDamage);
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
