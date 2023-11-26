using System;
using UnityEngine;

public class TimerController : MonoBehaviour {
    public GameStateController gameState;
    [SerializeField] private float countdown;
    public bool timerIsRunning = false;

    void Start() {
        UIController.instance.CreateCountdown();
        UIController.instance.SetCountdownText($"Enemies are going to spawn in {Math.Round(countdown)}");
    }

    void Update() {
        if (timerIsRunning) {
            if (countdown > 0) {
                UIController.instance.SetCountdownText($"Enemies are going to spawn in {Math.Round(countdown)}");
                countdown -= Time.deltaTime;
            }
            else {
                if (gameState) {
                    gameState.InitSpawn("SpawnEnemies", 0, 5);
                }

                UIController.instance.SetCountdownText("");
                DestroyImmediate(gameObject);
            }
        }
    }
}
