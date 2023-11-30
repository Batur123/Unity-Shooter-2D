using System;
using UnityEngine;

public enum EnemyTypes {
    BASIC_ZOMBIE,
    RUNNER_ZOMBIE,
    TANK_ZOMBIE,
}

public class Enemy : MonoBehaviour {
    public EnemyTypes enemyType;
    
    private Transform _target;
    
    public int health = 20;
    public int baseHealth = 20;
    public float moveSpeed = 3f;

    public int baseKillScore = 1;
    public int killScore;
    
    private void Start() {
        _target = GameObject.FindGameObjectWithTag("Character").transform;
        
        switch(enemyType)
        {
            case EnemyTypes.BASIC_ZOMBIE:
                killScore = baseKillScore;
                health = baseHealth;
                break;
            case EnemyTypes.RUNNER_ZOMBIE:
                killScore = baseKillScore + 2;
                health = baseHealth - 5;
                moveSpeed += 2f;
                break;
            case EnemyTypes.TANK_ZOMBIE:
                killScore = baseKillScore + 9;
                health = baseHealth + 30;
                moveSpeed -= 1f;
                break;
        }
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
        ScoreManager.Instance.AddScore(killScore); 
        Destroy(gameObject);
    }
}
