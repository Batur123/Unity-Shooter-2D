using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    enum UIState {
        MAIN_MENU,
        LOAD_GAME,
        START_GAME
    };

    public Button[] buttons;
    public Scene scene;
    private UIState _currentState = UIState.MAIN_MENU;

    private void Awake() {
        scene = SceneManager.GetActiveScene();
    }

    void Start() {
        foreach (var button in buttons) {
            button.onClick.AddListener(() => ButtonClicked(button));
        }
    }

    void ButtonClicked(Button buttonClicked) {
        switch (buttonClicked.name) {
            case "NewGameButton":
                SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
                break;
            case "LoadGameButton":
                Debug.Log("Load Game");
                break;
            case "QuitGameButton":
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;
            default:
                Debug.LogWarning("No button found with name " + buttonClicked.name);
                break;
        }
    }
}
