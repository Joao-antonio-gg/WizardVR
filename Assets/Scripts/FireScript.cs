using UnityEngine;

public class Projectile : MonoBehaviour, ISpell
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
        rb.useGravity = false;
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

        // If collided with XR Rig, do nothing so the projectile keeps going
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name.Contains("XRRig"))
        {
            Debug.Log("Projectile hit the player, ignoring collision.");
            return;
        }

        // Disable the projectile after it hits something else
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
