using UnityEngine;

public class DeathEnemi : MonoBehaviour
{
    private Animator animator;
    public float deathDelay = 1.0f; // ajuste conforme a duração da animação de morte

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magic"))
        {
            animator.SetBool("Dead", true); // ativa a animação de morte
            // Para parar o movimento, se houver um componente Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            Destroy(other.gameObject); // destrói o projétil
            
            StartCoroutine(DieAfterAnimation());
        }
    }

    private System.Collections.IEnumerator DieAfterAnimation()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
