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
        Debug.Log("Started!");
        _playerHealth.OnDamageTaken += DamageUpdateText;
        _playerHealth.SetMaxHealth += StartUpdateText;
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

 
}
