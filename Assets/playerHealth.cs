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
        _playerHealth.OnDamageTaken += UpdateText;
        _playerHealth.SetMaxHealth += UpdateText;
    }

    private void UpdateText(float oldHealth, float newHealth)
    {
        Debug.Log("Updated TExt!");
        
        //Sets the health UI to reflect playerHealth
        _healthText.text = newHealth.ToString();
    }

 
}
