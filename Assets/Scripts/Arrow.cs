using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;

    private float lifeTimer = 2f, timer;

    private bool hitSomething = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTimer)
            Destroy(gameObject);

        if (!hitSomething)
            transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("Arrow"))
        {
            hitSomething = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
