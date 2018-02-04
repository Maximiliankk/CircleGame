using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlRenderer : MonoBehaviour {

    public Material mat;
    public CircleCollider2D circleCollider;

    public float swirlPullMultiplier;
    public float deltaAngularVelocity;

    public Vector2 swirlLifetimeRange;
    public Vector2 timeBetweenSpawnRange;

    public bool activated;
    public float swirlSpeed;

    public float swirlMaxAngle;
    public float swirlAngleDelta;

    List<Rigidbody2D> affectedRigidbodies;
    Rigidbody2D rb2d;

    void Awake ()
    {
        affectedRigidbodies = new List<Rigidbody2D>();
        rb2d = GetComponentInParent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        activated = false;
        BeginSwirl();
        mat.SetFloat("_Angle", 0);
        // Invoke("BeginSwirl", Random.Range(timeBetweenSpawnRange.x, timeBetweenSpawnRange.y));
    }

    void BeginSwirl()
    {
        StartCoroutine(StartSwirlCoroutine());
    }

    void EndSwirl()
    {
        StartCoroutine(EndSwirlCoroutine());
    }

    IEnumerator StartSwirlCoroutine()
    {
        float swirlRadius = 0;
        activated = true;
        while (swirlRadius < swirlMaxAngle)
        {
            swirlRadius = Mathf.Min(swirlMaxAngle, swirlRadius + swirlAngleDelta * Time.deltaTime);
            mat.SetFloat("_Angle", swirlRadius);
            yield return null;
        }

        rb2d.velocity = Random.insideUnitCircle.normalized * swirlSpeed;
        Invoke("EndSwirl", Random.Range(swirlLifetimeRange.x, swirlLifetimeRange.y));
    }

    IEnumerator EndSwirlCoroutine ()
    {
        float swirlRadius = swirlMaxAngle;
        while (swirlRadius > 0)
        {
            swirlRadius = Mathf.Max(0, swirlRadius - swirlAngleDelta * Time.deltaTime);
            mat.SetFloat("_Angle", swirlRadius);
            yield return null;
        }

        activated = false;
        rb2d.velocity = Vector2.zero;
        Invoke("BeginSwirl", Random.Range(timeBetweenSpawnRange.x, timeBetweenSpawnRange.y));
    }

    void Update()
    {
        if (!activated)
        {
            return;
        }

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