using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    public int damageModifier;
}

public enum EnemyState {
    IDLE,
    ROAMING,
    CHASING,
}

public class Enemy : MonoBehaviour {
    public NavMeshAgent agent;

    public CharacterStats characterStats;
    public SpriteRenderer spriteRenderer;
    private Transform _target;

    public EnemyTypes enemyType;
    private EnemyState _currentState;
    private Coroutine _currentCoroutine = null;

    public int health = 20;
    public int baseHealth = 20;
    public float moveSpeed = 3f;

    public int baseDamageAmount = 5;
    public int damageAmount;

    private float timeSinceLastAttack = 0f;

    private readonly float _roamingRange = 10f;
    private readonly float _extendedRoamingRange = 30f;
    private readonly float _rareCaseProbability = 0.03f; // 3% chance

    public int baseKillScore = 1;
    public int killScore;

    private Vector3 _targetPosition;
    public float distanceThreshold = 5f;

    private float _hiddenTime;

    private static readonly Dictionary<EnemyTypes, EnemyConfig> Configs =
        new() {
            {
                EnemyTypes.BASIC_ZOMBIE, new EnemyConfig {
                    color = Color.red, extraKillScore = 0, healthModifier = 0, damageModifier = 0, speedModifier = 0f
                }
            }, {
                EnemyTypes.RUNNER_ZOMBIE, new EnemyConfig {
                    color = Color.green, extraKillScore = 2, healthModifier = -5, damageModifier = -2, speedModifier = 2f
                }
            }, {
                EnemyTypes.TANK_ZOMBIE, new EnemyConfig {
                    color = Color.black, extraKillScore = 9, healthModifier = +30, damageModifier = 15, speedModifier = -1f
                }
            }
        };

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Character");
        characterStats = playerObject.GetComponent<CharacterStats>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        _target = GameObject.FindGameObjectWithTag("Character").transform;

        EnemyConfig config = Configs[enemyType];

        spriteRenderer.color = config.color;
        killScore = baseKillScore + config.extraKillScore;
        health = baseHealth + config.healthModifier;
        moveSpeed += config.speedModifier;
        agent.speed = moveSpeed;
        damageAmount = baseDamageAmount + config.damageModifier;

        _hiddenTime = 0f;

        GetRandomPointInWorld();
        agent.SetDestination(_targetPosition);
        SetState(EnemyState.ROAMING);
    }

    void RotateEnemy() {
        Vector2 lookDir = agent.velocity.normalized;
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update() {
        DetectEnemies();
        RotateEnemy();

        switch (_currentState) {
            case EnemyState.IDLE:
                UpdateIdleState();
                break;
            case EnemyState.ROAMING:
                UpdateRoamingState();
                break;
            case EnemyState.CHASING:
                UpdateChasingState();
                break;
        }
    }

    private void SetState(EnemyState newState) {
        if(_currentCoroutine != null) {
            StopCoroutine(_currentCoroutine);
        }

        _currentState = newState;
        switch (_currentState) {
            case EnemyState.IDLE:
                _currentCoroutine = StartCoroutine(SetStateAfterDelay(EnemyState.ROAMING, Random.Range(4f, 8f)));
                break;
            case EnemyState.ROAMING:
                _currentCoroutine = StartCoroutine(SetStateAfterDelay(EnemyState.IDLE, 3f));
                break;
            case EnemyState.CHASING:
                _currentCoroutine = StartCoroutine(SetStateAfterDelay(EnemyState.ROAMING, 10f));
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
            RaycastHit2D hit = Physics2D.Raycast(enemyPos, direction, distance);
            if (hit.collider != null && (hit.collider.CompareTag("Structure"))) {
                return;
            }

            Debug.DrawRay(enemyPos, direction, Color.yellow);

            if (_currentState != EnemyState.CHASING) {
                _currentState = EnemyState.CHASING;
                RotateEnemyTowardsPlayer();
            }
        }
    }

    private void RotateEnemyTowardsPlayer() {
        var enemyPos = transform.position;
        var characterPos = _target.position;
        var distance = Vector2.Distance(enemyPos, characterPos);

        if (IsPlayerVisible(enemyPos, characterPos, distance)) {
            var direction = (characterPos - transform.position).normalized;
            if (direction != Vector3.zero) {
                var rotateAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward);
            }
        }
        else {
            RotateEnemy();
        }
    }

    private void UpdateIdleState() {
        // Implement behavior for the IDLE state here
    }

    private void UpdateRoamingState() {
        // Implement behavior for the ROAMING state here
        if (!agent.hasPath) {
            // Set a random destination for roaming
            Vector3 randomDestination = GetRandomPointInWorld();
            agent.SetDestination(randomDestination);
        }
    }

    private void UpdateChasingState() {
        RotateEnemyTowardsPlayer();

        var enemyPos = transform.position;
        var characterPos = _target.position;
        var distance = Vector2.Distance(enemyPos, characterPos);
        
        if (IsPlayerVisible(enemyPos, characterPos, distance)) {
            agent.SetDestination(characterPos);
            _hiddenTime = 0f;
            Debug.DrawRay(enemyPos, (characterPos - enemyPos), Color.yellow);
        }
        else {
            _hiddenTime += Time.deltaTime;
            agent.SetDestination(characterPos);

            if (_hiddenTime >= Random.Range(7f, 11f)) {
                SetState(EnemyState.ROAMING);
            }
        }
    }
    
    private bool IsPlayerVisible(Vector2 enemyPos, Vector2 characterPos, float distance) {
        var direction = (characterPos - enemyPos).normalized;
        var dotProduct = Vector2.Dot(direction, transform.right);
  
        return dotProduct > 0.9f && (distance < distanceThreshold) && !IsObstructed(enemyPos, direction, distance);
    }

    private bool IsObstructed(Vector2 origin, Vector2 direction, float distance) {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance);
  
        return hit.collider != null && hit.collider.CompareTag("Structure");
    }

    private IEnumerator SetStateAfterDelay(EnemyState state, float delay) {
        yield return new WaitForSeconds(delay);
        SetState(state);
    }

    private Vector2 GetRandomPointInWorld() {
        float rareCaseChance = Random.Range(0f, 1f);

        Vector2 randomPosition = (rareCaseChance < _rareCaseProbability) switch {
            true => new Vector2(Random.Range(-_extendedRoamingRange, _extendedRoamingRange), Random.Range(-_extendedRoamingRange, _extendedRoamingRange)),
            false => new Vector2(Random.Range(-_roamingRange, _roamingRange), Random.Range(-_roamingRange, _roamingRange))
        };

        Vector2 currentPosition = transform.position;

        _targetPosition = currentPosition + randomPosition;
        return _targetPosition;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Character")) {
            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack >= 1f) {
                Attack();
                timeSinceLastAttack = 0f;
            }
        }
    }

    private void Attack() {
        characterStats.TakeDamage(damageAmount);
    }

    private bool ShouldSpawnLoot() {
        return Random.value <= 0.8f;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) KillEnemy();
    }

    private void KillEnemy() {
        ScoreManager.Instance.AddScore(killScore);

        if (ShouldSpawnLoot()) {
            GameObject spawnerObject = GameObject.FindGameObjectWithTag("Spawner");
            LootSpawner lootSpawner = spawnerObject.GetComponent<LootSpawner>();
            lootSpawner.SpawnRandomItem(transform.position);
        }

        Destroy(gameObject);
    }
}