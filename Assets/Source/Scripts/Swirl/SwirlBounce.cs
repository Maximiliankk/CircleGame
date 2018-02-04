using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlBounce : MonoBehaviour {

    public SwirlRenderer swirlRenderer;

    Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = transform.parent.GetComponent<Rigidbody2D>();
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            BoxCollider2D box = other as BoxCollider2D;
            Bounds bounds = box.bounds;
            Vector2 point = transform.position;

            float newAngle = 0;

            if (point.x < bounds.min.x)
            {
                newAngle = Random.Range(Mathf.PI / 2, 3 * Mathf.PI / 2);
            }
            else if (point.x > bounds.max.x)
            {
                newAngle = Random.Range(-Mathf.PI / 2, Mathf.PI / 2);
            }
            else if (point.y < bounds.min.y)
            {
                newAngle = Random.Range(Mathf.PI, 2 * Mathf.PI);
            }
            else
            {
                newAngle = Random.Range(0, Mathf.PI);
            }

            rb2d.velocity = new Vector2(Mathf.Cos(newAngle) * swirlRenderer.swirlSpeed, Mathf.Sin(newAngle) * swirlRenderer.swirlSpeed);

        }
    }
}
