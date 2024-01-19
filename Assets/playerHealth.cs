using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _percentageText;
    [SerializeField] private Animator _healthAnimator;
    [SerializeField] private AudioSource _deathAudio;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Transform _shotGun;
    [SerializeField] private Transform _rifle;
    [SerializeField] private AudioSource _hurtSound;
    [SerializeField] private Animator _hurtScreenAnim;

    private Color _originalColor;
    private float _actualHealth;
    private float _displayedHealth;

    private void OnEnable()
    {
        _playerHealth.OnDamageTaken += DamageUpdateText;
        _playerHealth.OnDeath += OnDeathPlayer;
    }

    private void OnDisable()
    {
        _playerHealth.OnDamageTaken -= DamageUpdateText;
        _playerHealth.OnDeath -= OnDeathPlayer;
    }

    private void Start()
    {
        _actualHealth = _playerHealth.CurrentHealth;
        _healthText.text = _actualHealth.ToString();
        _originalColor = _healthText.color;
    }
    
    private void Update()
    {
        _displayedHealth = Mathf.Lerp(_displayedHealth, _actualHealth, Time.deltaTime * 5f);
        _healthText.text = (Mathf.RoundToInt(_displayedHealth).ToString() + "%");

        if(_playerHealth.CurrentHealth <= 5)
        {
            _hurtScreenAnim.SetTrigger("hurt");
        }

    }

    private void DamageUpdateText(float oldHealth, float newHealth)
    {  
        //Sets the health UI to reflect playerHealth
        _actualHealth = newHealth;
        StopAllCoroutines();
        StartCoroutine(FlashHealth());
        _healthAnimator.SetTrigger("hurt");
        _hurtScreenAnim.SetTrigger("hurt");
        _hurtSound.Play();
    }

    private void OnDeathPlayer()
    {
        //this is where the player will die!
        Debug.Log("playerDied!");
        if (_deathAudio.isPlaying == false)
        {
        _deathAudio.Play();
        }

        Debug.Log("Played Death Audio!");
        Transform _cam = Camera.main.transform;
        _cam.SetParent(null);
        _shotGun.SetParent(null);
        _rifle.SetParent(null);
        Rigidbody _rigid = _cam.gameObject.AddComponent<Rigidbody>();
        Vector3 _direction = new Vector3(10, 10, 10);
         _rigid.velocity = (Vector3.up * 2);
        _rigid.angularVelocity = Vector3.right * 1.0f;
        Destroy(transform.gameObject);
    }

    private IEnumerator FlashHealth()
    {
        _healthText.color = _originalColor;
        _percentageText.color = _originalColor;
        yield return new WaitForSeconds(0.1f);
        _healthText.color = Color.red;
        _percentageText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _healthText.color = _originalColor;
        _percentageText.color = _originalColor;
    }
}
