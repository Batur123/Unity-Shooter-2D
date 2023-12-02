using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public CharacterShooting characterShooting;
    public float maxLifetime = 1.5f;
    private float _timer = 0.0f;
    private bool _hasHit = false;

    private void Awake() {
        GameObject player = GameObject.FindGameObjectWithTag("Character");
        if (player != null) {
            characterShooting = player.GetComponent<CharacterShooting>();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (_hasHit) return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy) {
            enemy.TakeDamage(characterShooting.damageAmount);
            _hasHit = true;
            Destroy(gameObject);
        }
    }


    void Update() {
        _timer += Time.deltaTime;

        if (_timer >= maxLifetime) {
            Destroy(gameObject);
        }
    }
}