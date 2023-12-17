using System.Collections;
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

public enum EnemyState {
    IDLE,
    ROAMING,
}

public class Enemy : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    private Transform _target;
    public LayerMask obstacleMask;

    public EnemyTypes enemyType;
    private EnemyState currentState;
    
    private Coroutine currentCoroutine;
    
    public int health = 20;
    public int baseHealth = 20;
    public float moveSpeed = 3f;    

    public float roamingRange = 15f;

    public int baseKillScore = 1;
    public int killScore;

    private Vector3 targetPosition;

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
        SetState(EnemyState.IDLE);
        //SetRandomTarget();
    }

    void Update() {
        switch (currentState) {
            case EnemyState.ROAMING:
                UpdateRoamingState();
                break;
        }
    }

    private void SetState(EnemyState newState) {
        currentState = newState;

        switch (currentState) {
            case EnemyState.IDLE:
                StartCoroutine(SetStateAfterDelay(EnemyState.ROAMING, Random.Range(3f, 8f)));
                break;
            case EnemyState.ROAMING:
                StartCoroutine(SetStateAfterDelay(EnemyState.IDLE, Random.Range(2f, 6f)));
                break;
        }
    }

    private void UpdateRoamingState() {
        transform.position =
            Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            SetRandomTarget();
        }

        var origin = transform.position;
        var direction = targetPosition - origin;
        var distance = Vector3.Distance(origin, targetPosition);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, obstacleMask);
        Debug.DrawRay(origin, direction, spriteRenderer.color);

        if (hit.collider != null) {
            if (hit.collider.CompareTag("Enemy") && hit.collider.gameObject != gameObject) {
                float enemyHitDistance = Vector3.Distance(hit.collider.gameObject.transform
                    .position, gameObject.transform.position);

                if (enemyHitDistance < 2f) {
                    Debug.Log("Recalculate Route");
                    SetRandomTarget();
                }
                
                Debug.Log(
                    $"Enemy detected. CurrentId:{gameObject.GetInstanceID()}, HitId:{hit.collider.gameObject.GetInstanceID()}, Distance:{enemyHitDistance}");
            }
        }
    }
    
    private IEnumerator SetStateAfterDelay(EnemyState state, float delay) {
        yield return new WaitForSeconds(delay);
        SetState(state);
    }

    void SetRandomTarget() {
        float randomX = Random.Range(-roamingRange, roamingRange);
        float randomY = Random.Range(-roamingRange, roamingRange);
        targetPosition = new Vector3(randomX, randomY, 0f);
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