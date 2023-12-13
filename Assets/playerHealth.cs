using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Animator _healthAnimator;
    [SerializeField] private Health _playerHealth;

   void Start()
    {
        _playerHealth.OnDamageTaken += DamageUpdateText;
        _playerHealth.SetMaxHealth += StartUpdateText;
        _playerHealth.OnDeath += OnDeathPlayer;
    }

    private void StartUpdateText(float oldHealth, float newHealth)
    {
        //Sets the health UI to reflect playerHealth
        _healthText.text = newHealth.ToString();
    }

    private void DamageUpdateText(float oldHealth, float newHealth)
    {  
        //Sets the health UI to reflect playerHealth
        _healthText.text = newHealth.ToString();
        _healthAnimator.SetTrigger("hurt");
    }

    private void OnDeathPlayer()
    {
        //this is where the player will die!
        Debug.Log("playerDied!");
    }

 
}
