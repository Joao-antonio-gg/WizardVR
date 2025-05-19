using UnityEngine;
using System.Collections.Generic;
public class SpellSpawner : MonoBehaviour
{
    public List<GameObject> spellPrefabs; // List of spell prefabs to spawn
    public Transform spawnPoint; // Point where the spell will be spawned

    public void SpawnSpell(string spellName)
    {
        foreach (var item in spellPrefabs){
            if (item.name == spellName)
            {
                GameObject spell = Instantiate(item, spawnPoint.position, spawnPoint.rotation);
                spell.transform.SetParent(spawnPoint); // Set the parent to the spawn point

                Projectile projectile = spell.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.FireShot();
                }
                else
                {
                    Debug.LogWarning("The spawned spell does not have a Projectile script attached.");
                }

                break; // Exit the loop after spawning the spell
            }
        }
    }
}
