using UnityEngine;
using System.Collections.Generic;
public class SpellSpawner : MonoBehaviour
{
    public List<GameObject> spellPrefabs; // List of spell prefabs to spawn
    public Transform spawnPoint; // Point where the spell will be spawned
    public Transform alternateSpawnPoint; // Alternate spawn point for the spell
    public GameObject windVFXPrefab; // Prefab for the wind spell VFX

    public void SpawnSpell(string spellName)
    {
        // Increase spawnpoint forward position
        spawnPoint.position += spawnPoint.forward * 0.05f; // In front of the wand
        foreach (var item in spellPrefabs){
            if (item.name == spellName)
            {
                Vector3 position = spawnPoint.position;
                Quaternion rotation = spawnPoint.rotation; // Default to wand's rotation

                if (spellName == "Wind") // Replace with your actual AoE spell name
                {
                    // Keep position in front of wand, but flatten the rotation
                    Vector3 flatForward = new Vector3(spawnPoint.forward.x, 0f, spawnPoint.forward.z).normalized;
                    rotation = Quaternion.LookRotation(flatForward);

                    // Adjust position to be slightly below the wand
                    position = spawnPoint.position + flatForward * 0.5f; // Adjust as needed

                    // Spawn vfx prefab on alternate spawn point
                    if (alternateSpawnPoint != null && windVFXPrefab != null)
                    {
                        Vector3 vfxPosition = alternateSpawnPoint.position + Vector3.up * 0.8f;
                        Instantiate(windVFXPrefab, vfxPosition, Quaternion.identity);
                    }

                }

                GameObject spell = Instantiate(item, position, rotation);
                //spell.transform.SetParent(spawnPoint); // Set the parent to the spawn point

                ISpell spellScript = spell.GetComponent<ISpell>();
                if (spellScript != null)
                {
                    spellScript.FireShot();
                }
                else
                {
                    Debug.LogWarning("The spawned spell does not implement ISpell.");
                }

                break; // Exit the loop after spawning the spell
            }
        }
    }
}
