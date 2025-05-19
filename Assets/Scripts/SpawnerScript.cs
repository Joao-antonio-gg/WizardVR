using System.Collections;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public uint enemyCount;

    public int spawnRange = 10;
    public float activationDistance = 15f;
    public Transform playerTransform;

    private bool spawningStarted = false;

    void Update()
    {
        if (!spawningStarted && playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Verifica se o jogador acabou de entrar na área de ativação
            if (distanceToPlayer <= activationDistance)
            {
                spawningStarted = true;
                StartCoroutine(EnemyDrop());
            }
        }
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 5)
        {
            if (prefabToSpawn == null)
            {
                Debug.LogWarning("Prefab está nulo ou foi destruído.");
                yield break;
            }

            float xOffset = Random.Range(-spawnRange, spawnRange);
            float zOffset = Random.Range(-spawnRange, spawnRange);

            Vector3 spawnPosition = new Vector3(
                transform.position.x + xOffset,
                transform.position.y,
                transform.position.z + zOffset
            );

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            enemyCount += 1;

            yield return new WaitForSeconds(1f);
        }
    }
}
