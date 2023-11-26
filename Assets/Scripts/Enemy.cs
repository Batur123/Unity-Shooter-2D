using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int health = 20;
    private Transform _target;
    public float moveSpeed = 3f;
    
    private void Start() {
        _target = GameObject.FindGameObjectWithTag("Character").transform;
    }
    
    void Update() {
        if (_target) {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget() {
        Vector3 direction = (_target.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damageAmount) {
        health -= damageAmount;
        if (health <= 0) KillEnemy();
    }

    void KillEnemy() {
        Destroy(gameObject);
    }
}
