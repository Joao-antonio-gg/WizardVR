using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchForce = 10f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the projectile!");
        }
    }

    public void FireShot()
    {
        if (rb == null) return;

        // Reset velocity in case the object is reused
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Apply forward force
        rb.AddForce(transform.forward * launchForce, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Print the name of the object collided with
        Debug.Log($"Projectile collided with: {collision.gameObject.name}");

        // Optional: Add impact effects here (e.g., particles, sound)

        // Disable the projectile after it hits something
        gameObject.SetActive(false);
    }

    // Optional: reset the projectile when disabled (useful for pooling)
    void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
