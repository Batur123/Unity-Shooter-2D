using UnityEngine;

namespace Utils {
public class SpawnUtils {
    public static Vector3 GetRandomSpawnPosition(Rigidbody2D rigidbody2D) {
        Vector3 playerPosition = rigidbody2D.position;
        Vector3 spawnPosition;

        do {
            var randomX = Random.Range(playerPosition.x - 10f, playerPosition.x + 10f);
            var randomY = Random.Range(playerPosition.y - 10f, playerPosition.y + 10f);
            spawnPosition = new Vector3(randomX, randomY, 0f);
        } while (Vector3.Distance(rigidbody2D.position, spawnPosition) <= 5f);

        return spawnPosition;
    }
}
}