using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour {
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _maxArmor = 100;
    [SerializeField] private int _health = 100;
    [SerializeField] private int _armor = 100;

    private void Start() {
        UpdateUI();
    }

    private void UpdateUI() {
        UIController.Instance.SetTextValue(UIController.TextType.HEALTH_TEXT, $"Health: {_health.ToString()}");
        UIController.Instance.SetTextValue(UIController.TextType.ARMOR_TEXT, $"Armor: {_armor.ToString()}");
    }
    
    public int GetHealth() {
        return _health;
    }

    public int GetArmor() {
        return _armor;
    }

    public bool ShouldGiveHeal() {
        return _maxHealth != _health;
    }
    
    public bool ShouldGiveArmor() {
        return _maxArmor != _armor;
    }
    
    public void Heal(int healAmount) {
        _health += healAmount;
        if (_health >= _maxHealth) _health = _maxHealth;
        UpdateUI();
    }
    
    public void GiveArmor(int armorAmount) {
        _armor += armorAmount;
        if (_armor >= _maxArmor) _armor = _maxArmor;
        UpdateUI();
    }
    
    public void TakeDamage(int damageAmount) {
        int remainingDamage = Mathf.Max(0, damageAmount - _armor);
    
        _armor = Mathf.Max(0, _armor - damageAmount);
        _health -= remainingDamage;

        UpdateUI();
        
        Debug.Log($"Character took damage, current health {_health} and armor {_armor}");
        if (_health <= 0) {
            Die();
        }
    }
    
    private void Die() {
        Debug.Log("You are dead.");
        // Need to handle null pointer errors.
        this.GameObject().SetActive(false);
    }
}