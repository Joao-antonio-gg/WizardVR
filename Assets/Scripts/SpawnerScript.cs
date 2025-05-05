using System.Collections;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject prefabToSpawn; // The prefab to spawn
    public int xPos;
    public int zPos;
    public uint enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop()); // Start the coroutine to spawn enemies
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 5)
        {
            xPos = Random.Range(-10, 10);
            zPos = Random.Range(-10, 10);
            Instantiate(prefabToSpawn, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(1f); // Wait for 1 second before spawning the next enemy
            enemyCount += 1; // Increment the enemy count
        }
    }   



}
