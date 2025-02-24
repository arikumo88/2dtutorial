using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string playerName = "Hero";
    public int maxHp = 100;
    public int currentHp = 100;   
}
