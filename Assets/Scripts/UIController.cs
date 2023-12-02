using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController Instance { get; private set; }

    private Dictionary<TextType, string> _textData = new();
    private Dictionary<TextType, Text> _texts = new();

    public enum TextType {
        AMMO_TEXT,
        COUNTDOWN_TEXT,
        SCOREBOARD_TEXT
    }

    private GameObject _canvasObject;
    private Canvas _canvas;

    private void Awake() {
        LoadUI();
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void LoadUI() {
        LoadCanvas();

        CreateText(new TextSettings {
            Font = Resources.Load<Font>("Fonts/SampleFont"),
            FontSize = 32,
            Alignment = TextAnchor.UpperRight,
            Color = Color.black,
            HorizontalOverflow = HorizontalWrapMode.Overflow,
            VerticalOverflow = VerticalWrapMode.Truncate,
            TextType = TextType.AMMO_TEXT,
            AnchorMin = new Vector2(1, 1),
            AnchorMax = new Vector2(1, 1),
            Pivot = new Vector2(1, 1),
            AnchoredPosition = new Vector2(-10, -10),
            GetTextValue = () => GetTextValue(TextType.AMMO_TEXT)
        }, "AmmoText");

        CreateText(new TextSettings {
            Font = Resources.Load<Font>("Fonts/SampleFont"),
            FontSize = 32,
            Alignment = TextAnchor.MiddleCenter,
            Color = Color.black,
            HorizontalOverflow = HorizontalWrapMode.Overflow,
            VerticalOverflow = VerticalWrapMode.Truncate,
            TextType = TextType.COUNTDOWN_TEXT,
            AnchorMin = new Vector2(0.5f, 1f),
            AnchorMax = new Vector2(0.5f, 1f),
            Pivot = new Vector2(0.5f, 1f),
            AnchoredPosition = new Vector2(0, -10),
            GetTextValue = () => GetTextValue(TextType.COUNTDOWN_TEXT)
        }, "CountdownText");

        CreateText(new TextSettings {
            Font = Resources.Load<Font>("Fonts/SampleFont"),
            FontSize = 32,
            Alignment = TextAnchor.UpperLeft,
            Color = Color.black,
            HorizontalOverflow = HorizontalWrapMode.Overflow,
            VerticalOverflow = VerticalWrapMode.Truncate,
            TextType = TextType.SCOREBOARD_TEXT,
            AnchorMin = new Vector2(0, 1),
            AnchorMax = new Vector2(0, 1),
            Pivot = new Vector2(0, 1),
            AnchoredPosition = new Vector2(10, -10),
            GetTextValue = () => GetTextValue(TextType.SCOREBOARD_TEXT)
        }, "ScoreboardText");
    }

    private void LoadCanvas() {
        _canvasObject = new GameObject("Canvas");
        _canvas = _canvasObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvasObject.AddComponent<CanvasScaler>();
        _canvasObject.AddComponent<GraphicRaycaster>();
    }

    private record TextSettings {
        public Font Font;
        public int FontSize;
        public TextAnchor Alignment;
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 Pivot;
        public Vector2 AnchoredPosition;
        public Color Color;
        public HorizontalWrapMode HorizontalOverflow;
        public VerticalWrapMode VerticalOverflow;
        public Func<string> GetTextValue;
        public TextType TextType;
    }

    private void CreateText(TextSettings settings, string gameObjectName) {
        var newGameObject = new GameObject(gameObjectName);
        newGameObject.transform.SetParent(_canvasObject.transform);

        var newText = newGameObject.AddComponent<Text>();
        newText.font = settings.Font;
        newText.fontSize = settings.FontSize;
        newText.alignment = settings.Alignment;
        newText.color = settings.Color;
        newText.text = settings.GetTextValue();
        newText.horizontalOverflow = settings.HorizontalOverflow;
        newText.verticalOverflow = settings.VerticalOverflow;

        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchorMin = settings.AnchorMin;
        rectTransform.anchorMax = settings.AnchorMax;
        rectTransform.pivot = settings.Pivot;
        rectTransform.anchoredPosition = settings.AnchoredPosition;
        _texts.Add(settings.TextType, newText);
    }

    public void SetTextValue(TextType type, string value) {
        if (!_texts.TryGetValue(type, out var text)) return;
        text.text = value;
        _textData[type] = value;
    }

    public string GetTextValue(TextType type) {
        return _textData.TryGetValue(type, out var value) ? value : null;
    }
}
