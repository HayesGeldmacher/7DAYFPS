using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private bool _isDead = false;
    
    // These are properties because I want everybody to have public access to read them but nobody else can set them
    public float MaxHealth { get => _maxHealth; }
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value;}
    public bool IsDead { get => _isDead; }

    // Events for other scripts to subscribe to
    public delegate void HealthChangeHandler(float oldHealth, float newHealth);
    public delegate void DeathHandler();
    public HealthChangeHandler OnDamageTaken;
    public HealthChangeHandler OnHeal;
    public DeathHandler OnDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        // Take the damage
        float previousHealth = _currentHealth;
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        OnDamageTaken?.Invoke(previousHealth, _currentHealth);

        // Check if dead
        if (CurrentHealth <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float healAmount)
    {
        if (_isDead) return;

        // Heal
        float previousHealth = _currentHealth;
        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        OnHeal?.Invoke(previousHealth, _currentHealth);
    }
}
