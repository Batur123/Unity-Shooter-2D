using UnityEngine;
using Utils;
public class LootSpawner : MonoBehaviour {
    public LootManager lootManager;
    public Rigidbody2D player;

    private void Start() {
        lootManager = GetComponent<LootManager>();
        
        // Spawns Loot Box for Test
        SpawnLootBox(SpawnUtils.GetRandomSpawnPosition(player.GetComponent<Rigidbody2D>()));
    }

    public void SpawnLootBox(Vector3 lootPosition) {
        lootManager.SpawnItem(LootManager.LootableItems.LOOT_BOX, lootPosition);
    }
    
    public void SpawnRandomItem(Vector3 lootPosition) {
        LootManager.LootableItems randomItem = lootManager.GetRandomEnum<LootManager.LootableItems>();
        lootManager.SpawnItem(randomItem, lootPosition);
    }
}
