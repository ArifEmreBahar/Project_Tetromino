using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    static AudioPlayer instance;
    public AudioSource audioEffect;
    public AudioSource audioMusic;

    public AudioClip pauseClip;
    public AudioClip resumeClip;
    public AudioClip newGameClip;
    public AudioClip pieceMoveClip;
    public AudioClip pieceRotateClip;
    public AudioClip pieceDropClip;
    public AudioClip levelUpClip;
    public AudioClip lineUpClip;
    [Space]
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip cardHover;
    public AudioClip cardClick;
    [Space]
    public AudioClip bulletFire;
    public AudioClip spellFire;
    public AudioClip joltFire;
    [Space]
    public AudioClip enemyHit;
    [Space]
    public AudioClip playerDamage;
    public AudioClip playerDeath;

    public void PlayPauseClip() => audioEffect.PlayOneShot(pauseClip);
    public void PlayResumeClip() => audioEffect.PlayOneShot(resumeClip);
    public void PlayNewGameClip() => audioEffect.PlayOneShot(newGameClip);
    public void PlayPieceMoveClip() => audioEffect.PlayOneShot(pieceMoveClip);
    public void PlayPieceRotateClip() => audioEffect.PlayOneShot(pieceRotateClip);
    public void PlayPieceDropClip() => audioEffect.PlayOneShot(pieceDropClip);
    public void PlayToggleOnClip() => audioEffect.PlayOneShot(resumeClip);
    public void PlayToggleOffClip() => audioEffect.PlayOneShot(pauseClip);
    public void PlayButtonHoverClip() => audioEffect.PlayOneShot(buttonHover);
    public void PlayButtonClickClip() => audioEffect.PlayOneShot(buttonClick);
    public void PlayCardHoverClip() => audioEffect.PlayOneShot(cardHover);
    public void PlayCardClickClip() => audioEffect.PlayOneShot(cardClick);
    public void PlayLevelUpClip() => audioEffect.PlayOneShot(levelUpClip);
    public void PlayLineClearClip() => audioEffect.PlayOneShot(lineUpClip);

    public void PlayBulletFireClip() => audioEffect.PlayOneShot(bulletFire);
    public void PlaySpellFireClip() => audioEffect.PlayOneShot(spellFire);
    public void PlayJoltFireClip() => audioEffect.PlayOneShot(joltFire);

    public void PlayEnemyHitClip() => audioEffect.PlayOneShot(enemyHit);

    public void PlayPlayerDamageClip() => audioEffect.PlayOneShot(playerDamage);
    public void PlayPlayerDeathClip() => audioEffect.PlayOneShot(playerDeath);

    public static AudioPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioPlayer>();
            }

            return instance;
        }
    }

    void Awake()
    {
        transform.parent = null;
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
