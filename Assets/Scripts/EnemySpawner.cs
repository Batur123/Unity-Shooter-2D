using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public Rigidbody2D player;
    public GameObject enemyPrefab;

    public void Spawn() {
        GameObject newEnemy = Instantiate(enemyPrefab,
            GetRandomSpawnPosition(), Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition() {
        Vector3 spawnPosition = new Vector3(
            Random.Range(player.position.x - 10f, player.position.x + 10f),
            Random.Range(player.position.y - 10f, player.position.y + 10f), 0f);
        return spawnPosition;
    }
}
