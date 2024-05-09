using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class Character : MonoBehaviour {
    [SerializeField] private float movementSpeed = 10.0f;
    private Rigidbody2D _rigidBody;
    private Vector2 _movement;
    public static InventoryManager inventoryManager; // player's own inventory 
    
    void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    private void Start() {
        inventoryManager = gameObject.AddComponent<InventoryManager>();
        inventoryManager.AddSeed();
    }

    void Move() {
        var keyD = Input.GetKey(KeyCode.D);
        var keyA = Input.GetKey(KeyCode.A);
        var keyW = Input.GetKey(KeyCode.W);
        var keyS = Input.GetKey(KeyCode.S);

        _movement.x = (keyD || keyA) ? (keyD ? 1 : -1) : 0;
        _movement.y = (keyW || keyS) ? (keyW ? 1 : -1) : 0;
    }

    static void InventoryActions() {
        if (Input.GetKeyDown(KeyCode.I)) {
            Dictionary<Item.LootableItems, Item.ItemAttributes> inventory = inventoryManager.GetInventory();
            foreach (var item in inventory) {
                Debug.Log(item.Key + ": " + item.Value.ItemName + ", " + item.Value.Amount);
            }
        }
    }
    
    void Update() {
        InventoryActions();
        Move();
    }

    void FixedUpdate() {
        _rigidBody.MovePosition(_rigidBody.position + _movement * (movementSpeed * Time.fixedDeltaTime));
    }
}
