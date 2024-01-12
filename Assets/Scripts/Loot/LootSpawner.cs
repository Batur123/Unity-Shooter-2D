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
        lootManager.SpawnItem(Items.LootableItems.LOOT_BOX, lootPosition);
    }
    
    public void SpawnRandomItem(Vector3 lootPosition) {
        Items.LootableItems randomItem = lootManager.GetRandomEnum<Items.LootableItems>();
        lootManager.SpawnItem(randomItem, lootPosition);
    }
}
