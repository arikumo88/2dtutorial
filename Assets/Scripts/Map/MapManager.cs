using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public List<SceneMetadata> sceneMetadataList = new List<SceneMetadata>();
    public Canvas canvas;
    public GameObject buttonPrefab;

    void Awake()
    {
        SetupCanvas();
        SetupButtons();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        for (int i = 0; i < sceneMetadataList.Count && i < 9; i++)
        {
            var key = (Key)((int)Key.Digit1 + i);
            if (keyboard[key].wasPressedThisFrame)
            {
                LoadScene(sceneMetadataList[i].sceneName);
            }
        }
    }

    void SetupCanvas()
    {
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("MapCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            GameObject layoutObj = new GameObject("Buttons");
            layoutObj.transform.SetParent(canvas.transform);
            var rect = layoutObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            layoutObj.AddComponent<VerticalLayoutGroup>();
        }
    }

    void SetupButtons()
    {
        if (buttonPrefab == null)
        {
            buttonPrefab = CreateDefaultButtonPrefab();
        }

        Transform container = canvas.transform.Find("Buttons");
        if (container == null)
        {
            container = canvas.transform;
        }

        for (int i = 0; i < sceneMetadataList.Count; i++)
        {
            GameObject btnObj = Instantiate(buttonPrefab, container);
            Button btn = btnObj.GetComponent<Button>();
            Text txt = btnObj.GetComponentInChildren<Text>();
            if (txt != null)
            {
                txt.text = $"{i + 1}: {sceneMetadataList[i].displayName}";
            }
            int index = i;
            btn.onClick.AddListener(() => LoadScene(sceneMetadataList[index].sceneName));
        }
    }

    GameObject CreateDefaultButtonPrefab()
    {
        GameObject obj = new GameObject("MapButton");
        obj.AddComponent<RectTransform>();
        Button btn = obj.AddComponent<Button>();

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(obj.transform);
        var rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;

        return obj;
    }

    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
