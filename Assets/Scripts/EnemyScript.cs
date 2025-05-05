using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;      // Referência ao Transform do player
    public float speed = 5f;      // Velocidade de movimento
    public float stopDistance = 2f; // Distância mínima para parar de seguir

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Fazer o objeto olhar para o player (opcional)
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
