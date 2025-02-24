using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    //敵オブジェクト
    public GameObject enemyObj;
    private SpriteRenderer enemySprite;
    public Text playerHPText;
    public Text enemyHPText;
    public Button attackButton;
    public Button defendButton;

    private int playerHp = 100;
    private int enemyHp = 100;
    private bool isPlayerTurn = true;
    private bool bgmChanged = false;
    public AudioSource audioSource;
    public AudioClip bgmClip;
    public AudioClip bgmClipIntense;
    public float bgmFadeDuration = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (enemyObj == null)
        {
            enemyObj = GameObject.Find("Enemy");
        }
        
        // enemyObjからSpriteRendererコンポーネントを取得
        enemySprite = enemyObj.GetComponent<SpriteRenderer>();
        
        UpdateBattleUI();

        PlayBGM(bgmClip);
    }

    public void OnAttack()
    {
        if (!isPlayerTurn) return;

        enemyHp -= 10;

        if (enemySprite != null)
        {
            StartCoroutine(FlashRed(enemySprite, 0.2f));
        }

        UpdateBattleUI();
        if (enemyHp <= 0)
        {
            EndBattle(true);
            return;
        }

        if (enemyHp <= 50 && !bgmChanged)
        {
            bgmChanged = true;
            StartCoroutine(ChangeBGM());
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

    IEnumerator FlashRed(SpriteRenderer sprite, float duration)
    {
        Color originalColor = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(duration);
        sprite.color = originalColor;
    }

    void PlayBGM(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    IEnumerator ChangeBGM()
    {
        if (audioSource != null)
        {
            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < bgmFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / bgmFadeDuration);
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;

            for (int i = 0; i < 4; i++)
            {
                enemySprite.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                enemySprite.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }

            PlayBGM(bgmClipIntense);
        }
    }
}
