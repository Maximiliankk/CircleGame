using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlRenderer : MonoBehaviour {

    public Material mat;
    public CircleCollider2D circleCollider;
    public float swirlPullMultiplier;
    public float deltaAngularVelocity;

    List<Rigidbody2D> affectedRigidbodies;

    void Awake ()
    {
        affectedRigidbodies = new List<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
        mat.SetVector("_Center", new Vector4(pos.x, pos.y));

        foreach (Rigidbody2D rb2d in affectedRigidbodies)
        {
            float dist = Vector2.Distance(transform.position, rb2d.transform.position);
            rb2d.AddForce((transform.position - rb2d.transform.position).normalized * Mathf.Clamp01(1 - dist/circleCollider.radius) * swirlPullMultiplier);
            rb2d.AddTorque(deltaAngularVelocity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
        if (rb2d && !affectedRigidbodies.Contains(rb2d))
        {
            affectedRigidbodies.Add(rb2d);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
        if (rb2d && affectedRigidbodies.Contains(rb2d))
        {
            affectedRigidbodies.Remove(rb2d);
        }
    }
}