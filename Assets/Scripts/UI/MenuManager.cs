using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public PlayerData playerData;
    public GameObject menuPanel;
    public Text playerNameText;
    public Text playerHPText;
    public Button closeButton;

    void Start()
    {
        menuPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseMenu);
    }

    // Update is called once per frame
    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        if (menuPanel.activeSelf)
        {
            UpdateMenuUI();
        }
    }
    
    void UpdateMenuUI()
    {
        playerNameText.text = playerData.playerName;
        playerHPText.text = $"{playerData.currentHp}/{playerData.maxHp}";
    }
}
