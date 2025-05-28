using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;            // Referência ao Transform do jogador
    public float speed = 5f;            // Velocidade do inimigo
    public float stopDistance = 2f;     // Distância para parar de andar e começar a atacar
    private Animator animator;          // Referência ao Animator
    private bool isDead = false;        // Verifica se está morto

    void Start()
    {
        // Tenta encontrar o jogador automaticamente pela tag "Player"
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Jogador não encontrado! Atribua a tag 'Player' ao objeto do jogador.");
            }
        }

        // Pega o Animator se existir
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || isDead)
        {
            if (isDead) Debug.Log($"{gameObject.name} está morto, não se move.");
            return;
        }

        // Mantém sempre olhando a posição do player na câmera
        player = Camera.main.transform;
        Vector3 player_position = player.position;
        player_position.y = 1;  // Força a altura para 1

        float distance = Vector3.Distance(transform.position, player_position);

        if (distance > stopDistance)
        {
            // Calcula direção e move
            Vector3 direction = (player_position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Rotaciona no eixo Y
            Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);
            if (lookDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            if (animator != null)
            {
                animator.SetBool("Atack", false);
                animator.SetBool("Run", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("Atack", true);
                animator.SetBool("Run", false);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;  // Se está morto, ignora colisões

        if (collision.gameObject.CompareTag("Wind"))
        {
            if (collision.contacts.Length > 0)
            {
                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 pushDirection = (transform.position - contactPoint).normalized;
                pushDirection.y = 0f;
                pushDirection = pushDirection.normalized;
                float pushDistance = 0.8f;
                transform.position += pushDirection * pushDistance;
            }
        }

        // Exemplo de morrer ao colidir com "Sword"
        if (collision.gameObject.CompareTag("Sword"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} morreu!");

        if (animator != null)
        {
            animator.SetTrigger("Die");       // Animação de morte
            animator.SetBool("Atack", false); // Para ataque
            animator.SetBool("Run", false);   // Para corrida
        }

        // Desativa o colisor
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Se tiver Rigidbody, para física
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;  // Não sofre mais forças físicas
        }

        // Destrói o objeto após 2 segundos
        Destroy(gameObject, 2f);
    }
}
