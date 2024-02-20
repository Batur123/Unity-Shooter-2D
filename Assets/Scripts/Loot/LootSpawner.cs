using Items;
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
        lootManager.SpawnItem(Item.LootableItems.LOOT_BOX, lootPosition);
    }
    
    public void SpawnRandomItem(Vector3 lootPosition) {
        Item.LootableItems randomItem = lootManager.GetRandomEnum<Item.LootableItems>();
        lootManager.SpawnItem(randomItem, lootPosition);
    }
}
