using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public float maxLifetime = 1.5f;
    private float _timer = 0.0f;
    public int damageAmount = 5;
    private bool _hasHit = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_hasHit) return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy) {
            enemy.TakeDamage(damageAmount);
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