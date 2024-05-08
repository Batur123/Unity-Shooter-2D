using System;
using UnityEngine;
using Items;
using Unity.VisualScripting;

public class Lootable : MonoBehaviour {
    [SerializeField] public Item.LootableItems lootableItem;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Character")) {
            HandleLoot(col.gameObject);
        }
    }

    void HandleLoot(GameObject player) {
        Item.ItemAttributes item = Item.GetItem(lootableItem);

        if (item == null) {
            Destroy(gameObject);
        }
        
        CharacterStats characterStats = player.GetComponent<CharacterStats>();
        
        switch (item.EffectType) {
            case Item.EffectType.HEALTH:
                if (characterStats.ShouldGiveHeal()) {
                    characterStats.Heal(item.Amount);
                    ConsumeLoot();
                }
                break;
            case Item.EffectType.ARMOR:
                if (characterStats.ShouldGiveArmor()) {
                    characterStats.GiveArmor(item.Amount);
                    ConsumeLoot();
                }
                break;
            case Item.EffectType.NONE:
                ConsumeLoot();
                break;
        }
    }

    void ConsumeLoot() {
        Destroy(gameObject);
    }
}
