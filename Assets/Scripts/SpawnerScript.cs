using System.Collections;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int maxEnemyCount = 5;

    public int spawnRange = 10;
    public float activationDistance = 15f;
    public Transform playerTransform;

    private bool spawningStarted = false;
    private int currentEnemyCount = 0;

    void Start()
    {
        // Tenta atribuir automaticamente o jogador
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogError("Jogador não encontrado! Adicione a tag 'Player' ao jogador.");
            }
        }
    }

    void Update()
    {
        if (!spawningStarted && playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= activationDistance)
            {
                spawningStarted = true;
                StartCoroutine(EnemyDrop());
            }
        }
    }

    IEnumerator EnemyDrop()
    {
        while (currentEnemyCount < maxEnemyCount)
        {
            if (prefabToSpawn == null)
            {
                Debug.LogWarning("Prefab está nulo ou foi destruído.");
                yield break;
            }

            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0f,
                Random.Range(-spawnRange, spawnRange)
            );

            Vector3 spawnPosition = transform.position + randomOffset;

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            currentEnemyCount++;

            yield return new WaitForSeconds(1f);
        }
    }

    // (Opcional) Se quiser decrementar a contagem quando um inimigo morrer:
    public void OnEnemyDestroyed()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }
}
