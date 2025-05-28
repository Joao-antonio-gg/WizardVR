using UnityEngine;

public class WindScript : MonoBehaviour, ISpell
{
    public float launchForce = 10f;

    public float speed = 5f;
    private Vector3 moveDirection;
    private bool isMoving = false;
    public float duration = 0.5f; // Duration of the projectile before it is destroyed

    public void FireShot()
    {
        // Use the current forward direction (flattened if needed)
        moveDirection = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        isMoving = true;
        Destroy(gameObject, duration); // Destroy the projectile after a certain duration
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
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
        //gameObject.SetActive(false);
    }

    
}
