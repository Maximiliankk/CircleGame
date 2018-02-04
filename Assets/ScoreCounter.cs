using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour {

    public int Kills;
    public UnityEngine.UI.Text scoreText;

    public void AddKills(int kills)
    {
        Kills += kills;
        scoreText.text = "Kills: " + Kills.ToString();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
