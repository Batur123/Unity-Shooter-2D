using System.Collections.Generic;
using Items;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    private Dictionary<Item.LootableItems, Item.ItemAttributes> ItemList = new();

    public Dictionary<Item.LootableItems, Item.ItemAttributes> GetInventory() {
        return ItemList;
    }

    public void AddSeed() {
        Item.LootableItems seedType = Item.LootableItems.CORN_SEED;
        Item.ItemAttributes itemAttributes = new Item.ItemAttributes { ItemName = "Corn Seed", EffectType = Item.EffectType.NONE, Amount = 5 };
        ItemList.Add(seedType, itemAttributes);
    }
}
