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
    public AudioClip bgmClip;//通常BGM
    public AudioClip bgmClipIntense; //低下時BGM
    public float bgmFadeDuration = 1.0f; //BGMフェード時間
    public AudioClip enemyHitSE; //敵ダメージ時SE
    public AudioClip playerHitSE; //プレイヤーダメージ時SE

    //フラッシュエフェクト
    private Camera mainCamera;
    public float flashDuration = 0.1f;
    private Color originalColor;

    public GameObject dialogBox;
    public Text dialogText;
    public Button dialogButton;
    public bool isDialogOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (enemyObj == null)
        {
            enemyObj = GameObject.Find("Enemy");
        }
        
        // enemyObjからSpriteRendererコンポーネントを取得
        enemySprite = enemyObj.GetComponent<SpriteRenderer>();

        mainCamera = Camera.main;
        originalColor = mainCamera.backgroundColor;
        
        UpdateBattleUI();

        PlayBGM(bgmClip);

        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
        }

        if (dialogButton != null)
        {
            dialogButton.onClick.AddListener(() => StartCoroutine(ProceedAfterDialog()));
        }
    }

    public void OnAttack()
    {
        if (!isPlayerTurn) return;

        enemyHp -= 10;

        //敵ダメージ時SE再生
        if (audioSource != null && enemyHitSE != null)
        {
            audioSource.PlayOneShot(enemyHitSE);
        }

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

        //敵のHPが50%以下になったらダイアログを表示してBGMを変更
        if (enemyHp <= 50 && !bgmChanged)
        {
            bgmChanged = true;
            isPlayerTurn = false; //プレイヤーのターンを一時停止
            StartCoroutine(TriggerEnemyLowHPEvent());
            return;
        }

        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator TriggerEnemyLowHPEvent()
    {
        //画面を赤くフラッシュ
        StartCoroutine(FlashScreen(flashDuration));

        //エネミーを点滅
        for (int i = 0; i < 4; i++)
        {
            enemySprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            enemySprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        //bgmをフェードアウトして無音に
        yield return StartCoroutine(FadeOutBGM());

        //ダイアログを表示
        ShowDialog("HPが50%以下になりました!");
    }

    IEnumerator ProceedAfterDialog()
    {
        CloseDialog();
        isDialogOpen = false;

        //BGMを再生
        yield return null;
        PlayBGM(bgmClipIntense);

        //敵のターンを開始
        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());

        yield break;
    }

    IEnumerator FadeOutBGM()
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
        }
    }

    public void ShowDialog(string message)
    {
        if (dialogBox != null && dialogText != null)
        {
            dialogText.text = message;  
            dialogBox.SetActive(true);
            isDialogOpen = true;
        }
    }

    public void CloseDialog()
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
            isDialogOpen = false;
        }
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

        if (audioSource != null && playerHitSE != null)
        {
            audioSource.PlayOneShot(playerHitSE);
        }

        //画面を赤くフラッシュ
        StartCoroutine(FlashScreen(flashDuration));

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
        }
        else
        {
            Debug.Log("プレイヤーの敗北!");
        }
        StartCoroutine(ReturnToMainScene());
    }

    IEnumerator ReturnToMainScene()
    {
        if (audioSource != null)
        {
            yield return StartCoroutine(FadeOutBGM(1.0f));
        } 

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator FadeOutBGM(float duration)
    {
        if (audioSource != null)
        {
            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
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

    //BGMを変更するコルーチン
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
            audioSource.volume = startVolume; //ボリュームを元に戻す

            for (int i = 0; i < 4; i++)

            //低下時BGMを再生  
            PlayBGM(bgmClipIntense);
        }
    }

    IEnumerator FlashScreen(float duration)
    {
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = Color.red; //画面を赤くする
            yield return new WaitForSeconds(duration);
            mainCamera.backgroundColor = originalColor; //元の色に戻す
        }
    }
}
