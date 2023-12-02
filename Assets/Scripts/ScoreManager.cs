using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance { get; private set; }

    private int _score = 0;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        UIController.Instance.SetTextValue(UIController.TextType.SCOREBOARD_TEXT,
            $"Score: {_score}");
    }

    public void AddScore(int value) {
        _score += value;
        UIController.Instance.SetTextValue(UIController.TextType.SCOREBOARD_TEXT,
            $"Score: {_score}");
    }

    public int GetScore() {
        return _score;
    }
}