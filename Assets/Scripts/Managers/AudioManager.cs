using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip bgm;

    [Header("Player SFX")]
    public AudioClip[] walkSounds;
    public AudioClip[] jumpSounds;
    public AudioClip pickUpItem;

    [Header("Dialogue SFX")]
    public AudioClip dialogueStart;
    public AudioClip dialogueNext;

    [Header("Banner SFX")]
    public AudioClip questBanner;
    public AudioClip itemBanner;

    private int lastWalkIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (bgmSource != null && bgm != null)
        {
            bgmSource.clip = bgm;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        int index = Random.Range(0, clips.Length);

        sfxSource.PlayOneShot(clips[index]);
    }

    public void PlayWalk()
    {
        if (walkSounds.Length == 0)
            return;

        int index;

        do
        {
            index = Random.Range(0, walkSounds.Length);
        }
        while (walkSounds.Length > 1 && index == lastWalkIndex);

        lastWalkIndex = index;

        sfxSource.PlayOneShot(walkSounds[index]);
    }

    public void PlayJump()
    {
        PlayRandom(jumpSounds);
    }

    public void PlayPickUpItem() => PlaySFX(pickUpItem);
    public void PlayDialogueStart() => PlaySFX(dialogueStart);
    public void PlayDialogueNext() => PlaySFX(dialogueNext);
    public void PlayQuestBanner() => PlaySFX(questBanner);
    public void PlayItemBanner() => PlaySFX(itemBanner);
}