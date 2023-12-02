using System;
using UnityEngine;

public class TimerController : MonoBehaviour {
    public GameStateController gameState;
    [SerializeField] private float countdown;
    public bool timerIsRunning = false;

    void Start() {
        UIController.Instance.SetTextValue(UIController.TextType.COUNTDOWN_TEXT,
            $"Enemies are going to spawn in {Math.Round(countdown)}");
    }

    void Update() {
        if (timerIsRunning) {
            if (countdown > 0) {
                UIController.Instance.SetTextValue(UIController.TextType.COUNTDOWN_TEXT,
                    $"Enemies are goingto spawn in {Math.Round(countdown)}");
                countdown -= Time.deltaTime;
            }
            else {
                if (gameState) {
                    gameState.InitSpawn("SpawnEnemies", 0, 5);
                }

                UIController.Instance.SetTextValue(UIController.TextType.COUNTDOWN_TEXT, "");
                DestroyImmediate(gameObject);
            }
        }
    }
}
