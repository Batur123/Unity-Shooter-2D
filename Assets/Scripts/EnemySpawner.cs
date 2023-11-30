using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public Rigidbody2D player;
    public GameObject enemyPrefab;

    public void Spawn() {
        GameObject newEnemyObj = Instantiate(enemyPrefab,
            GetRandomSpawnPosition(), Quaternion.identity);
        Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();

        int randomNumber = Random.Range(1, 1001);
        if (randomNumber <= 700) {
            newEnemy.enemyType = EnemyTypes.BASIC_ZOMBIE;
            Debug.Log("Basic zombie spawned");
        }
        else if (randomNumber <= 950) {
            newEnemy.enemyType = EnemyTypes.RUNNER_ZOMBIE;
            Debug.Log("Runner zombie spawned");
        }
        else {
            newEnemy.enemyType = EnemyTypes.TANK_ZOMBIE;
            Debug.Log("Tank zombie spawned");
        }
    }

    Vector3 GetRandomSpawnPosition() {
        Vector3 spawnPosition = new Vector3(
            Random.Range(player.position.x - 10f, player.position.x + 10f),
            Random.Range(player.position.y - 10f, player.position.y + 10f), 0f);
        return spawnPosition;
    }
}
