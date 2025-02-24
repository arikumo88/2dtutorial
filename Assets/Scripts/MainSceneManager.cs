using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip fieldBGM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBGM(fieldBGM);
    }

    // Update is called once per frame
    void PlayBGM(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = fieldBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
