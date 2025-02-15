using UnityEngine;
using UnityEngine.SceneManagement;
public class BattleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entrered by: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the battle trigger");
            SceneManager.LoadScene("BattleScene");
        }
    }
}
