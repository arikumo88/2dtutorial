using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public PlayerData playerData;
    public GameObject menuPanel;
    public Text playerNameText;
    public Text playerHPText;
    public Button closeButton;

    private RectTransform menuRect;
    private bool isMenuOpen = false;
    private Vector2 openPosition;
    private Vector2 closedPosition;
    private float animationSpeed = 0.2f;
    private float offsetX = 130f;

    void Start()
    {
        menuRect = menuPanel.GetComponent<RectTransform>();

        closedPosition = new Vector2(-menuRect.rect.width, menuRect.anchoredPosition.y);
        openPosition = new Vector2(offsetX, menuRect.anchoredPosition.y);

        menuRect.anchoredPosition = closedPosition;

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ToggleMenu);
        }
    }

    // Update is called once per frame
    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            StartCoroutine(SlideMenu(closedPosition));
        }
        else
        {
            UpdateMenuUI();
            StartCoroutine(SlideMenu(openPosition));
        }
        isMenuOpen = !isMenuOpen;
    }

    IEnumerator SlideMenu(Vector2 targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 startPosition = menuRect.anchoredPosition;

        while (elapsedTime < animationSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationSpeed;
            menuRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        menuRect.anchoredPosition = targetPosition;
    }
    
    void CloseMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    void UpdateMenuUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = playerData.playerName;
        }
        if (playerHPText != null)
        {
            playerHPText.text = $"{playerData.currentHp}/{playerData.maxHp}";
        }
    }
}
