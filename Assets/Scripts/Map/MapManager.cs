using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public List<SceneMetadata> sceneMetadataList = new List<SceneMetadata>();

    void Update()
    {
        for (int i = 0; i < sceneMetadataList.Count && i < 9; i++)
        {
            KeyCode key = (KeyCode)((int)KeyCode.Alpha1 + i);
            if (Input.GetKeyDown(key))
            {
                LoadScene(sceneMetadataList[i].sceneName);
            }
        }
    }

    void OnGUI()
    {
        for (int i = 0; i < sceneMetadataList.Count; i++)
        {
            GUILayout.Label($"{i + 1}: {sceneMetadataList[i].displayName}");
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
