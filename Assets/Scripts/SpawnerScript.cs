using System.Collections;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int maxEnemyCount = 5;
    public int spawnRange = 10;
    public float activationDistance = 15f;
    public Transform playerTransform;
    public Vector3 targetPosition;

    private bool spawningStarted = false;
    private bool enemiesSpawned = false;

    void Start()
    {
        // Atribui o jogador automaticamente, se não estiver definido
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogError("Jogador não encontrado! Adicione a tag 'Player'.");
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

        // Após o spawn, verifica se todos os inimigos morreram
        if (enemiesSpawned)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                MovePlayer();
                enemiesSpawned = false; // evita mover mais de uma vez
            }
        }
    }

    IEnumerator EnemyDrop()
    {
        for (int i = 0; i < maxEnemyCount; i++)
        {
            if (prefabToSpawn == null)
            {
                Debug.LogWarning("Prefab está nulo.");
                yield break;
            }

            Vector3 offset = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0f,
                Random.Range(-spawnRange, spawnRange)
            );

            Vector3 spawnPos = transform.position + offset;
            GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            // Garante que o inimigo tenha a tag "Enemy"
            enemy.tag = "Enemy";

            yield return new WaitForSeconds(1f);
        }

        enemiesSpawned = true;
    }

    void MovePlayer()
    {
        if (playerTransform != null)
        {
            playerTransform.position = targetPosition;
            Debug.Log("Todos os inimigos derrotados. Jogador movido.");
        }
    }
}
