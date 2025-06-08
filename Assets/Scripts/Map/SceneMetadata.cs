using UnityEngine;

[CreateAssetMenu(fileName = "SceneMetadata", menuName = "Game/Scene Metadata")]
public class SceneMetadata : ScriptableObject
{
    public string sceneName;
    public string displayName;
    [TextArea]
    public string description;
}
