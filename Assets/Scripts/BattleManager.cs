using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public Text playerHPText;
    public Text enemyHPText;
    public Button attackButton;
    public Button defendButton;

    private int playerHp = 100;
    private int enemyHp = 100;
    private bool isPlayerTurn = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateBattleUI();
    }

    public void OnAttack()
    {
        if (!isPlayerTurn) return;

        enemyHp -= 10;
        UpdateBattleUI();
        if (enemyHp <= 0)
        {
            EndBattle(true);
            return;
        }

        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    public void OnDefend()
    {
        if (!isPlayerTurn) return;
        UpdateBattleUI();
        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        playerHp -= 5;
        UpdateBattleUI();
        if (playerHp <= 0)
        {
            EndBattle(false);
            yield break;
        }

        isPlayerTurn = true;
    }

    void UpdateBattleUI()
    {
        playerHPText.text = "Player HP: " + playerHp;
        enemyHPText.text = "Enemy HP: " + enemyHp;
    }

    void EndBattle(bool playerWin)
    {
        if (playerWin)
        {
            Debug.Log("プレイヤーの勝利!");
            StartCoroutine(ReturnToMainScene());
        }
        else
        {
            Debug.Log("プレイヤーの敗北!");
            StartCoroutine(ReturnToMainScene());
        }
    }

    IEnumerator ReturnToMainScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene");
    }
}
