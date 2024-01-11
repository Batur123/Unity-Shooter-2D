using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class LootManager : MonoBehaviour {
    private string lootPrefabsFolder = "LootPrefabs";

    public enum LootableItems {
        LOOT_BOX,
        MED_KIT,
        ARMOR_KIT
    }
    
    private Dictionary<LootableItems, string> itemNames = new() {
        { LootableItems.MED_KIT, "Medkit" },
        { LootableItems.ARMOR_KIT, "Armorkit" },
        { LootableItems.LOOT_BOX, "Item_Box_1"}
    };

    public Dictionary<LootableItems, string> GetAllItems() {
        return itemNames;
    }

    public string GetItemName(LootableItems item) {
        return itemNames.TryGetValue(item, out var itemName) ? itemName : item.ToString();
    }
    
    public T GetRandomEnum<T>()
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    }
    
    public void SpawnItem(LootableItems itemType, Vector3 spawnLocation) {
        GameObject itemPrefab = Resources.Load<GameObject>($"{lootPrefabsFolder}/{GetItemName(itemType)}");
        Instantiate(itemPrefab, spawnLocation, Quaternion.identity);
    }
}
