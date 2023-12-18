using UnityEngine;

public class GameStateController : MonoBehaviour {
    public Transform player;
    public EnemySpawner spawner;
    public int count = 0;

    public void InitSpawn(string methodName, float time, float repeatRate) {
        Debug.Log("Enemy spawn has been initialized");
        InvokeRepeating(methodName, time, repeatRate);
    }

    public void RemoveSpawn(string methodName) {
        Debug.Log($"{methodName} spawner has been deactivated.");
        CancelInvoke(methodName);
    }

    void SpawnEnemies() {
        spawner.Spawn();
        count++;

        if (count > 0) {
            CancelInvoke("SpawnEnemies");
        }
    }
}
