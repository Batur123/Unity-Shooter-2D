using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController Instance { get; private set; }

    private Text _ammoText;
    private string _ammoUiText;
    
    private Text _countdownText;
    private string _countdownUiText;
    
    private Text _scoreBoardText;
    private string _scoreBoardUiText;
    
    private GameObject _canvasObject;
    private Canvas _canvas;
    
    private void Awake() {
        LoadUI();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadUI() {
        LoadCanvas();
        
        // TextSettings ammoTextSettings = new TextSettings {
        //     Font = Resources.Load<Font>("Fonts/SampleFont"),
        //     FontSize = 32,
        //     Alignment = TextAnchor.UpperRight,
        //     Color = Color.black,
        //     HorizontalOverflow = HorizontalWrapMode.Overflow,
        //     VerticalOverflow = VerticalWrapMode.Truncate,
        //     GetTextFunction = GetAmmunitionText
        // };
        //
        // TextSettings countdownTextSettings = new TextSettings {
        //     Font = Resources.Load<Font>("Fonts/SampleFont"),
        //     FontSize = 32,
        //     Alignment = TextAnchor.UpperRight,
        //     Color = Color.black,
        //     HorizontalOverflow = HorizontalWrapMode.Overflow,
        //     VerticalOverflow = VerticalWrapMode.Truncate,
        //     GetTextFunction = GetCountdownText
        // };
        //
        // CreateText(ammoTextSettings, "AmmoText");
        // CreateText(countdownTextSettings, "CountdownText");
        
        CreateCountdown();
        CreateAmmoText();
        CreateScoreboardText();
    }

    private void LoadCanvas() {
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

    private string GetAmmunitionText() {
        return _ammoUiText;
    }

    private void UpdateAmmoText() {
        if (_ammoText) {
            _ammoText.text = GetAmmunitionText();
        }
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
    
    public void SetScoreBoardText(string text) {
        _scoreBoardUiText = text;
        UpdateScoreBoardText();
    }

    public string GetScoreBoardText() {
        return _scoreBoardUiText;
    }

    public void UpdateScoreBoardText() {
        if (_scoreBoardText) {
            _scoreBoardText.text = GetScoreBoardText();
        }
    }

    // private record TextSettings {
    //     public Font Font;
    //     public int FontSize;
    //     public TextAnchor Alignment;
    //     public Color Color;
    //     public HorizontalWrapMode HorizontalOverflow;
    //     public VerticalWrapMode VerticalOverflow;
    //     public Func<string> GetTextFunction;
    // }
    //
    // private void CreateText(TextSettings settings, string gameObjectName) {
    //     var newGameObject = new GameObject(gameObjectName);
    //     newGameObject.transform.SetParent(_canvasObject.transform);
    //
    //     var newText = newGameObject.AddComponent<Text>();
    //     newText.font = settings.Font;
    //     newText.fontSize = settings.FontSize;
    //     newText.alignment = settings.Alignment;
    //     newText.color = settings.Color;
    //     newText.text = settings.GetTextFunction();
    //     newText.horizontalOverflow = settings.HorizontalOverflow;
    //     newText.verticalOverflow = settings.VerticalOverflow;
    //
    //     var rectTransform = newText.GetComponent<RectTransform>();
    //     rectTransform.anchorMin = new Vector2(1, 1);
    //     rectTransform.anchorMax = new Vector2(1, 1);
    //     rectTransform.pivot = new Vector2(1, 1);
    //     rectTransform.anchoredPosition = new Vector2(-10, -10);
    // }

    private void CreateAmmoText() {
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
    
    private void CreateScoreboardText() {
        GameObject ammoTextObject = new GameObject("ScoreboardText");
        ammoTextObject.transform.SetParent(_canvasObject.transform);

        _scoreBoardText = ammoTextObject.AddComponent<Text>();
        _scoreBoardText.font = Resources.Load<Font>("Fonts/SampleFont");
        _scoreBoardText.fontSize = 32;
        _scoreBoardText.alignment = TextAnchor.UpperLeft;
        _scoreBoardText.color = Color.black;
        _scoreBoardText.text = GetAmmunitionText();
        _ammoText.horizontalOverflow = HorizontalWrapMode.Overflow;
        _scoreBoardText.verticalOverflow = VerticalWrapMode.Truncate;
        
        RectTransform rectTransform = _scoreBoardText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(10, -10);
    }

    private void CreateCountdown() {
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
}
