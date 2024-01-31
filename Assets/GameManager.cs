using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _endScoreText;
    [SerializeField] private Animator _deathScreen;
    [SerializeField] private Animator _blackScreen;
    [SerializeField] private Animator _hazardSprite;
    [SerializeField] private GameObject _deathImage;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private GameObject _hurtImage;
    [SerializeField] private AudioSource _watchOut;
    private bool _isSlowedDown = false;
    private float _pauseEndTime = 0;
    public float TimeElapsed { get; private set; } = 0f;
    public bool PlayerDied { get; private set; } = false;
    private bool hasPressedRespawn = false;
    [SerializeField] private AudioSource _beheadingSound;
    public Volume _v;
    public bool isBlooming = false;
    private Bloom _b;
    #region Singleton

    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory present!! NOT GOOD!");
            return;
        }

        instance = this;
    }

    #endregion

    private void OnEnable()
    {
        _playerHealth.OnDeath += OnDeathPlayer;
    }

    private void OnDisable()
    {
        _playerHealth.OnDeath -= OnDeathPlayer;
    }

    void Start()
    {
        _v.profile.TryGet(out _b);
        _hurtImage.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerDied && !hasPressedRespawn)
        {
            if (Input.anyKeyDown)
            {
                hasPressedRespawn = true;
                StartCoroutine(EndGame());
            }
        }
        
        if (PlayerDied) return;

        TimeElapsed += Time.deltaTime;
        _timeText.text = TimeElapsed.ToString("F2");

        
        //counts real time to check when to stop slow motion
        if (_isSlowedDown)
        {
            if (Time.realtimeSinceStartup > _pauseEndTime)
            {
                Debug.Log("BackToNormal");
                Time.timeScale = 1f;
                _isSlowedDown = false;
            }
        }


        if (isBlooming)
        {
            
            _b.intensity.value = Mathf.Lerp(_b.intensity.value, 20f, 0.05f);
        }
        else
        {
            if(_b.intensity.value != 1)
            {
                _b.intensity.value = Mathf.Lerp(_b.intensity.value, 1f, 0.05f);
            }
        }
    }

    private IEnumerator BloomOut()
    {
        
        isBlooming = true;
        yield return new WaitForSeconds(0.5f);
        isBlooming = false;
    }

    public void WatchOut()
    {
        if (!_watchOut.isPlaying)
        {
            _watchOut.pitch = Random.Range(0.8f, 1.2f);
            _watchOut.Play();
        }
    }

    public void CallBloomOut()
    {
        if (!isBlooming)
        {
            StartCoroutine(BloomOut());
        }
    }

    public void CallSlowMotion(float time)
    {
        if (!_isSlowedDown)
        {
            StartCoroutine(SlowMotion(time));
        }
    }
    private IEnumerator SlowMotion(float time)
    {

        Time.timeScale = 0.2f;
        _isSlowedDown = true;
        _pauseEndTime = Time.realtimeSinceStartup + time;


        yield return new WaitForSeconds(0.1f);

    }

    private void OnDeathPlayer()
    {
        //this is where the player will die!
         _beheadingSound.Play();
        StartCoroutine(DeathFadeIn());
        
    }
    private IEnumerator DeathFadeIn()
    {
        yield return new WaitForSeconds(2f);
        PlayerDied = true;
        Debug.Log("playerDied!");
        _deathImage.SetActive(true);
        _deathScreen.SetTrigger("fade");
        _hazardSprite.SetTrigger("fade");
        float f = Mathf.Round(TimeElapsed * 10.0f) * 0.1f;

        _endScoreText.text = f.ToString();
    }

    private IEnumerator EndGame()
    {
        _audio.Play();
        _blackScreen.SetTrigger("fade");
       _hazardSprite.SetTrigger("fade out");
        _deathScreen.SetTrigger("fade out");
        yield return new WaitForSeconds(2f);
        Debug.Log("Game Over");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("testScene");

    }

   
}
