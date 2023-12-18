using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    CHASING,
}

public class Enemy : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    private Transform _target;
    public LayerMask obstacleMask;
    public LayerMask characterMask;

    public EnemyTypes enemyType;
    private EnemyState _currentState;

    private Coroutine _currentCoroutine;

    public int health = 20;
    public int baseHealth = 20;
    public float moveSpeed = 3f;

    public float roamingRange = 15f;

    public int baseKillScore = 1;
    public int killScore;

    private Vector3 _targetPosition;

    public float distanceThreshold = 5f;

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
        obstacleMask = LayerMask.GetMask("Player", "Enemy");
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
        DetectEnemies();
        
        switch (_currentState) {
            case EnemyState.ROAMING:
                UpdateRoamingState();
                break;
            case EnemyState.CHASING:
                UpdateChasingState();
                break;
        }
    }

    private void DetectEnemies() {
        var enemyPos = transform.position;
        var characterPos = _target.position;
        var distance = Vector2.Distance(enemyPos, characterPos);

        if (distance > 15f) {
            return;
        }

        var direction = characterPos - enemyPos;
        
        var dotProduct = Vector2.Dot(direction.normalized, transform.right);
        
        if (dotProduct > 0.9f && (distance < distanceThreshold)) {
            Debug.DrawRay(enemyPos, direction, Color.yellow);
            
            if (_currentState == EnemyState.CHASING) {
                return;
            }
            
            SetState(EnemyState.CHASING);
        }
    }

    private void SetState(EnemyState newState) {
        if (_currentCoroutine != null) {
            StopCoroutine(_currentCoroutine); // Stop the previous coroutine
        }
    
        _currentCoroutine = StartCoroutine(SetStateAfterDelay(newState));
    }

    private void RotateEnemyTowardsPlayer() {
        if (_currentState == EnemyState.CHASING) {
            var direction = (_target.position - transform.position).normalized;
            if (direction != Vector3.zero) {
                var rotateAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward);
            }
        }
    }

    private void RotateEnemyAutomatically() {
        var moveDirection = _targetPosition - transform.position;
        if (moveDirection != Vector3.zero) {
            var angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void UpdateChasingState() {
        Vector3 direction = (_target.position - transform.position).normalized;
        RotateEnemyTowardsPlayer();
        transform.Translate(direction * (moveSpeed * Time.deltaTime));
    }

    private void UpdateRoamingState() {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition,
            moveSpeed * Time.deltaTime);

        RotateEnemyAutomatically();

        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f) {
            SetRandomTarget();
        }

        var origin = transform.position;
        var direction = _targetPosition - origin;
        var distance = Vector3.Distance(origin, _targetPosition);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, obstacleMask);
        Debug.DrawRay(origin, direction, spriteRenderer.color);

        if (hit.collider != null) {
            if (hit.collider.CompareTag("Enemy") && hit.collider.gameObject != gameObject) {
                float enemyHitDistance = Vector3.Distance(hit.collider.gameObject.transform
                    .position, gameObject.transform.position);

                if (enemyHitDistance < 2f) {
                    //Debug.Log("Recalculate Route");
                    SetRandomTarget();
                }

                // Debug.Log(
                //     $"Enemy detected. CurrentId:{gameObject.GetInstanceID()}, HitId:{hit.collider.gameObject.GetInstanceID()}, Distance:{enemyHitDistance}");
            }
        }
    }

    private IEnumerator SetStateAfterDelay(EnemyState state) {
        float delay = 0f;
        
        Debug.Log($"New {state} Current {_currentState}");

        switch (state) {
            case EnemyState.IDLE:
                delay = Random.Range(3f, 8f);
                break;
            case EnemyState.ROAMING:
                delay = Random.Range(2f, 6f);
                break;
            case EnemyState.CHASING:
                delay = Random.Range(8f, 11f);
                break;
        }

        yield return new WaitForSeconds(delay);
        _currentState = state;

        switch (_currentState) {
            case EnemyState.IDLE:
                SetState(EnemyState.ROAMING);
                break;
            case EnemyState.ROAMING:
                SetState(EnemyState.IDLE);
                break;
            case EnemyState.CHASING:
                SetState(EnemyState.ROAMING);
                break;
        }
    }

    void SetRandomTarget() {
        var randomX = Random.Range(-roamingRange, roamingRange);
        var randomY = Random.Range(-roamingRange, roamingRange);
        _targetPosition = new Vector3(randomX, randomY, 0f);
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