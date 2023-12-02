using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes {
    BASIC_ZOMBIE,
    RUNNER_ZOMBIE,
    TANK_ZOMBIE,
}

public struct EnemyConfig {
    public Color color;
    public int extraKillScore;
    public int healthModifier;
    public float speedModifier;
}

public class Enemy : MonoBehaviour {
    public SpriteRenderer spriteRenderer;

    public EnemyTypes enemyType;

    private Transform _target;

    public int health = 20;
    public int baseHealth = 20;
    public float moveSpeed = 3f;

    public int baseKillScore = 1;
    public int killScore;

    private static readonly Dictionary<EnemyTypes, EnemyConfig> Configs =
        new() {
            {
                EnemyTypes.BASIC_ZOMBIE,
                new EnemyConfig {
                    color = Color.red, extraKillScore = 0, healthModifier = 0,
                    speedModifier = 0f
                }
            }, {
                EnemyTypes.RUNNER_ZOMBIE,
                new EnemyConfig {
                    color = Color.green, extraKillScore = 2,
                    healthModifier = -5, speedModifier = 2f
                }
            }, {
                EnemyTypes.TANK_ZOMBIE,
                new EnemyConfig {
                    color = Color.black, extraKillScore = 9,
                    healthModifier = +30, speedModifier = -1f
                }
            }
        };

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _target = GameObject.FindGameObjectWithTag("Character").transform;

        EnemyConfig config = Configs[enemyType];

        spriteRenderer.color = config.color;
        killScore = baseKillScore + config.extraKillScore;
        health = baseHealth + config.healthModifier;
        moveSpeed += config.speedModifier;
    }

    private void Update() {
        if (_target) {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget() {
        var direction = (_target.position - transform.position).normalized;
        transform.Translate(direction * (moveSpeed * Time.deltaTime));
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
