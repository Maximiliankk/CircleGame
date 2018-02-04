using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownPot : MonoBehaviour {

    public float detonate_timer;
    float time = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time >= detonate_timer)
        {
            Debug.Log("die mother fucker");
            Destroy(gameObject);
        }
	}
}
