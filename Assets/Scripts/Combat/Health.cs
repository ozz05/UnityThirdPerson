using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action  OnDeath;
    public bool IsAlive => _currentHealth > 0;
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private bool _isInvulnerable;
    private void Start() {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_currentHealth == 0){return;}
        if (_isInvulnerable){return;}
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        Debug.LogWarning("Health: " + _currentHealth);
        OnTakeDamage?.Invoke();
        if (_currentHealth == 0)
        {
            HandleDeath();
        }
    }
    public void SetInvulnerable(bool state)
    {
        _isInvulnerable = state;
    }
    private void HandleDeath()
    {
        OnDeath?.Invoke();
    }
}
