using UnityEngine;

public class Lootable : MonoBehaviour {
    [SerializeField] public Items.LootableItems lootableItem;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Character")) {
            HandleLoot(col.gameObject);
        }
    }

    void HandleLoot(GameObject player) {
        Items.ItemAttributes item = Items.GetItem(lootableItem);

        if (item == null) {
            Destroy(gameObject);
        }

        CharacterStats characterStats = player.GetComponent<CharacterStats>();

        switch (item.EffectType) {
            case Items.EffectType.HEALTH:
                characterStats.Heal(item.Amount);
                break;
            case Items.EffectType.ARMOR:
                characterStats.GiveArmor(item.Amount);
                break;
            case Items.EffectType.NONE:
                break;
        }

        Destroy(gameObject);
    }
}
