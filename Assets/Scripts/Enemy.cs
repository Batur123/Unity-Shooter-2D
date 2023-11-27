using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int health = 20;
    private Transform _target;
    public float moveSpeed = 3f;
    
    private void Start() {
        _target = GameObject.FindGameObjectWithTag("Character").transform;
    }
    
    private void Update() {
        if (_target) {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget() {
        var direction = (_target.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damageAmount) {
        health -= damageAmount;
        if (health <= 0) KillEnemy();
    }

    private void KillEnemy() {
        Destroy(gameObject);
    }
}
