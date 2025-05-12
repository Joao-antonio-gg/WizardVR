using System.Collections;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject prefabToSpawn; // O prefab a ser instanciado
    public uint enemyCount; // Quantidade atual de inimigos

    public int spawnRange = 10; // Raio em torno do spawner para gerar inimigos

    void Start()
    {
        StartCoroutine(EnemyDrop()); // Inicia a coroutine de spawn
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 5)
        {
            float xOffset = Random.Range(-spawnRange, spawnRange);
            float zOffset = Random.Range(-spawnRange, spawnRange);

            Vector3 spawnPosition = new Vector3(
                transform.position.x + xOffset,
                transform.position.y,
                transform.position.z + zOffset
            );

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            enemyCount += 1;
        }
    }
}
