using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    public Rigidbody2D player;
    public GameObject enemyPrefab;
    private readonly float _safeDistance = 5f;
    [SerializeField]
    public List<string> Enemies = new();

    public void Spawn() {
        GameObject newEnemyObj = Instantiate(enemyPrefab,
            GetRandomSpawnPosition(), Quaternion.identity);
        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();

        var randomNumber = Random.Range(1, 1001);
        newEnemy.enemyType = randomNumber switch {
            <= 700 => EnemyTypes.BASIC_ZOMBIE,
            <= 950 => EnemyTypes.RUNNER_ZOMBIE,
            _ => EnemyTypes.TANK_ZOMBIE
        };
    
        newEnemyObj.name = $"{newEnemy.enemyType.ToString()}";
        Enemies.Add(newEnemyObj.GetInstanceID().ToString());
    }

    private Vector3 GetRandomSpawnPosition() {
        Vector3 playerPosition = player.position;
        Vector3 spawnPosition;

        do {
            spawnPosition =
                new Vector3(Random.Range(playerPosition.x - 10f, playerPosition.x + 10f),
                    Random.Range(playerPosition.y - 10f, playerPosition.y + 10f), 0f);
        } while (Vector3.Distance(player.position, spawnPosition) <= _safeDistance);

        return spawnPosition;
    }
}
