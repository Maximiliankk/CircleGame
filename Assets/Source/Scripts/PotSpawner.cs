using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotSpawner : MonoBehaviour {

    public float spawn_min;
    public float spawn_max;
    public GameObject sitting_pot_prefab;

    void SpawnPotCallback()
    {
        GameObject pot = Instantiate(sitting_pot_prefab);
        float x = Random.Range(-8.0f, 8.0f);
        float y = Random.Range(-4.0f, 4.0f);
        Debug.Log(x);
        Debug.Log(y);
        pot.transform.position = new Vector3(x, y, 0);
        Invoke("SpawnPotCallback", Random.Range(spawn_min, spawn_max));
    }

	// Use this for initialization
    void Start () {
        Invoke("SpawnPotCallback", Random.Range(spawn_min, spawn_max));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
