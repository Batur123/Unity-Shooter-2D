using System.Collections.Generic;

namespace Items {
public class Item {
    public enum LootableItems {
        LOOT_BOX,
        MED_KIT,
        ARMOR_KIT,
        CORN_SEED,
    }

    public enum EffectType {
        HEALTH,
        ARMOR,
        NONE,
    }

    public record ItemAttributes {
        public string ItemName;
        public EffectType EffectType;
        public int Amount;
    }

    public static Dictionary<LootableItems, ItemAttributes> ItemList = new() {
        { LootableItems.MED_KIT, new ItemAttributes { ItemName = "Medkit", EffectType = EffectType.HEALTH, Amount = 20 } },
        { LootableItems.ARMOR_KIT, new ItemAttributes { ItemName = "Armorkit", EffectType = EffectType.ARMOR, Amount = 30 } },
        { LootableItems.LOOT_BOX, new ItemAttributes { ItemName = "Item_Box_1", EffectType = EffectType.NONE, Amount = 0 } }
    };

    public static Dictionary<LootableItems, ItemAttributes> GetAllItems() {
        return ItemList;
    }

    public static ItemAttributes GetItem(LootableItems item) {
        Dictionary<LootableItems, ItemAttributes> allItems = GetAllItems();
        return allItems.TryGetValue(item, out var outItem) ? outItem : null;
    }

    public static string GetItemName(LootableItems item) {
        return GetAllItems().TryGetValue(item, out var outItem) ? outItem.ItemName : item.ToString();
    }
}
}
