using System;
using UnityEngine;
using Items;
using Unity.VisualScripting;

public class Lootable : MonoBehaviour {
    [SerializeField] public Item.LootableItems lootableItem;
    public CharacterStats characterStats;
    public Character character;

    private void Awake() {
        characterStats = gameObject.AddComponent<CharacterStats>();
        character = gameObject.AddComponent<Character>();
    }

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
