using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour {

    public LineRenderer lr;
    public LayerMask lm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        lr.SetPosition(0, this.transform.position);

        Ray2D r = new Ray2D(transform.position, -transform.up);
        RaycastHit2D rh;
        if(rh = Physics2D.Raycast(r.origin, r.direction, Mathf.Infinity, layerMask: lm.value))
        {
            lr.SetPosition(1, rh.point);
        }
        else
        {
            lr.SetPosition(1, this.transform.position - this.transform.up * 100);
        }

    }
}
