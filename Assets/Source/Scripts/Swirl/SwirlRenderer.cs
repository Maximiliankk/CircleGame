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

    public Vector2 minPosition;
    public Vector2 maxPosition;

    List<Rigidbody2D> affectedRigidbodies;
    Rigidbody2D rb2d;
    bool finishing;

    void Awake ()
    {
        affectedRigidbodies = new List<Rigidbody2D>();
        rb2d = GetComponentInParent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb2d.velocity = Vector2.zero;
        activated = false;
        finishing = false;
        BeginSwirl();
        mat.SetFloat("_Angle", 0);
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
        float swirlAngle = 0;
        activated = true;
        rb2d.velocity = Vector2.zero;

        transform.position = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));

        while (swirlAngle < swirlMaxAngle)
        {
            swirlAngle = Mathf.Min(swirlMaxAngle, swirlAngle + swirlAngleDelta * Time.deltaTime);
            mat.SetFloat("_Angle", swirlAngle);
            yield return null;
        }

        rb2d.velocity = Random.insideUnitCircle.normalized * swirlSpeed;
        Invoke("EndSwirl", Random.Range(swirlLifetimeRange.x, swirlLifetimeRange.y));
    }

    IEnumerator EndSwirlCoroutine ()
    {
        float swirlRadius = swirlMaxAngle;
        finishing = true;

        foreach (Rigidbody2D rb in affectedRigidbodies)
        {
            if (rb)
            {
                rb.drag = 4;
                PlayerController pc = rb.GetComponent<PlayerController>();
                if (pc)
                {
                    rb.GetComponent<PlayerController>().SetMovementAllowed(true);
                }
            }
        }

        while (swirlRadius > 0)
        {
            swirlRadius = Mathf.Max(0, swirlRadius - swirlAngleDelta * Time.deltaTime);
            mat.SetFloat("_Angle", swirlRadius);
            yield return null;
        }

        activated = false;
        finishing = false;
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
            if (rb2d)
            {
                float dist = Vector2.Distance(transform.position, rb2d.transform.position);
                rb2d.AddForce((transform.position - rb2d.transform.position).normalized * Mathf.Clamp01(1 - dist / circleCollider.radius) * swirlPullMultiplier);
                rb2d.AddTorque(deltaAngularVelocity);
                rb2d.freezeRotation = false;
                rb2d.drag = 1;

                if (dist <= 2 && !finishing)
                {
                    PlayerController pc = rb2d.GetComponent<PlayerController>();
                    if (pc)
                    {
                        rb2d.GetComponent<PlayerController>().SetMovementAllowed(false);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
        if (rb2d && !affectedRigidbodies.Contains(rb2d) && other.tag != "Sword")
        {
            affectedRigidbodies.Add(rb2d);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
        if (rb2d && affectedRigidbodies.Contains(rb2d))
        {
            rb2d.drag = 4;
            affectedRigidbodies.Remove(rb2d);
            PlayerController pc = rb2d.GetComponent<PlayerController>();
            if (pc)
            {
                rb2d.GetComponent<PlayerController>().SetMovementAllowed(true);
            }
        }
    }
}