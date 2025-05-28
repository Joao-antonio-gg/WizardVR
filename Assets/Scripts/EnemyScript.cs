using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;            // Referência ao Transform do jogador
    public float speed = 5f;            // Velocidade do inimigo
    public float stopDistance = 2f;     // Distância para parar de andar e começar a atacar
    private Animator animator;          // Referência ao Animator

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
        if (player == null) return;
        player = Camera.main.transform;
        Vector3 player_position = player.position;
        player_position.y = 1;
        float distance = Vector3.Distance(transform.position, player_position);
        
        
        

        if (distance > stopDistance)
        {
            // Calcula a direção e move em direção ao jogador
            Vector3 direction = (player_position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Rotaciona apenas no eixo Y (ignora inclinação vertical)
            Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);
            if (lookDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Desativa animação de ataque
            if (animator != null)
                animator.SetBool("Atack", false);
        }
        else
        {
            // Dentro do raio de ataque: ativa animação
            if (animator != null)
                animator.SetBool("Atack", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica se colidiu com o jogador
        if (collision.gameObject.CompareTag("Wind"))
        {
            // Push goblin back in the opposite direction of the collision contact point
            if (collision.contacts.Length > 0)
            {
                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 pushDirection = (transform.position - contactPoint).normalized;
                // Zero out the Y component to keep push on X,Z plane
                pushDirection.y = 0f;
                pushDirection = pushDirection.normalized;
                float pushDistance = 0.8f; // Distância do empurrão
                transform.position += pushDirection * pushDistance;
            }
        }
    }
}
