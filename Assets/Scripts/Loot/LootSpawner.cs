using UnityEngine;
using Utils;
public class LootSpawner : MonoBehaviour
{
    public Rigidbody2D player;
    public GameObject lootBoxPrefab;
    
    // Spawns Loot Box for Test
    void Start()
    {
        Instantiate(lootBoxPrefab, SpawnUtils.GetRandomSpawnPosition(player.GetComponent<Rigidbody2D>()), Quaternion.identity);
    }
}
