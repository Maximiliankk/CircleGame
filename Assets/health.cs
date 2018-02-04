using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour {

    public GameObject hit_particles_prefab;
    public GameObject smoke_prefab;
    public int currentHealth = 100;
    public int maxHealth = 100;

    bool has_smoke = false;
    public int smokeThreshold = 40;

    public void TakeDamage(int dmg, GameObject source)
    {
        currentHealth -= dmg;
        GameObject hit_particle = Instantiate(hit_particles_prefab);
        hit_particle.transform.position = transform.position;
        gameObject.GetComponent<PlayerController>().DamageFlash();

        if (currentHealth <= 0)
        {
            source.GetComponent<ScoreCounter>().AddKills(1);
            this.gameObject.GetComponent<PlayerController>().enabled = false;
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(RespawnPlayer());
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (currentHealth <= smokeThreshold && !has_smoke)
        {
            GameObject smoke = Instantiate(smoke_prefab);
            smoke.transform.position = gameObject.transform.position;
            smoke.transform.parent = gameObject.transform;
            smoke.transform.localScale *= transform.localScale.x;
            has_smoke = true;
        }
	}

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(5);
        currentHealth = maxHealth;
        this.gameObject.GetComponent<PlayerController>().enabled = true;
        this.gameObject.GetComponent<Renderer>().enabled = true;
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        this.gameObject.GetComponent<PlayerController>().GoToRespawn();
    }
}
