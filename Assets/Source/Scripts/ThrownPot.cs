using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownPot : MonoBehaviour {

    public GameObject playerOwner;
    public GameObject potExplodeParticles;
    public AudioClip potCrash;
    public AudioClip potExplode;
    public float detonate_timer;
    float time = 0.0f;

    void DieAndPlayBreak()
    {
        Debug.Log("die mother fucker");
        playerOwner.GetComponent<AudioSource>().PlayOneShot(potCrash);
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time >= detonate_timer)
        {
            DieAndPlayBreak();
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject != playerOwner)
        {
            Instantiate(potExplodeParticles, transform.position, transform.rotation);
            DieAndPlayBreak();
            collision.gameObject.GetComponent<health>().TakeDamage(100, playerOwner);
            playerOwner.GetComponent<AudioSource>().PlayOneShot(potExplode);
        }
        else if(collision.gameObject.tag != "Bullet")
        {
            DieAndPlayBreak();
        }
    }
}
