using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;

    private Text _ammoText;
    private string _ammoUiText;
    private Text _countdownText;
    private string _countdownUiText;
    private GameObject _canvasObject;
    private Canvas _canvas;

    private void Awake() {
        if (!instance) instance = this;
    }

    private void Start() {
        _canvasObject = new GameObject("Canvas");
        _canvas = _canvasObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvasObject.AddComponent<CanvasScaler>();
        _canvasObject.AddComponent<GraphicRaycaster>();
    }

    public void SetAmmunitionText(int ammo, int maxAmmo) {
        _ammoUiText = $"Ammo: {ammo}/{maxAmmo}";
        UpdateAmmoText();
    }

    public void SetAmmunitionText(string customText) {
        _ammoUiText = customText;
        UpdateAmmoText();
    }

    public string GetAmmunitionText() {
        return _ammoUiText;
    }

    public void UpdateAmmoText() {
        if (_ammoText) {
            _ammoText.text = GetAmmunitionText();
        }
    }

    public void CreateAmmoText() {
        GameObject ammoTextObject = new GameObject("AmmoText");
        ammoTextObject.transform.SetParent(_canvasObject.transform);

        _ammoText = ammoTextObject.AddComponent<Text>();
        _ammoText.font = Resources.Load<Font>("Fonts/SampleFont");
        _ammoText.fontSize = 32;
        _ammoText.alignment = TextAnchor.UpperRight;
        _ammoText.color = Color.black;
        _ammoText.text = GetAmmunitionText();
        _ammoText.horizontalOverflow = HorizontalWrapMode.Overflow;
        _ammoText.verticalOverflow = VerticalWrapMode.Truncate;
        
        RectTransform rectTransform = _ammoText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        rectTransform.anchoredPosition = new Vector2(-10, -10);
    }

    public void CreateCountdown() {
        GameObject countdownTextObject = new GameObject("CountdownText");
        countdownTextObject.transform.SetParent(_canvasObject.transform);

        _countdownText = countdownTextObject.AddComponent<Text>();
        _countdownText.font = Resources.Load<Font>("Fonts/SampleFont");
        _countdownText.fontSize = 32;
        _countdownText.alignment = TextAnchor.MiddleCenter;
        _countdownText.color = Color.black;
        _countdownText.text = GetCountdownText();
        _countdownText.horizontalOverflow = HorizontalWrapMode.Overflow;
        _countdownText.verticalOverflow = VerticalWrapMode.Truncate;
        
        RectTransform rectTransform =
            _countdownText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = new Vector2(0, -10);
    }

    public void SetCountdownText(string text) {
        _countdownUiText = text;
        UpdateCountdownText();
    }

    public string GetCountdownText() {
        return _countdownUiText;
    }

    public void UpdateCountdownText() {
        if (_countdownText) {
            _countdownText.text = GetCountdownText();
        }
    }
}
