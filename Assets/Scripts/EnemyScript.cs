using Unity.VisualScripting;
using UnityEngine;

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
        float distance = Vector3.Distance(transform.position, player.position);
        

        if (distance > stopDistance)
        {
            // Calcula a direção e move em direção ao jogador
            Vector3 direction = (player.position - transform.position).normalized;
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
}
