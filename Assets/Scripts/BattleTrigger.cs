using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class BattleTrigger : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 0.3f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entrered by: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the battle trigger");
            StartCoroutine(FadeToBattle());
        }
    }

    IEnumerator FadeToBattle()
    {
        float elapsedTime = 0f;
        Color fadeColor = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadePanel.color = fadeColor;
            yield return null;
        }
        SceneManager.LoadScene("BattleScene");
    }
}
